using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Enums;

namespace WebApi.Requests.Installations
{
    public class ListInstallationsRequest
    {
        [FromQuery]
        public InstallationStatus? Status { get; set; }

        [FromQuery]
        public bool IncludeDeleted { get; set; } = false;

        [FromQuery]
        public string Query { get; set; } = "";

        [FromQuery]
        public string FieldName { get; set; } = "Modified";

        [FromQuery]
        public OrderByDirection Direction { get; set; } = OrderByDirection.Asc;

        [FromQuery]
        public int? Limit { get; set; } = 10;

        [FromQuery]
        public long? Offset { get; set; } = 0;
    }

    public class ListInstallationsRequestValidator : AbstractValidator<ListInstallationsRequest>
    {
        public ListInstallationsRequestValidator()
        {
            RuleFor(x => x.Limit).GreaterThanOrEqualTo(10).LessThanOrEqualTo(100).When(x => x.Limit.HasValue);
            RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).When(x => x.Offset.HasValue);
        }
    }
}
