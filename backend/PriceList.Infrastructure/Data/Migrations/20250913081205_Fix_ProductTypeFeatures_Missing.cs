using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceList.Infrastructure.Data.Migrations
{
    public partial class Fix_ProductTypeFeatures_Missing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[ProductTypeFeatures]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ProductTypeFeatures]
    (
        [ProductTypeId]     INT NOT NULL,
        [ProductFeatureId]  INT NOT NULL,

        -- (optional audit columns; keep only if your model expects them)
        [CreateDate]        varchar(10)  NULL,
        [CreateTime]        varchar(4)   NULL,
        [UpdateDate]        varchar(10)  NULL,
        [UpdateTime]        varchar(4)   NULL,
        [CreateDateAndTime] datetime2    NOT NULL CONSTRAINT DF_ProductTypeFeatures_CreateDateAndTime DEFAULT (SYSUTCDATETIME()),
        [UpdateDateAndTime] datetime2    NOT NULL CONSTRAINT DF_ProductTypeFeatures_UpdateDateAndTime DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT [PK_ProductTypeFeatures] PRIMARY KEY ([ProductTypeId],[ProductFeatureId]),
        CONSTRAINT [FK_ProductTypeFeatures_ProductTypes_ProductTypeId]
            FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductTypes]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProductTypeFeatures_ProductFeatures_ProductFeatureId]
            FOREIGN KEY ([ProductFeatureId]) REFERENCES [dbo].[ProductFeatures]([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_ProductTypeFeatures_ProductFeatureId]
        ON [dbo].[ProductTypeFeatures]([ProductFeatureId]);
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[ProductTypeFeatures]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[ProductTypeFeatures];
END
");
        }
    }
}
