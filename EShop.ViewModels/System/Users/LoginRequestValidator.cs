using FluentValidation;

namespace eShop.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("PassWord is required");
        }
    }
}
