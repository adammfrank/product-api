using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public bool IsActive { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }

    public record CategoryCreateDto(
        [property: Required]
        [property: MaxLength(200)]
        string Name,
        string? Description);
    public record ProductSummaryDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int StockQuantity,
        DateTime CreatedDate,
        bool IsActive,
        int CategoryId
    );

    public record CategoryWithProductsDto(
        int Id,
        string Name,
        string Description,
        bool IsActive,
        List<ProductSummaryDto> Products
    );

    // Represents a price range with decimal boundaries
    public record PriceRange(decimal Min, decimal Max);

    // Summary DTO with optional analytics fields. The first four parameters remain
    // positional to preserve existing call sites elsewhere in the project.
    public record CategorySummaryDto(
        int CategoryId,
        string CategoryName,
        string CategoryDescription,
        int TotalProducts,
        int ActiveProducts,
        decimal AveragePrice,
        decimal TotalInventoryValue,
        PriceRange PriceRange,
        int OutOfStockCount
    );

}