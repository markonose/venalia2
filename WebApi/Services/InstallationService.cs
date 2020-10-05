using AutoMapper;
using Dapper;
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

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInstallationRepository<Installation> _installationsRepository;
        private readonly IMapper _mapper;

        public InstallationService(IHttpContextAccessor httpContextAccessor, IInstallationRepository<Installation> installationsRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _installationsRepository = installationsRepository;
            _mapper = mapper;
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
            string sql =
                @$"UPDATE [dbo].[Installations]
                   SET
                    InstallerId = NULL,
                    [Status] = @Status,
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
            installation.Status = InstallationStatus.Unassigned;

            string sql =
                @"INSERT INTO [dbo].[Installations]
                (
                    Id,
                    BusinessId,
                    Address,
                    PostCode,
                    City,
                    Region,
                    CustomerFirstName,
                    CustomerLastName,
                    CustomerEmail,
                    CustomerPhoneNumber,
                    AcUnitBrand,
                    AcUnitPower,
                    AcIndoorUnitHeight,
                    AcOutdoorUnitHeight,
                    IsExpress,
                    Remark,
                    PreferedDate,
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
                    @PostCode,
                    @City,
                    @Region,
                    @CustomerFirstName,
                    @CustomerLastName,
                    @CustomerEmail,
                    @CustomerPhoneNumber,
                    @AcUnitBrand,
                    @AcUnitPower,
                    @AcIndoorUnitHeight,
                    @AcOutdoorUnitHeight,
                    @IsExpress,
                    @Remark,
                    @PreferedDate,
                    @Status,
                    @Created,
                    @CreatedBy,
                    @Modified,
                    @ModifiedBy
                );";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new
            {
                installation.Id,
                installation.BusinessId,
                installation.Address,
                installation.PostCode,
                installation.City,
                installation.Region,
                installation.CustomerFirstName,
                installation.CustomerLastName,
                installation.CustomerEmail,
                installation.CustomerPhoneNumber,
                installation.AcUnitBrand,
                installation.AcUnitPower,
                installation.AcIndoorUnitHeight,
                installation.AcOutdoorUnitHeight,
                installation.IsExpress,
                installation.Remark,
                installation.PreferedDate,
                installation.Status,
                installation.IsDeleted,
                installation.Created,
                installation.CreatedBy,
                installation.Modified,
                installation.ModifiedBy,
            });
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
            GetInstallationResponseData installation;

            string sql = "SELECT Id, BusinessId, InstallerId, Address, PostCode, City, Region, CustomerFirstName, CustomerLastName, CustomerEmail, CustomerPhoneNumber, AcUnitBrand, AcUnitPower, AcOutdoorUnitHeight, AcIndoorUnitHeight, IsExpress, Remark, PreferedDate, [Status], Created, IsDeleted FROM [dbo].[Installations] WHERE Id = @id";
            using (var conn = new SqlConnection(_connectionString))
            {
                installation = conn.Query<GetInstallationResponseData>(sql, new { request.Id }).FirstOrDefault();
            }

            if (installation == null)
            {
                throw new NotFoundException($"Installation with the {request.Id} does not exist");
            }


            if (_httpContextAccessor.HttpContext.User.IsInstaller() &&
                installation.InstallerId != _httpContextAccessor.HttpContext.User.GetId() &&
                installation.Status != InstallationStatus.Assigned)
            {
                throw new UnauthorizedException($"Unauthorized");
            }
            else if(_httpContextAccessor.HttpContext.User.IsBusiness() &&
                installation.BusinessId != _httpContextAccessor.HttpContext.User.GetId())
            {
                throw new UnauthorizedException($"Unauthorized");
            }

            return installation;
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
                        PostCode,
                        City,
                        Region,
                        CustomerFirstName,
                        CustomerLastName,
                        CustomerEmail,
                        CustomerPhoneNumber,
                        AcUnitBrand,
                        AcUnitPower,
                        AcIndoorUnitHeight,
                        AcOutdoorUnitHeight,
                        IsExpress,
                        Remark,
                        PreferedDate,
                        [Status],
                        IsDeleted,
                        Created
                    FROM [dbo].[Installations] ";

                string whereClause = "";
                whereClause += $"WHERE (CustomerFirstName LIKE '%{request.Query}%' OR CustomerLastName LIKE '%{request.Query}%' OR Address LIKE '%{request.Query}%' OR City LIKE '%{request.Query}%' OR Region LIKE '%{request.Query}%') ";

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
            string sql =
                @"UPDATE [dbo].[Installations]
                SET
                    BusinessId = @BusinessId,
                    Address = @Address,
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
                    PreferedDate = @PreferedDate,
                    Modified = @Modified,
                    ModifiedBy = @ModifiedBy
                WHERE Id = @Id;";

            using var conn = new SqlConnection(_connectionString);
            conn.Execute(sql, new
            {
                installation.Id,
                installation.BusinessId,
                installation.Address,
                installation.PostCode,
                installation.City,
                installation.Region,
                installation.CustomerFirstName,
                installation.CustomerLastName,
                installation.CustomerEmail,
                installation.CustomerPhoneNumber,
                installation.AcUnitBrand,
                installation.AcUnitPower,
                installation.AcOutdoorUnitHeight,
                installation.AcIndoorUnitHeight,
                installation.IsExpress,
                installation.Remark,
                installation.PreferedDate,
                installation.Modified,
                installation.ModifiedBy
            });
        }
    }
}
