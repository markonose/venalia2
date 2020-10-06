using System;
using WebApi.Enums;
using WebApi.Shared;

namespace WebApi.Entities
{
    public class File : EntityBase
    {
        public Guid EntityId { get; set; }
        public FileType Type { get; set; }
    }
}
