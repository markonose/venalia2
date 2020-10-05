using FluentValidation;

namespace WebApi.Requests.Users
{
    public class ResetPasswordUserRequest
    {
        public string Password { get; set; }

        public string PasswordRepeat { get; set; }
    }

    public class ResetPasswordUserRequestValidator : AbstractValidator<ResetPasswordUserRequest>
    {
        public ResetPasswordUserRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PasswordRepeat).NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.PasswordRepeat);
        }
    }
}