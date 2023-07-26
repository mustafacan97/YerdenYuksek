using FluentValidation;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmailAndPassword;

public sealed class GetCustomerByEmailAndPasswordQueryValidator : AbstractValidator<GetCustomerByEmailAndPasswordQuery>
{
    #region Constructors and Destructors

    public GetCustomerByEmailAndPasswordQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .MaximumLength(128);

        RuleFor(q => q.Password)
            .NotEmpty().WithMessage("Password can not be empty!")
            .NotNull().WithMessage("Password can not be null!");
    }

    #endregion
}
