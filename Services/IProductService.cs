using ProductApi.Models;

    interface IProductService
    {
        Task<ProductReadDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductReadDto?> GetActiveProductByIdAsync(int id);
        Task<List<ProductReadDto>> GetActiveProductsAsync();
        Task<PagedResult<ProductReadDto>> SearchAsync(
            string? searchTerm,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            bool? inStock,
            string? sortBy,
            string? sortOrder,
            int pageNumber,
            int pageSize
        );
        Task SoftDeleteProductAsync(int id);
        Task<ProductReadDto?> UpdateProductAsync(int id, ProductUpdateDto dto);
    }
