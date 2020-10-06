using System;
using WebApi.Enums;
using WebApi.Shared;

namespace WebApi.Entities
{
    public class Penalty : EntityBase
    {
        public Guid UserId { get; set; }
        public PenaltyType Type { get; set; }
    }
}
