using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Categories
            migrationBuilder.Sql(@"
                INSERT INTO ""Categories"" (""Id"", ""Name"", ""Description"", ""IsActive"") VALUES
                    (1, 'Electronics', 'Electronic gadgets and devices', TRUE),
                    (2, 'Books', 'Books across various genres', TRUE),
                    (3, 'Home', 'Home and kitchen essentials', TRUE)
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            // Seed Products
            // Note: CreatedDate is stored as timestamptz; use NOW() for current timestamp
            migrationBuilder.Sql(@"
                INSERT INTO ""Products"" (""Id"", ""Name"", ""Description"", ""Price"", ""StockQuantity"", ""CreatedDate"", ""IsActive"", ""CategoryId"") VALUES
                    (1, 'Wireless Mouse', 'Ergonomic wireless mouse', 29.99, 150, NOW(), TRUE, 1),
                    (2, 'USB-C Charger', 'Fast charging USB-C power adapter', 19.99, 200, NOW(), TRUE, 1),
                    (3, 'Sci-Fi Novel', 'A captivating science fiction story', 14.95, 80, NOW(), TRUE, 2),
                    (4, 'Cookbook', 'Recipes for quick meals', 24.50, 60, NOW(), TRUE, 2),
                    (5, 'Coffee Maker', '12-cup drip coffee maker', 49.00, 40, NOW(), TRUE, 3)
                ON CONFLICT (""Id"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seeded Products first due to FK to Categories
            migrationBuilder.Sql(@"
                DELETE FROM ""Products"" WHERE ""Id"" IN (1,2,3,4,5);
            ");

            migrationBuilder.Sql(@"
                DELETE FROM ""Categories"" WHERE ""Id"" IN (1,2,3);
            ");
        }
    }
}
