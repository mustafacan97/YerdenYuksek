using eCommerce.Core.Entities.Media;
using eCommerce.Infrastructure.Persistence.Extensions;
using FluentMigrator.Builders.Create.Table;

namespace eCommerce.Infrastructure.Persistence.Builders.Medias;

public class PictureBuilder : IEntityBuilder
{
    #region Public Methods

    public void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Picture.Id)).AsGuid().NotNullable().PrimaryKey()
            .WithColumn(nameof(Picture.MimeType)).AsString(16).NotNullable()
            .WithColumn(nameof(Picture.SeoFilename)).AsString(128).NotNullable()
            .WithColumn(nameof(Picture.AltAttribute)).AsString(128).Nullable()
            .WithColumn(nameof(Picture.TitleAttribute)).AsString(128).Nullable()
            .WithColumn(nameof(Picture.IsNew)).AsBoolean().NotNullable()
            .WithColumn(nameof(Picture.VirtualPath)).AsString(512).Nullable()
            .WithColumn(nameof(Picture.CreatedOnUtc)).AsCustomDateTime().NotNullable()
            .WithColumn(nameof(Picture.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(nameof(Picture.Deleted)).AsBoolean().NotNullable();
    }

    #endregion
}
