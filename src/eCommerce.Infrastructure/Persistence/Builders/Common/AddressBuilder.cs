using eCommerce.Core.Entities.Common;
using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Primitives;
using eCommerce.Infrastructure.Extensions;
using FluentMigrator.Builders.Create.Table;
using System.Data;

namespace eCommerce.Infrastructure.Persistence.Builders.Common;

public class AddressBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(BaseEntity.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(Address.FirstName)).AsString(128).NotNullable()
            .WithColumn(nameof(Address.LastName)).AsString(128).NotNullable()
            .WithColumn(nameof(Address.Email)).AsString(128).NotNullable()
            .WithColumn(nameof(Address.PhoneNumber)).AsString(12).Nullable()
            .WithColumn(nameof(Address.CustomerId)).AsGuid().ForeignKey<Customer>(onDelete: Rule.None).NotNullable()
            .WithColumn(nameof(Address.CityId)).AsGuid().ForeignKey<City>(onDelete: Rule.None).NotNullable()
            .WithColumn(nameof(Address.CountryId)).AsGuid().ForeignKey<Country>(onDelete: Rule.None).NotNullable()
            .WithColumn(nameof(Address.Address1)).AsString().NotNullable()
            .WithColumn(nameof(Address.Address2)).AsString().Nullable()
            .WithColumn(nameof(Address.ZipCode)).AsString(32).Nullable()
            .WithColumn(nameof(Address.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Address.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Address.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
