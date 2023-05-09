using FluentValidation;

namespace eShop.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");

            //RuleFor(x => x.Dob).GreaterThan(DateTime.Now.Date).WithMessage("Dob is not valid");
            //RuleFor(x => x.Dob).LessThan(new DateTime(1900,1,1).Date).WithMessage("Dob is not valid");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.PassWord).NotEmpty().WithMessage("PassWord is required");
            RuleFor(x => x.ConfirmPass).NotEmpty().WithMessage("Confirm Pass is required");
        }
    }
}