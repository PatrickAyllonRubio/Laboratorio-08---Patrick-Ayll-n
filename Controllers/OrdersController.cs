using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.DTOs;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{orderId}/productos")]
    public async Task<IActionResult> GetOrderProducts(int orderId)
    {
        var productos = await _orderService.GetOrderProductsAsync(orderId);

        if (!productos.Any())
            return NotFound($"No se encontraron productos para la orden {orderId}");

        return Ok(productos);
    }
    
    [HttpGet("{orderId}/totalQuantity")]
    public async Task<IActionResult> GetTotalQuantity(int orderId)
    {
        try
        {
            var totalCantidad = await _orderService.GetTotalProductQuantityByOrderAsync(orderId);
            return Ok(new { OrderId = orderId, TotalQuantity = totalCantidad });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("afterdate")]
    public async Task<IActionResult> GetOrdersAfterDate([FromQuery] DateTime date)
    {
        if (date == default)
            return BadRequest("La fecha es obligatoria y debe ser válida.");

        var orders = await _orderService.GetOrdersAfterDateAsync(date);
        if (orders == null || !orders.Any())
            return NotFound("No se encontraron pedidos después de la fecha especificada.");

        return Ok(orders);
    }
    
    [HttpGet("details")]
    public async Task<IActionResult> GetAllOrderDetails()
    {
        var details = await _orderService.GetAllOrderDetailsAsync();
        return Ok(details);
    }
    
    [HttpGet("details-lab09")]
    public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> GetOrdersWithDetails()
    {
        var result = await _orderService.GetOrdersWithDetailsAsync();
        return Ok(result);
    }
    
    [HttpGet("clientes-con-productos-lab09")]
    public async Task<IActionResult> GetClientsWithTotalProducts()
    {
        var result = await _orderService.GetClientsWithTotalProductsAsync();
        return Ok(result);
    }
    
    [HttpGet("ventas-por-cliente")]
    public async Task<IActionResult> GetSalesByClient()
    {
        var result = await _orderService.GetTotalSalesByClientAsync();
        return Ok(result);
    }

}