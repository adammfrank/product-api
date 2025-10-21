using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Services;

namespace ProductApi.Endpoints;

public static class CategoryEndPoints
{

    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        // GET /api/categories - All active categories, including their products
        routes.MapGet("/api/categories", async ([FromServices] ICategoryService categoryService) =>
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return Results.Ok(categories);
        });

        // POST /api/categories - Create category
        routes.MapPost("/api/categories", async (CategoryCreateDto dto, Db db) =>
        {
            var (isValid, errorResult) = ProductApi.Infrastructure.ValidationHelper.Validate(dto);
            if (!isValid) return Results.BadRequest(errorResult!);

            try
            {
                var categoryService = new CategoryService(db);
                var created = await categoryService.CreateCategoryAsync(dto);
                return Results.Created($"/api/categories/{created.Id}", created);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // GET /api/categories/{id}/summary - Get summary data for a specific category 
        routes.MapGet("/api/categories/{id}/summary", async (int id, [FromServices] ICategoryService categoryService) =>
        {
            var category = await categoryService.GetCategorySummaryByIdAsync(id);
            return category is not null ? Results.Ok(category) : Results.NotFound();
        });

        return routes;

    }
}

