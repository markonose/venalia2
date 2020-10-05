using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebApi.Enums;
using WebApi.Requests.Installers;
using WebApi.Responses.Installers;
using WebApi.Shared;

namespace WebApi.Services
{
    public class InstallerService
    {
        private const string _connectionString = @"Server=localhost;Database=Venalia;Trusted_Connection=True;";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public InstallerService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

    }
}
