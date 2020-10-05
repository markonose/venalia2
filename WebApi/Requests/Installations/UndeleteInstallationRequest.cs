using FluentValidation;
using System;

namespace WebApi.Requests.Installations
{
    public class UndeleteInstallationRequest
    {
        public Guid Id { get; set; }
    }

    public class UndeleteInstallationRequestValidator : AbstractValidator<DeleteInstallationRequest>
    {
        public UndeleteInstallationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}