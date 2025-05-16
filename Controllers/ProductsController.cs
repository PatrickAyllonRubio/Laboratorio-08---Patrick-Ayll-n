using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("filter-by-price")]
    public async Task<IActionResult> GetProductsByMinPrice([FromQuery] decimal minPrice)
    {
        if (minPrice <= 0)
            return BadRequest("El precio mínimo debe ser mayor a 0.");

        var products = await _productService.GetProductsByMinPriceAsync(minPrice);
        return Ok(products);
    }
    
    [HttpGet("most-expensive")]
    public async Task<IActionResult> GetMostExpensiveProduct()
    {
        var product = await _productService.GetMostExpensiveProductAsync();

        if (product == null)
            return NotFound("No hay productos disponibles.");

        return Ok(product);
    }
    
    [HttpGet("averageprice")]
    public async Task<IActionResult> GetAveragePrice()
    {
        var averagePrice = await _productService.GetAveragePriceAsync();

        if (averagePrice == 0)
            return NotFound("No hay productos para calcular el promedio.");

        return Ok(new { AveragePrice = averagePrice });
    }
    [HttpGet("nodescription")]
    public async Task<IActionResult> GetProductsWithoutDescription()
    {
        var products = await _productService.GetProductsWithoutDescriptionAsync();

        if (products == null || !products.Any())
            return NotFound("No se encontraron productos sin descripción.");

        return Ok(products);
    }
}