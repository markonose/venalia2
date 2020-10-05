using FluentValidation;
using System;

namespace WebApi.Requests.Installations
{
    public class DeleteInstallationRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteInstallationRequestValidator : AbstractValidator<DeleteInstallationRequest>
    {
        public DeleteInstallationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}