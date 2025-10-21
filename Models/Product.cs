using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }

    public record ProductCreateDto(
        [property: Required]
        string Name,
        [property: Required]
        string Description,
        [property: Range(0, double.MaxValue)]
        decimal Price,
        [property: Range(0, int.MaxValue)]
        int StockQuantity,
        [property: Required]
        int CategoryId);

    public record ProductUpdateDto(
        [property: Required]
        string Name,
        [property: Required]
        string Description,
        [property: Range(0, double.MaxValue)]
        decimal Price,
        [property: Range(0, int.MaxValue)]
        int StockQuantity,
        [property: Required]
        int CategoryId);

    public record CategoryTruncatedDto(int Id, string Name, string Description, bool IsActive);
    public record ProductReadDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int StockQuantity,
        DateTime CreatedDate,
        bool IsActive,
        CategoryTruncatedDto Category
    );

}