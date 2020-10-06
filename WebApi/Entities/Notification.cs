using System;
using WebApi.Enums;
using WebApi.Shared;

namespace WebApi.Entities
{
    public class Notification : EntityBase
    {
        public Guid UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
    }
}
