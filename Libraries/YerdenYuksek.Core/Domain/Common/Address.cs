﻿using YerdenYuksek.Core.Primitives;

namespace YerdenYuksek.Core.Domain.Common;

public class Address : BaseEntity
{
    #region Constructure and Destructure

    public Address()
    {
    }

    #endregion

    #region Public Properties

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public int? CountryId { get; set; }

    public int? StateProvinceId { get; set; }

    public string County { get; set; }

    public string City { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string ZipPostalCode { get; set; }

    public string PhoneNumber { get; set; }

    public string CustomAttributes { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    #endregion
}
