using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using WebApi.Extensions;

namespace WebApi.Requests.Installations
{
    public class CreateInstallationRequest
    {
        public Guid? BusinessId { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public InstallationDetails[] Installations { get; set; }
        public string Remark { get; set; }
        public DateTime PreferedDate { get; set; }
    }

    public class CreateInstallationRequestValidator : AbstractValidator<CreateInstallationRequest>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateInstallationRequestValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            RuleFor(x => x.BusinessId).NotNull().When(x => _httpContextAccessor.HttpContext.User.IsAdministrator());
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.PostCode).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Region).NotEmpty();
            RuleFor(x => x.CustomerFirstName).NotEmpty();
            RuleFor(x => x.CustomerLastName).NotEmpty();
            RuleFor(x => x.CustomerEmail).NotEmpty();
            RuleFor(x => x.CustomerPhoneNumber).NotEmpty();
            RuleFor(x => x.PreferedDate).GreaterThanOrEqualTo(DateTime.Now.AddDays(7));
        }
    }

    public class InstallationDetails
    {
        public string AcUnitBrand { get; set; }
        public decimal AcUnitPower { get; set; }
        public decimal? AcIndoorUnitHeight { get; set; }
        public decimal? AcOutdoorUnitHeight { get; set; }
    }

    public class InstallationDetailsValidator : AbstractValidator<InstallationDetails>
    {
        public InstallationDetailsValidator()
        {
            RuleFor(x => x.AcUnitBrand).NotEmpty();
            RuleFor(x => x.AcUnitPower).GreaterThan(0);
            RuleFor(x => x.AcOutdoorUnitHeight).GreaterThan(4).When(x => x.AcOutdoorUnitHeight.HasValue);
        }
    }
}