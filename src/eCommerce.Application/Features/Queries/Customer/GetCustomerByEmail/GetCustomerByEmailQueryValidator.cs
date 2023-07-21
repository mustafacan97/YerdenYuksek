using FluentValidation;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmail;

public sealed class GetCustomerByEmailQueryValidator : AbstractValidator<GetCustomerByEmailQuery>
{
    #region Constructors and Destructors

    public GetCustomerByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .MaximumLength(128);
    }

    #endregion
}
