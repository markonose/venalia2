using FluentValidation;
using System;
using WebApi.Enums;

namespace WebApi.Requests.Users
{
    public class ChangePasswordUserRequest
    {
        public Guid Id { get; set; }

        public string PasswordOld { get; set; }

        public string Password { get; set; }

        public string PasswordRepeat { get; set; }
    }

    public class ChangePasswordUserRequestValidator : AbstractValidator<ChangePasswordUserRequest>
    {
        public ChangePasswordUserRequestValidator()
        {
            RuleFor(x => x.PasswordOld).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PasswordRepeat).NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.PasswordRepeat);
        }
    }
}