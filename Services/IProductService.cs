using ProductApi.Models;

    interface IProductService
    {
        Task<ProductReadDto> CreateProductAsync(ProductCreateDto dto);
        Task<ProductReadDto?> GetActiveProductByIdAsync(int id);
        Task<List<ProductReadDto>> GetActiveProductsAsync();
        Task SoftDeleteProductAsync(int id);
        Task<ProductReadDto?> UpdateProductAsync(int id, ProductUpdateDto dto);
    }
