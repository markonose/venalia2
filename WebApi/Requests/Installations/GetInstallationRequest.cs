using FluentValidation;
using System;

namespace WebApi.Requests.Installations
{
    public class GetInstallationRequest
    {
        public Guid Id { get; set; }
    }

    public class GetInstallationRequestValidator : AbstractValidator<GetInstallationRequest>
    {
        public GetInstallationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}