using eCommerce.Core.Entities.Directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace eCommerce.Infrastructure.Persistence.Builders.Directory;

public class CurrencyBuilder : IEntityTypeConfiguration<Currency>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currency");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(64);

        builder.Property(x => x.CurrencyCode)
            .HasMaxLength(4);

        builder.Property(x => x.DisplayLocale)
            .HasMaxLength(8);

        builder.Property(x => x.CustomFormatting)
            .HasMaxLength(32);

        builder.Property(x => x.Rate)
            .HasPrecision(18, 4);

        builder.Property(x => x.CreatedOnUtc)
            .HasPrecision(6);

        builder.HasMany(x => x.Customers)
            .WithOne(x => x.Currency)
            .HasForeignKey(x => x.CurrencyId)
            .IsRequired(false);

        builder.HasMany(x => x.Languages)
            .WithOne(x => x.DefaultCurrency)
            .HasForeignKey(x => x.DefaultCurrencyId)
            .IsRequired();

        builder.HasData(SeedDefaultCurrencyData());
    }

    #endregion

    #region Methods

    private static IList<Currency> SeedDefaultCurrencyData()
    {
        IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var currencies = new List<Currency>
        {
            new Currency
            {
                Id = _configuration.GetValue<Guid>("DefaultValues:CurrencyId"),
                Name = "US Dollar",
                CurrencyCode = "USD",
                Rate = 1,
                DisplayLocale = "en-US",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                CustomFormatting = null,
                Deleted = false,
                RoundingType = RoundingType.Rounding001,
                DisplayOrder = 0,
            }
        };

        return currencies;
    }

    #endregion
}
