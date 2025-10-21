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
                    INSERT INTO ""Categories"" (""Name"", ""Description"", ""IsActive"") VALUES
                        ('Electronics', 'Electronic gadgets and devices', TRUE),
                        ('Books', 'Books across various genres', TRUE),
                        ('Home', 'Home and kitchen essentials', TRUE),
                        ('Toys', 'Toys and games for all ages', TRUE),
                        ('Clothing', 'Apparel and accessories', TRUE)
                    ON CONFLICT (""Id"") DO NOTHING;
                ");

            // Seed 20 Products, distributed across categories
            migrationBuilder.Sql(@"
                    INSERT INTO ""Products"" (""Name"", ""Description"", ""Price"", ""StockQuantity"", ""CreatedDate"", ""IsActive"", ""CategoryId"") VALUES
                        ('Wireless Mouse', 'Ergonomic wireless mouse', 29.99, 150, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Electronics')),
                        ('USB-C Charger', 'Fast charging USB-C power adapter', 19.99, 200, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Electronics')),
                        ('Bluetooth Headphones', 'Noise-cancelling over-ear headphones', 89.99, 75, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Electronics')),
                        ('Smartphone Stand', 'Adjustable phone stand', 12.50, 0, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Electronics')),
                        ('LED Desk Lamp', 'Dimmable LED lamp with USB port', 34.99, 60, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Electronics')),

                        ('Sci-Fi Novel', 'A captivating science fiction story', 14.95, 80, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Books')),
                        ('Cookbook', 'Recipes for quick meals', 24.50, 60, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Books')),
                        ('Mystery Thriller', 'A page-turning mystery', 16.99, 90, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Books')),
                        ('Children''s Picture Book', 'Illustrated book for kids', 9.99, 0, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Books')),
                        ('History Textbook', 'Comprehensive world history', 39.95, 30, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Books')),

                        ('Coffee Maker', '12-cup drip coffee maker', 49.00, 40, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Home')),
                        ('Blender', 'High-speed kitchen blender', 59.99, 35, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Home')),
                        ('Vacuum Cleaner', 'Bagless upright vacuum', 129.00, 20, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Home')),
                        ('Nonstick Frying Pan', '10-inch nonstick pan', 22.99, 70, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Home')),
                        ('Bath Towel Set', 'Set of 6 cotton towels', 34.99, 50, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Home')),

                        ('Building Blocks', '100-piece block set', 29.99, 80, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Toys')),
                        ('Board Game', 'Strategy board game for families', 44.95, 45, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Toys')),
                        ('Action Figure', 'Superhero action figure', 14.99, 100, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Toys')),
                        ('Dollhouse', 'Wooden dollhouse with furniture', 89.00, 15, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Toys')),
                        ('Puzzle Set', '500-piece jigsaw puzzle', 17.50, 60, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Toys')),

                        ('Men''s T-Shirt', 'Cotton crew neck t-shirt', 12.99, 150, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Clothing')),
                        ('Women''s Jeans', 'Slim fit stretch jeans', 39.99, 90, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Clothing')),
                        ('Baseball Cap', 'Adjustable sports cap', 15.00, 70, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Clothing')),
                        ('Winter Scarf', 'Wool blend scarf', 22.50, 40, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Clothing')),
                        ('Sneakers', 'Lightweight running shoes', 59.99, 60, NOW(), TRUE, (SELECT ""Id"" FROM ""Categories"" WHERE ""Name""='Clothing'))
                    ON CONFLICT (""Id"") DO NOTHING;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                TRUNCATE TABLE "Products", "Categories" RESTART IDENTITY CASCADE;
                """);
        }
    }
}
