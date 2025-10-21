using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Services
{

    class CategoryService : ICategoryService
    {
        private readonly Db _db;

        public CategoryService(Db db)
        {
            _db = db;
        }

        public async Task<List<CategoryWithProductsDto>> GetAllCategoriesAsync()
        {
            return await _db.Categories
                         .AsNoTracking()
                         .Where(c => c.IsActive)
                         .Select(c => new CategoryWithProductsDto(
                             c.Id,
                             c.Name,
                             c.Description,
                             c.IsActive,
                             c.Products
                                 .Where(p => p.IsActive)
                                 .Select(p => new ProductSummaryDto(
                                     p.Id,
                                     p.Name,
                                     p.Description,
                                     p.Price,
                                     p.StockQuantity,
                                     p.CreatedDate,
                                     p.IsActive,
                                     p.CategoryId
                                 ))
                                 .ToList()
                         )).ToListAsync();

        }

        public async Task<CategoryWithProductsDto> CreateCategoryAsync(CategoryCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Name is required", nameof(dto.Name));
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                IsActive = true
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            var created = new CategoryWithProductsDto(
                category.Id,
                category.Name,
                category.Description,
                category.IsActive,
                new List<ProductSummaryDto>()
            );

            return created;
        }


        public async Task<CategorySummaryDto?> GetCategorySummaryByIdAsync(int id)
        {
            return await _db.Categories
                .AsNoTracking()
                .Where(c => c.Id == id && c.IsActive)
                .Select(c => new CategorySummaryDto(
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Products.Count(),
                    c.Products.Count(p => p.IsActive),
                    c.Products.Average(p => p.Price),
                    c.Products.Sum(p => p.Price * p.StockQuantity),
                    new PriceRange(c.Products.Min(p => p.Price), c.Products.Max(p => p.Price)),
                    c.Products.Count(p => p.StockQuantity == 0)
                ))
                .FirstOrDefaultAsync();
        }
    }
}