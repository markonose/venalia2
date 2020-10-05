using System;

namespace WebApi.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
