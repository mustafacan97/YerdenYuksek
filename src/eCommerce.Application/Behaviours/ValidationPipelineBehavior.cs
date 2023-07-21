using eCommerce.Core.Primitives;
using FluentValidation;
using MediatR;

namespace eCommerce.Application.Behaviours;

public sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    #region Fields

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    #endregion

    #region Constructure and Destructure

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) 
    {
        _validators = validators;
    }

    #endregion

    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        Error[] errors = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    #endregion

    #region Methods

    private static TResult CreateValidationResult<TResult>(Error[] errors) where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(errors.First()) as TResult)!;
        }

        object validationResult = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(Result.Failure))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }

    #endregion
}