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

    public record CategoryCreateDto(string Name, string? Description);
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

}