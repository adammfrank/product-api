using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Models
{
    [Index(nameof(IsActive))]
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

    public record PriceRange(decimal Min, decimal Max);

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