using eCommerce.Core.Primitives;
using MediatR;

namespace eCommerce.Application.Features.Queries.Customers.GetCustomerByEmailAndPassword;

public sealed record GetCustomerByEmailAndPasswordQuery(string Email, string Password) : IRequest<Result<GetCustomerByEmailAndPasswordResponse>>
{
}
