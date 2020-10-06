using AutoMapper;
using Dapper;
using Dapper.Transaction;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Exceptions;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Requests.Installations;
using WebApi.Responses.Installation;
using WebApi.Shared;

namespace WebApi.Services
{
    public class InstallationService
    {
        private const string _connectionString = @"Server=localhost;Database=Venalia;Trusted_Connection=True;";

        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInstallationRepository<Installation> _installationsRepository;
        private readonly IMapper _mapper;

        public InstallationService(IEmailService emailService, IHttpContextAccessor httpContextAccessor, IInstallationRepository<Installation> installationsRepository, IMapper mapper)
        {
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _installationsRepository = installationsRepository;
            _mapper = mapper;
        }

        public void Approve(ApproveInstallationRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET
                    InstallerId = NULL,
                    [Status] = @Status,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            transaction.Execute(sql, new
            {
                Status = InstallationStatus.Completed,
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });

            sql =
                @$"INSERT INTO [dbo].[Notifications]
                   SET
                    UserId = NULL,
                    [Type] = ,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            transaction.Execute(sql, new
            {
                Status = InstallationStatus.Completed,
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });

            _emailService.Send("test@eklip.si", "test@eklip.si"/*_httpContextAccessor.HttpContext.User.GetEmail()*/, "Potrditev", $"Montaza: {request.Id} je bila potrjena");

            transaction.Commit();
        }

        public void Assign(AssignInstallationRequest request)
        {
            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET
                    InstallerId = @InstallerId,
                    [Status] = @Status,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new
            {
                request.InstallerId,
                Status = InstallationStatus.Assigned,
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });
        }

        public void Cancel(CancelInstallationRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET
                    InstallerId = NULL,
                    [Status] = @Status,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            transaction.Execute(sql, new
            {
                Status = InstallationStatus.Completed,
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });

            if (_httpContextAccessor.HttpContext.User.IsBusiness())
            {
                Installation installation;

                sql = @"SELECT
                            Id, 
                            BusinessId,
                            InstallerId,
                            Address,
                            PostCode,
                            City,
                            Region,
                            CustomerFirstName,
                            CustomerLastName,
                            CustomerEmail,
                            CustomerPhoneNumber,
                            IsExpress,
                            Remark,
                            Deadline,
                            [Status],
                            Created,
                            IsDeleted
                        FROM [dbo].[Installations]
                        WHERE Id = @id";

                installation = transaction.Query<Installation>(sql, new { request.Id }).FirstOrDefault();

                _emailService.Send("test@eklip.si", "test@eklip.si", "Potrditev", $"Montaza: {request.Id} je bila potrjena");
            }

            transaction.Commit();
        }

