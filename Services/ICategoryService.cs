using ProductApi.Models;

interface ICategoryService
    {
        Task<CategoryWithProductsDto> CreateCategoryAsync(CategoryCreateDto dto);
        Task<List<CategoryWithProductsDto>> GetAllCategoriesAsync();
    }
