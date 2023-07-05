using FluentValidation;
using YerdenYuksek.Core.Domain.Customers;
using YerdenYuksek.Web.Contract.Models.Customer;

namespace YerdenYuksek.Web.Validators.Authentication;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    #region Constructure and Destructure

    public RegisterModelValidator(CustomerSettings customerSettings)
    {
        RuleFor(q => q.Email)
            .NotEmpty().WithMessage("Email can not be empty!")
            .NotNull().WithMessage("Email can not be null!")
            .EmailAddress().WithMessage("Email is not valid!");

        RuleFor(q => q.Password)
            .NotEmpty().WithMessage("Password can not be empty!")
            .NotNull().WithMessage("Password can not be null!")
            .MinimumLength(customerSettings.PasswordMinLength).WithMessage($"Password length must at least {customerSettings.PasswordMinLength} characters!")
            .MaximumLength(customerSettings.PasswordMaxLength).WithMessage($"Password length must have maximum {customerSettings.PasswordMinLength} characters!"); ;

        if (customerSettings.PasswordRequireUppercase)
        {
            RuleFor(q => q.Password).Matches("^(?=.*?[A-Z])").WithMessage("Password must contain uppercase letter!");
        }

        if (customerSettings.PasswordRequireLowercase)
        {
            RuleFor(q => q.Password).Matches("^(?=.*?[a-z])").WithMessage("Password must contain lower letter!");
        }

        if (customerSettings.PasswordRequireDigit)
        {
            RuleFor(q => q.Password).Matches("^(?=.*?[0-9])").WithMessage("Password must contain digit!");
        }

        if (customerSettings.PasswordRequireNonAlphanumeric)
        {
            RuleFor(q => q.Password).Matches("^(?=.*?[#?!@$%^&*-])").WithMessage("Password must contain non alphanumeric character!");
        }
    }

    #endregion
}
