using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using WebApi.Enums;
using WebApi.Requests.Users;
using WebApi.Responses.Users;
using WebApi.Services;
using WebApi.Shared;

namespace WebApi.Apis
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ApiBase
    {
        private const string _connectionString = @"Server=localhost;Database=Venalia;Trusted_Connection=True;";
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{entityId}/list")]
        public List<WebApi.Entities.File> List(Guid entityId)
        {
            List<WebApi.Entities.File> files;

            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT Id, [Type] FROM WHERE [EntityId] = @EntityId";
                files = conn.Query<WebApi.Entities.File>(sql, new { entityId }).ToList();
            }

            return files;
        }

        [HttpPost("types/{fileType}")]
        public Guid Upload(FileType fileType)
        {
            return _fileService.Upload(fileType, Request.Form.Files[0]);
        }

        [HttpGet("{id}")]
        public IActionResult Download(Guid id)
        {
            var folderName = Path.Combine("Resources", "Images", id.ToString());
            var filePath = Directory.GetFiles(folderName)[0];

            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
        }
    }
}
