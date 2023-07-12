using eCommerce.Core.Primitives;
using YerdenYuksek.Core.Domain.Customers;
using eCommerce.Core.Primitives;

namespace YerdenYuksek.Application.Models.Customers;

public record RegisterResponseModel : BaseModel
{
    #region Constructure and Destructure

    public RegisterResponseModel()
    {
        Errors = new List<Error>();
    }

    #endregion

    #region Public Properties    

    public Customer Customer { get; private set; }

    #endregion

    #region Properties

    private List<Error> Errors { get; set; }

    #endregion

    #region Public Methods

    public bool IsSuccess => !Errors.Any();

    public void SetCustomer(Customer customer) => Customer = customer;

    public void AddError(Error error) => Errors.Add(error);

    public void AddErors(IList<Error> errors) => Errors.AddRange(errors);

    #endregion
}