        public void Complete(CompleteInstallationRequest request)
        {
            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET [Status] = @Status,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                   WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new
            {
                Status = InstallationStatus.Assigned,
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });
        }

        public void Create(Installation installation)
        {
            installation.ApplyCreatedFields(_httpContextAccessor.HttpContext);
            installation.Quantity = installation.InstallationDetails.Length;
            installation.Status = InstallationStatus.Unassigned;

            foreach (var installationDetail in installation.InstallationDetails)
            {
                installationDetail.ApplyCreatedFields(_httpContextAccessor.HttpContext);
            }

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            string sql =
                @"INSERT INTO [dbo].[Installations]
                (
                    Id,
                    BusinessId,
                    Address,
                    StreetNumber,
                    PostCode,
                    City,
                    Region,
                    CustomerFirstName,
                    CustomerLastName,
                    CustomerEmail,
                    CustomerPhoneNumber,
                    IsExpress,
                    Quantity,
                    Remark,
                    Deadline,
                    [Status],
                    Created,
                    CreatedBy,
                    Modified,
                    ModifiedBy
                )
                Values
                (
                    @Id,
                    @BusinessId,
                    @Address,
                    @StreetNumber,
                    @PostCode,
                    @City,
                    @Region,
                    @CustomerFirstName,
                    @CustomerLastName,
                    @CustomerEmail,
                    @CustomerPhoneNumber,
                    @IsExpress,
                    @Quantity,
                    @Remark,
                    @Deadline,
                    @Status,
                    @Created,
                    @CreatedBy,
                    @Modified,
                    @ModifiedBy
                );";

            transaction.Execute(sql, new
            {
                installation
            });

            sql = @"INSERT INTO [dbo].[InstallationsDetails]
                (
	              [IdInstallation]
                  ,[AcUnitBrand]
                  ,[AcUnitPower]
                  ,[AcOutdoorUnitHeight]
                  ,[Created]
                  ,[CreatedBy]
                  ,[Modified]
                  ,[ModifiedBy]
                )
                Values
                (
	              @IdInstallation,
                  @AcUnitBrand,
                  AcUnitPower,
                  AcOutdoorUnitHeight,
                  Created,
                  CreatedBy,
                  Modified,
                  ModifiedBy,
                );";

            foreach (var installationDetail in installation.InstallationDetails)
            {
                transaction.Execute(sql, new
                {
                    installationDetail
                });
            }

            transaction.Commit();
        }

        public void Delete(DeleteInstallationRequest request)
        {
            string sql =
                @"UPDATE [dbo].[Installations]
                SET
                    IsDeleted = 1,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new
            {
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });
        }

        public GetInstallationResponseData GetById(GetInstallationRequest request)
        {
            Installation installation;

            string sql = @"SELECT
                            Id, 
                            BusinessId,
                            InstallerId,
                            Address,
                            StreetNumber,
                            PostCode,
                            City,
                            Region,
                            CustomerFirstName,
                            CustomerLastName,
                            CustomerEmail,
                            CustomerPhoneNumber,
                            AcUnitBrand,
                            AcUnitPower,
                            AcOutdoorUnitHeight,
                            AcIndoorUnitHeight,
                            IsExpress,
                            Remark,
                            Deadline,
                            [Status],
                            Created,
                            IsDeleted,
                            Quantity
                        FROM [dbo].[Installations]
                        WHERE Id = @id";

            using (var conn = new SqlConnection(_connectionString))
            {
                installation = conn.Query<Installation>(sql, new { request.Id }).FirstOrDefault();
            }

            if (installation == null)
            {
                throw new NotFoundException($"Installation with the {request.Id} does not exist");
            }

            if (_httpContextAccessor.HttpContext.User.IsInstaller())
            {
                if (installation.InstallerId != _httpContextAccessor.HttpContext.User.GetId() && 
                    (installation.Status != InstallationStatus.Assigned || installation.Status != InstallationStatus.Completed))
                {
                    throw new UnauthorizedException($"Unauthorized");
                }

                if (installation.Status == InstallationStatus.Completed)
                {
                    installation.RemovePersonalData();
                }
            }
            else if (_httpContextAccessor.HttpContext.User.IsBusiness() &&
                installation.BusinessId != _httpContextAccessor.HttpContext.User.GetId())
            {
                throw new UnauthorizedException($"Unauthorized");
            }

            if (!_httpContextAccessor.HttpContext.User.IsAdministrator())
            {
                installation.RemoveAdministrationData();
            }

            return _mapper.Map<GetInstallationResponseData>(installation);
        }

        public (List<ListInstallationResponseData> installations, Pagination pagination) List(ListInstallationsRequest request)
        {
            List<ListInstallationResponseData> installations;
            var pagination = new Pagination()
            {
                Limit = request.Limit,
                Offset = request.Offset
            };

            using (var conn = new SqlConnection(_connectionString))
            {
                string sql =
                    @"SELECT
                        Id,
                        BusinessId,
                        InstallerId,
                        Address,
                        StreetNumber,
                        PostCode,
                        City,
                        Region,
                        CustomerFirstName,
                        CustomerLastName,
                        CustomerEmail,
                        CustomerPhoneNumber,
                        IsExpress,
                        Remark,
                        Deadline,
                        [Status],
                        IsDeleted,
                        Created,
                        Quantity,
                    FROM [dbo].[Installations] ";

                string whereClause = "";
                whereClause += $"WHERE (CustomerFirstName LIKE '%{request.Query}%' OR CustomerLastName LIKE '%{request.Query}%' OR Address LIKE '%{request.Query}%' OR City LIKE '%{request.Query}%' OR Region LIKE '%{request.Query}%') ";

                if (_httpContextAccessor.HttpContext.User.IsInstaller())
                {
                    whereClause += $"AND Region = '{(int)_httpContextAccessor.HttpContext.User.GetRegion()}' ";
                }

                if (_httpContextAccessor.HttpContext.User.IsBusiness())
                {
                    whereClause += $"AND BusinessId = '{_httpContextAccessor.HttpContext.User.GetId()}' ";
                }

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

                installations = conn.Query<ListInstallationResponseData>(sql, new { request.Status, pagination.Offset, pagination.Limit }).ToList();

                sql = $"SELECT COUNT(1) FROM [dbo].[Installations] {whereClause};";
                pagination.NumberOfPages = (long)Math.Ceiling(conn.Query<long>(sql, new { request.Status }).Single() / (decimal)request.Limit);
            }

            if (_httpContextAccessor.HttpContext.User.IsInstaller())
            {
                foreach (var installation in installations)
                {
                    if (installation.Status == InstallationStatus.Completed)
                    {
                        installation.CustomerFirstName = "";
                        installation.CustomerLastName = "";
                        installation.CustomerEmail = "";
                        installation.CustomerPhoneNumber = "";
                        installation.StreetNumber = "";
                    }
                }
            }

            return (installations, pagination);
        }

        public void Start(StartInstallationRequest request)
        {
            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET [Status] = @Status
                   WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new { Status = InstallationStatus.InProgress, request.Id });
        }

        public void Unassign(UnassignInstallationRequest request)
        {
            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET
                    InstallerId = NULL,
                    [Status] = @Status
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new { Status = InstallationStatus.Unassigned, request.Id });
        }

        public void Undelete(UndeleteInstallationRequest request)
        {
            string sql =
                @"UPDATE [dbo].[Installations]
                SET
                    IsDeleted = 0,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new
            {
                Modified = DateTime.Now,
                ModifiedBy = _httpContextAccessor.HttpContext.User.GetId(),
                request.Id
            });
        }

        public void Update(Installation installation)
        {
            installation.ApplyUpdatedFields(_httpContextAccessor.HttpContext);
            installation.Quantity = installation.InstallationDetails.Length;

            foreach (var installationDetail in installation.InstallationDetails)
            {
                installationDetail.ApplyUpdatedFields(_httpContextAccessor.HttpContext);
            }

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            string sql =
                @"UPDATE [dbo].[Installations]
                    SET
                        BusinessId = @BusinessId,
                        Address = @Address,
                        StreetNumber = @StreetNumber,
                        PostCode = @PostCode,
                        City = @City,
                        Region = @Region,
                        CustomerFirstName = @CustomerFirstName,
                        CustomerLastName = @CustomerLastName,
                        CustomerEmail = @CustomerEmail,
                        CustomerPhoneNumber = @CustomerPhoneNumber,
                        AcUnitBrand = @AcUnitBrand,
                        AcUnitPower = @AcUnitPower,
                        AcOutdoorUnitHeight = @AcOutdoorUnitHeight,
                        AcIndoorUnitHeight = @AcIndoorUnitHeight,
                        IsExpres = @IsExpress,
                        Remark = @Remark,
                        Deadline = @Deadline,
                        Modified = @Modified,
                        ModifiedBy = @ModifiedBy,
                        Quantity = @Quantity
                    WHERE Id = @Id;";

            transaction.Execute(sql, new
            {
                installation
            });

            sql = @"DELETE FROM [dbo].[InstallationsDetails]
                    WHERE IdInstallation = @Id";

            transaction.Execute(sql, installation.Id);

            sql = @"INSERT INTO [dbo].[InstallationsDetails]
                (
                    [InstallationId]
                    ,[AcUnitBrand]
                    ,[AcUnitPower]
                    ,[AcOutdoorUnitHeight]
                    ,[Created]
                    ,[CreatedBy]
                    ,[Modified]
                    ,[ModifiedBy]
                )
                Values
                (
                    @InstallationId,
                    @AcUnitBrand,
                    @AcUnitPower,
                    @AcOutdoorUnitHeight,
                    @Created,
                    @CreatedBy,
                    @Modified,
                    @ModifiedBy,
                );";

            foreach (var installationDetail in installation.InstallationDetails)
            {
                transaction.Execute(sql, new
                {
                    installationDetail
                });
            }

            transaction.Commit();
        }
    }
}
