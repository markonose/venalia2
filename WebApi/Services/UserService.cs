using AutoMapper;
using Dapper;
using Dapper.Transaction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Exceptions;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Requests.Users;
using WebApi.Responses.Users;
using WebApi.Shared;

namespace WebApi.Services
{
    public class UserService
    {
        private const string _connectionString = @"Server=localhost;Database=Venalia;Trusted_Connection=True;";

        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(IEmailService emailService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public void Activate(ActivateUserRequest request)
        {
            string sql =
                @$"UPDATE [dbo].[Users] SET [Status] = @Status
                  WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new { Status = UserStatus.Active, request.Id });
        }

        public void ChangePassword(ChangePasswordUserRequest request)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

            using var connection = new SqlConnection(_connectionString);
            string sql =
                @"UPDATE [dbo].[Users]
                  SET
                    PasswordHash = @PasswordHash
                  WHERE Id = @Id
	            ;";

            connection.Execute(sql, new
            {
                passwordHash,
                request.Id
            });
        }

        public void Confirm(ConfirmUserRequest request)
        {
            string sql =
                @"UPDATE [dbo].[Users]
                SET
                    IsConfirmed = 1
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new { request.Id });
        }

        public void Delete(DeleteUserRequest request)
        {
            string sql =
                @"UPDATE [dbo].[Users]
                SET
                    IsDeleted = 1
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new { request.Id });
        }

        public EmailExistsUserReponseData EmailExists(EmailExistsUserRequest request)
        {
            EmailExistsUserReponseData response;

            string sql = "SELECT Email FROM [dbo].[Users] WHERE Email = @Email";
            using (var conn = new SqlConnection(_connectionString))
            {
                var email = conn.Query<string>(sql, new { request.Email }).FirstOrDefault();

                response = new EmailExistsUserReponseData()
                {
                    EmailExists = !string.IsNullOrEmpty(email)
                };
            }

            return response;
        }

        public GetUserResponseData GetById(GetUserRequest request)
        {
            GetUserResponseData user;

            string sql = "SELECT Id, [Type], Email, FirstName, LastName, BusinessName, Address, PostCode, City, Region, IsTaxablePerson, VATNumber, IBAN, IsTaxablePerson, [Status], IsDeleted FROM [dbo].[Users] WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString))
            {
                user = conn.Query<GetUserResponseData>(sql, new { request.Id }).FirstOrDefault();
            }

            if (user == null)
            {
                throw new NotFoundException($"User with the id: '{request.Id}' does not exist");
            }

            return user;
        }

        public GetContextUserResponseData GetContext()
        {
            GetContextUserResponseData user;

            var sql = "SELECT Id, Email, Type, [Status] FROM [dbo].[Users] WHERE Id = @Id;";
            using (var conn = new SqlConnection(_connectionString))
            {
                user = conn.Query<GetContextUserResponseData>(sql, new { Id = _httpContextAccessor.HttpContext.User.GetId() }).Single();
            }

            return user;
        }

        [Authorize(Roles = "Administrator")]
        public (List<ListUsersResponseData> users, Pagination pagination) List(ListUsersRequest request)
        {
            List<ListUsersResponseData> users;
            var pagination = new Pagination()
            {
                Limit = request.Limit,
                Offset = request.Offset
            };

            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT Id, [Type], Email, FirstName, LastName, BusinessName, Address, PostCode, City, Region, IsTaxablePerson, VATNumber, IBAN, IsTaxablePerson, [Status], IsDeleted FROM [dbo].[Users] WHERE [Type] = @Type ";
                string whereClause = "";

                whereClause += $"AND (IsNull(FirstName, '') LIKE '%{request.Query}%' OR IsNull(LastName, '') LIKE '%{request.Query}%' OR IsNull(BusinessName, '') LIKE '%{request.Query}%' OR IsNull(City, '') LIKE '%{request.Query}%') ";

                if (request.Status.HasValue)
                {
                    whereClause += $"AND [Status] = @Status ";
                }

                if (!request.IncludeDeleted)
                {
                    whereClause += "AND IsDeleted = 0 ";
                }

                sql += whereClause;
                sql += $"ORDER BY {request.FieldName} {(request.Direction == OrderByDirection.Asc ? "ASC" : "DESC")} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

                users = conn.Query<ListUsersResponseData>(sql, new { request.Type, request.Status, pagination.Offset, pagination.Limit }).ToList();

                sql = $"SELECT COUNT(1) FROM [dbo].[Users] WHERE Type = @Type {whereClause};";
                pagination.NumberOfPages = (long)Math.Ceiling(conn.Query<long>(sql, new { request.Type, request.Status }).Single() / (decimal)request.Limit);
            }

            return (users, pagination);
        }

        public async Task<LoginUserResponseData> Login(LoginUserRequest request)
        {
            User user = null;

            string sql = "SELECT * FROM [dbo].[Users] WHERE Email = @Email AND IsConfirmed = 1 AND IsDeleted = 0";
            using (var conn = new SqlConnection(_connectionString))
            {
                user = conn.Query<User>(sql, new
                {
                    request.Email
                }).FirstOrDefault();
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email or password is incorrect.");
            }

            await AuthenticateAsync(user);

            return _mapper.Map<LoginUserResponseData>(user);
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public RegisterUserResponseData Register(RegisterUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);
            user.ApplyCreatedFields(_httpContextAccessor.HttpContext);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();
                string sql =
                    @"INSERT INTO [dbo].[Users]
                    (
	                    Id,
	                    Email,
	                    PasswordHash,
	                    Type,
	                    FirstName,
	                    LastName,
	                    BusinessName,
	                    Address,
	                    PostCode,
	                    City,
	                    Region,
                        IsTaxablePerson,
	                    VATNumber,
	                    IBAN,
	                    [Status],
                        IsConfirmed,
                        IsDeleted,
	                    Created,
	                    CreatedBy,
	                    Modified,
	                    ModifiedBy
                    )
                    Values
                    (
	                    @Id,
	                    @Email,
	                    @PasswordHash,
	                    @Type,
	                    @FirstName,
	                    @LastName,
	                    @BusinessName,
	                    @Address,
	                    @PostCode,
	                    @City,
	                    @Region,
                        @IsTaxablePerson,
	                    @VATNumber,
	                    @IBAN,
	                    @Status,
                        @IsConfirmed,
                        @IsDeleted,
	                    @Created,
	                    @CreatedBy,
	                    @Modified,
	                    @ModifiedBy
                    );";

                transaction.Execute(sql, new
                {
                    user
                });

                _emailService.Send("test@eklip.si", "test@eklip.si", "Potrditev", user.Id.ToString());

                transaction.Commit();
            }

            return _mapper.Map<RegisterUserResponseData>(user);
        }

        public void Undelete(UndeleteUserRequest request)
        {
            string sql =
                @"UPDATE [dbo].[Users]
                SET
                    IsDeleted = 0
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new { request.Id });
        }

        public async Task<UpdateUserResponseData> Update(Guid id, UpdateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.ApplyUpdatedFields(_httpContextAccessor.HttpContext);
            user.Id = id;
            user.Status = UserStatus.Ready;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                string sql =
                    @"UPDATE [dbo].[Users]
                      SET
                        [FirstName]  = @FirstName,
                        [LastName]  = @LastName,
                        [BusinessName]  = @BusinessName,
                        [Address]  = @Address,
                        [PostCode]  = @PostCode,
                        [City]  = @City,
                        [Region]  = @Region,
                        [VATNumber]  = @VATNumber,
                        [IBAN]  = @IBAN,
                        [IsTaxablePerson] = @IsTaxablePerson,
                        [Status] = @Status,
                        [Modified]  = @Modified,
                        [ModifiedBy] = @ModifiedBy
                       WHERE Id = @Id
	                ;";

                transaction.Execute(sql, new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.BusinessName,
                    user.Address,
                    user.PostCode,
                    user.City,
                    user.Region,
                    user.VATNumber,
                    user.IBAN,
                    user.IsTaxablePerson,
                    user.Status,
                    user.Modified,
                    user.ModifiedBy,
                });

                _emailService.Send("test@eklip.si", "test@eklip.si", "Uporabnik je pripravljen", user.Id.ToString());

                transaction.Commit();
            }

            await Logout();
            await AuthenticateAsync(user);

            return _mapper.Map<UpdateUserResponseData>(user);
        }

        [Authorize(Roles = "Administrator")]
        public void UpdatePaymentTerm(Guid id, int paymentTerm)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql =
                @"UPDATE [dbo].[Users]
                      SET
                        [PaymentTerm]  = @PaymentTerm,
                        Modified = @Modified,
                        ModifiedBy = @ModifiedBy
                       WHERE Id = @Id
	                ;";

            connection.Execute(sql, new
            {
                paymentTerm,
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                id
            });
        }

        private async Task AuthenticateAsync(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Type.ToString()),
                    new Claim("BusinessName", user.BusinessName),
                    new Claim("Region", user.Status.ToString()),
                    new Claim("Status", user.Status.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
