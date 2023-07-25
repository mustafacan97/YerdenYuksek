using eCommerce.Core.Entities.Customers;
using eCommerce.Core.Entities.Directory;
using eCommerce.Core.Entities.Localization;
using eCommerce.Core.Entities.Media;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator.Builders.Create.Table;
using System.Data;

namespace eCommerce.Infrastructure.Persistence.Builders.Customers;

public class CustomerBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Customer.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(Customer.Email)).AsString(128).NotNullable()
            .WithColumn(nameof(Customer.FirstName)).AsString(128).Nullable()
            .WithColumn(nameof(Customer.LastName)).AsString(128).Nullable()
            .WithColumn(nameof(Customer.BirthDate)).AsString(64).Nullable()
            .WithColumn(nameof(Customer.PhoneNumber)).AsString(12).Nullable()
            .WithColumn(nameof(Customer.EmailValidated)).AsBoolean().NotNullable()
            .WithColumn(nameof(Customer.PhoneNumberValidated)).AsBoolean().NotNullable()
            .WithColumn(nameof(Customer.LanguageId)).AsGuid().ForeignKey<Language>(onDelete: Rule.SetNull).Nullable()
            .WithColumn(nameof(Customer.CurrencyId)).AsGuid().ForeignKey<Currency>(onDelete: Rule.SetNull).Nullable()
            .WithColumn(nameof(Customer.PictureId)).AsGuid().ForeignKey<Picture>(onDelete: Rule.SetNull).Nullable()
            .WithColumn(nameof(Customer.DefaultAddressId)).AsGuid().Nullable()            
            .WithColumn(nameof(Customer.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Customer.LastLoginDateUtc)).AsCustomDateTime().Nullable()
            .WithColumn(nameof(Customer.LastActivityDateUtc)).AsCustomDateTime().Nullable();
    }

    #endregion
}
