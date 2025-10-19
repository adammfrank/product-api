using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using Microsoft.AspNetCore.Routing;

namespace ProductApi.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        // GET /api/categories - All active categories
        routes.MapGet("/api/categories", async (Db db) =>
        {
            var categories = await db.Categories
                .Where(c => c.IsActive)
                .ToListAsync();
            return Results.Ok(categories);
        });

        // POST /api/categories - Create category
        routes.MapPost("/api/categories", async (CategoryCreateDto dto, Db db) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest(new { error = "Name is required" });
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                IsActive = true
            };

            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return Results.Created($"/api/categories/{category.Id}", category);
        });

        return routes;
    }
}

public record CategoryCreateDto(string Name, string? Description);
