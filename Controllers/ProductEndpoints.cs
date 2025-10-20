using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        // GET /api/products - All active products with category info
        routes.MapGet("/api/products", async (Db db) =>
        {
            var products = await db.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategorySummaryDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .ToListAsync();
            return Results.Ok(products);
        });

        // GET /api/products/{id} - Specific product (404 if not found or inactive)
        routes.MapGet("/api/products/{id:int}", async (int id, Db db) =>
        {
            var product = await db.Products
                .Where(p => p.Id == id && p.IsActive)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategorySummaryDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .FirstOrDefaultAsync();
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        // POST /api/products - Create product
        routes.MapPost("/api/products", async (ProductCreateDto dto, Db db) =>
        {
            var categoryExists = await db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return Results.BadRequest(new { error = "Invalid CategoryId" });
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

            db.Products.Add(product);
            await db.SaveChangesAsync();

            // Load as DTO for response
            var created = await db.Products
                .Where(p => p.Id == product.Id)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategorySummaryDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .FirstAsync();
            return Results.Created($"/api/products/{product.Id}", created);
        });

        // PUT /api/products/{id} - Update product (404 if not found or inactive)
        routes.MapPut("/api/products/{id:int}", async (int id, ProductUpdateDto dto, Db db) =>
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product is null)
            {
                return Results.NotFound();
            }

            var categoryExists = await db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return Results.BadRequest(new { error = "Invalid CategoryId" });
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await db.SaveChangesAsync();

            var updated = await db.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductReadDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CreatedDate,
                    p.IsActive,
                    new CategorySummaryDto(p.Category!.Id, p.Category!.Name, p.Category!.Description, p.Category!.IsActive)
                ))
                .FirstAsync();
            return Results.Ok(updated);
        });

        // DELETE /api/products/{id} - Soft delete (set IsActive = false)
        routes.MapDelete("/api/products/{id:int}", async (int id, Db db) =>
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product is null)
            {
                return Results.NotFound();
            }

            product.IsActive = false;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return routes;
    }
}

