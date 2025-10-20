using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        // GET /api/products - All active products with category info
        routes.MapGet("/api/products", async ([FromServices] ProductService productService) =>
        {
            var result = await productService.GetActiveProductsAsync();
            return Results.Ok(result);
        });

        // GET /api/products/{id} - Specific product (404 if not found or inactive)
        routes.MapGet("/api/products/{id:int}", async (int id, [FromServices] ProductService productService) =>
        {
            var result = await productService.GetActiveProductByIdAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        // POST /api/products - Create product
        routes.MapPost("/api/products", async (ProductCreateDto dto, [FromServices] ProductService productService) =>
        {
            var (isValid, errorResult) = ProductApi.Infrastructure.ValidationHelper.Validate(dto);
            if (!isValid) return Results.BadRequest(errorResult!);

            try
            {
                var result = await productService.CreateProductAsync(dto);
                return Results.Created($"/api/products/{result.Id}", result);
            }
            catch (ArgumentException e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        // PUT /api/products/{id} - Update product (404 if not found or inactive)
        routes.MapPut("/api/products/{id:int}", async (int id, ProductUpdateDto dto, [FromServices] ProductService productService) =>
        {
            var (isValid, errorResult) = ProductApi.Infrastructure.ValidationHelper.Validate(dto);
            if (!isValid) return Results.BadRequest(errorResult!);

            try
            {
                var product = await productService.UpdateProductAsync(id, dto);

                if (product is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(product);

            }
            catch (ArgumentException)
            {
                return Results.BadRequest(new { error = "Invalid CategoryId" });
            }

        });

        // DELETE /api/products/{id} - Soft delete (set IsActive = false)
        routes.MapDelete("/api/products/{id:int}", async (int id, [FromServices] ProductService productService) =>
        {
            try
            {
                await productService.SoftDeleteProductAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        return routes;
    }

    // ...existing code...
}

