using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetClientsByNamePrefix([FromQuery] string startsWith)
    {
        if (string.IsNullOrEmpty(startsWith))
            return BadRequest("El parámetro 'startsWith' es obligatorio.");

        var clients = await _clientService.GetClientsByNamePrefixAsync(startsWith);
        return Ok(clients);
    }

    [HttpGet("most-orders")]
    public async Task<IActionResult> GetClientWithMostOrders()
    {
        var result = await _clientService.GetClientWithMostOrdersAsync();

        if (result.clientId == 0)
            return NotFound("No se encontraron órdenes.");

        return Ok(new
        {
            ClientId = result.clientId,
            TotalOrders = result.orderCount
        });
    }
    
    [HttpGet("{clientId}/products")]
    public async Task<IActionResult> GetProductsSoldToClient(int clientId)
    {
        var products = await _clientService.GetProductsSoldToClientAsync(clientId);
    
        if (!products.Any())
            return NotFound($"No se encontraron productos para el cliente con ID {clientId}");

        return Ok(products);
    }
    
    [HttpGet("{productId}/buyers")]
    public async Task<IActionResult> GetClientsByProduct(int productId)
    {
        var result = await _clientService.GetClientsByProductAsync(productId);
    
        if (!result.Any())
            return NotFound($"No se encontraron clientes que hayan comprado el producto con ID {productId}.");

        return Ok(result);
    }
    
    [HttpGet("with-orders-lab09")]
    public async Task<IActionResult> GetClientsWithOrders()
    {
        var result = await _clientService.GetClientOrdersAsync();
        return Ok(result);
    }

}