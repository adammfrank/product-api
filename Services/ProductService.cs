using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Services
{

    class ProductService : IProductService
    {
        private readonly Db _db;

        public ProductService(Db db)
        {
            _db = db;
        }

        public async Task<List<ProductReadDto>> GetActiveProductsAsync()
        {
            var result = await _db.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategoryTruncatedDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                )).ToListAsync();

            return result;
        }

        public async Task<ProductReadDto?> GetActiveProductByIdAsync(int id)
        {
            var result = await _db.Products
                .AsNoTracking()
                .Where(p => p.Id == id && p.IsActive)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategoryTruncatedDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                )).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto dto)
        {
            var categoryExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException("Invalid CategoryId");
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CategoryId = dto.CategoryId
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            // Load as DTO for response
            var result = await _db.Products
                .Where(p => p.Id == product.Id)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategoryTruncatedDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .FirstAsync();

            return result;
        }

        public async Task<ProductReadDto?> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product is null)
            {
                return null;
            }

            var categoryExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException("Invalid CategoryId");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await _db.SaveChangesAsync();

            var result = await _db.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategoryTruncatedDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .FirstAsync();

            return result;
        }


        public async Task SoftDeleteProductAsync(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product is null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.IsActive = false;
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductReadDto>> SearchAsync(
            string? searchTerm,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStock,
            string? sortBy,
            string? sortOrder,
            int pageNumber,
            int pageSize)
        {
            var query = _db.Products.AsNoTracking().Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) || p.Description.ToLower().Contains(term));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (inStock.HasValue)
            {
                if (inStock.Value)
                    query = query.Where(p => p.StockQuantity > 0);
                else
                    query = query.Where(p => p.StockQuantity == 0);
            }

            var isDesc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            query = (sortBy ?? "name").ToLower() switch
            {
                "price" => isDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "createddate" => isDesc ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate),
                "stockquantity" => isDesc ? query.OrderByDescending(p => p.StockQuantity) : query.OrderBy(p => p.StockQuantity),
                _ => isDesc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            };

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var skip = (pageNumber - 1) * pageSize;

            var items = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategoryTruncatedDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .ToListAsync();

            return new PagedResult<ProductReadDto>(items, totalCount, pageNumber, pageSize, totalPages);
        }

    }
}