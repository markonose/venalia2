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
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
    public class FileService
    {
        private const string _connectionString = @"Server=localhost;Database=Venalia;Trusted_Connection=True;";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FileService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        ////public Guid Download(Guid id)
        ////{
        ////    var file = new WebApi.Entities.File()
        ////    {
        ////        Type = type
        ////    };
        ////    file.ApplyCreatedFields(_httpContextAccessor.HttpContext);

        ////    var fileContent = formFile;
        ////    var folderName = Path.Combine("Resources", "Images", file.Id.ToString());
        ////    Directory.CreateDirectory(folderName);

        ////    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        ////    var fileName = ContentDispositionHeaderValue.Parse(fileContent.ContentDisposition).FileName.Trim('"');
        ////    var fullPath = Path.Combine(pathToSave, fileName);
        ////    ////var dbPath = Path.Combine(folderName, fileName);

        ////    using (var stream = new FileStream(fullPath, FileMode.Create))
        ////    {
        ////        fileContent.CopyTo(stream);
        ////    }

        ////    using (var connection = new SqlConnection(_connectionString))
        ////    {
        ////        connection.Open();

        ////        using var transaction = connection.BeginTransaction();

        ////        string sql =
        ////            @"INSERT INTO [dbo].[Files]
        ////            (
        ////                [Id],
        ////                [EntityId],
        ////                [Type],
        ////                [Created],
        ////                [CreatedBy],
        ////                [Modified],
        ////                [ModifiedBy]
        ////            )
        ////            VALUES
        ////            (
        ////                @Id,
        ////                @EntityId,
        ////                @Type,
        ////                @Created,
        ////                @CreatedBy,
        ////                @Modified,
        ////                @ModifiedBy
        ////            );";

        ////        transaction.Execute(sql, file);

        ////        transaction.Commit();
        ////    }

        ////    return file.Id;
        ////}

        public Guid Upload(FileType type, IFormFile formFile)
        {
            var file = new WebApi.Entities.File()
            {
                Type = type
            };
            file.ApplyCreatedFields(_httpContextAccessor.HttpContext);

            var fileContent = formFile;
            var folderName = Path.Combine("Resources", "Images", file.Id.ToString());
            Directory.CreateDirectory(folderName);

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fileName = $"{type}1.jpg";
            var fullPath = Path.Combine(pathToSave, fileName);
            ////var dbPath = Path.Combine(folderName, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                fileContent.CopyTo(stream);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                string sql =
                    @"INSERT INTO [dbo].[Files]
                    (
                        [Id],
                        [Type],
                        [Created],
                        [CreatedBy],
                        [Modified],
                        [ModifiedBy]
                    )
                    VALUES
                    (
                        @Id,
                        @Type,
                        @Created,
                        @CreatedBy,
                        @Modified,
                        @ModifiedBy
                    );";

                transaction.Execute(sql, file);

                transaction.Commit();
            }

            return file.Id;
        }
    }
}
