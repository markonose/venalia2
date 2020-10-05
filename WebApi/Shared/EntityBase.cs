using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Shared
{
    public abstract class EntityBase : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public Guid ModifiedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(ParentCreatedByUsers))]
        public User CreatedByUser { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual List<User> ParentCreatedByUsers { get; set; }

        [ForeignKey(nameof(ModifiedBy))]
        [InverseProperty(nameof(ParentModifiedByUsers))]
        public User ModifiedByUser { get; set; }
        [ForeignKey(nameof(ModifiedBy))]
        public virtual List<User> ParentModifiedByUsers { get; set; }
    }
}
