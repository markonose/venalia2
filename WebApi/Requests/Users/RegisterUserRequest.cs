using FluentValidation;
using WebApi.Enums;

namespace WebApi.Requests.Users
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordRepeat { get; set; }

        public UserType Type { get; set; }
    }

    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PasswordRepeat).NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.PasswordRepeat);
        }
    }
}