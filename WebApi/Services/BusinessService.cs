using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebApi.Enums;
using WebApi.Requests.Businesses;
using WebApi.Responses;
using WebApi.Responses.Businesses;
using WebApi.Shared;

namespace WebApi.Services
{
    public class BusinessService
    {
        private const string _connectionString = @"Server=localhost;Database=Venalia;Trusted_Connection=True;";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BusinessService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public (List<ListBusinessResponseData> business, Pagination pagination) List(ListBusinessesRequest request)
        {
            List<ListBusinessResponseData> business;
            var pagination = new Pagination()
            {
                Limit = request.Limit,
                Offset = request.Offset
            };

            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT Id, Name FROM [dbo].[Users] WHERE Type = {UserType.Installer} ";
                business = conn.Query<ListBusinessResponseData>(sql, new { pagination.Offset, pagination.Limit }).ToList();

                if (!request.IncludeDeleted)
                {
                    sql += "AND IsDeleted = 0 ";
                }

                sql += $"ORDER BY @FieldName DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;";

                sql = $"SELECT COUNT(1) FROM [dbo].[Users] WHERE Type = {UserType.Installer};";
                pagination.NumberOfPages = (long) Math.Ceiling(conn.Query<long>(sql).Single() / (decimal)request.Limit);
            }

            return (business, pagination);
        }
    }
}
