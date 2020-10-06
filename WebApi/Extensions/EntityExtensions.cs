using Microsoft.AspNetCore.Http;
using System;
using WebApi.Interfaces;

namespace WebApi.Extensions
{
    public static class EntityExtensions
    {
        public static void ApplyCreatedFields(this IEntity entity, HttpContext context)
        {
            entity.Id = Guid.NewGuid();
            entity.Created = DateTime.Now;
            entity.CreatedBy = context.User.GetId();
            entity.ApplyUpdatedFields(context);
        }

        public static void ApplyUpdatedFields(this IEntity entity, HttpContext context)
        {
            entity.Modified = DateTime.Now;
            entity.ModifiedBy = context.User.GetId();
        }

        public static void RemoveAdministrationData(this IEntity entity)
        {
            entity.CreatedBy = Guid.Empty;
            entity.Modified = DateTime.MinValue;
            entity.ModifiedBy = Guid.Empty;
        }
    }
}
