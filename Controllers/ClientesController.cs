using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.UnitofWork;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public ClientesController(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    // Ejercicio 1: Buscar clientes por nombre (parámetro "nombre")
    [HttpGet("ejercicio1")]
    public async Task<IActionResult> BuscarPorNombre([FromQuery] string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest("El nombre es requerido");

        var clientes = await _unitOfWork
            .Repository<Client>()
            .FindAsync(c => c.Name.ToLower().StartsWith(nombre.ToLower()));
        return Ok(clientes);
    }


    // Ejercicio 2: Obtener productos con precio mayor a un valor
    [HttpGet("ejercicio2")]
    public async Task<IActionResult> GetProductosConPrecioMayor([FromQuery] decimal valor)
    {
        var productos = await _unitOfWork.Repository<Product>()
            .FindAsync(p => p.Price > valor);

        return Ok(productos.ToList());
    }

    // Ejercicio 3: Mostrar detalles de productos en una orden específica
    [HttpGet("ejercicio3/{id}")]
    public async Task<IActionResult> GetDetalleProductosDeOrden(int id)
    {
        var ordenes = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.OrderId == id, o => o.Product);

        var detalle = ordenes
            .Select(o => new
            {
                Producto = o.Product.Name,
                Precio = o.Product.Price,
                Fecha = o.OrderDate
            })
            .ToList();

        return Ok(detalle);
    }

    // Ejercicio 4: Calcular la cantidad total de productos en una orden
    [HttpGet("ejercicio4/{orderId}")]
    public async Task<IActionResult> ObtenerCantidadTotalProductosPorOrden(int orderId)
    {
        var detalles = await _unitOfWork.Repository<OrderDetail>()
            .FindAsync(od => od.OrderId == orderId);

        var totalCantidad = detalles.Sum(od => od.Quantity);

        return Ok(new
        {
            OrderId = orderId,
            CantidadTotal = totalCantidad
        });
    }


    // Ejercicio 5: Obtener el producto más caro
    [HttpGet("ejercicio5")]
    public async Task<IActionResult> GetProductoMasCaro()
    {
        var productoMasCaro = await _unitOfWork.Repository<Product>()
            .GetAllAsync();

        var productoCaro = productoMasCaro
            .OrderByDescending(p => p.Price)
            .FirstOrDefault();

        if (productoCaro == null)
        {
            return NotFound("No se encontró un producto.");
        }

        return Ok(productoCaro);
    }

    // Ejercicio 6: Obtener pedidos realizados después de una fecha dada
    [HttpGet("ejercicio6")]
    public async Task<IActionResult> GetPedidosDespuesDeFecha([FromQuery] DateTime fechaEspecifica)
    {
        var pedidos = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.OrderDate > fechaEspecifica);

        if (pedidos == null || !pedidos.Any())
        {
            return NotFound("No se encontraron pedidos después de la fecha especificada.");
        }

        return Ok(pedidos);
    }

    // Ejercicio 7: Calcular el precio promedio de todos los productos
    [HttpGet("ejercicio7")]
    public async Task<IActionResult> GetPromedioPrecioProductos()
    {
        var productos = await _unitOfWork.Repository<Product>().GetAllAsync();

        var promedioPrecio = productos.Average(p => p.Price);

        return Ok(new { PromedioPrecio = promedioPrecio });
    }

    // Ejercicio 8: Obtener productos sin descripción
    [HttpGet("ejercicio8")]
    public async Task<IActionResult> GetProductosSinDescripcion()
    {
        var productos = await _unitOfWork.Repository<Product>().GetAllAsync();

        var productosSinDescripcion = productos
            .Where(p => string.IsNullOrEmpty(p.Description))
            .ToList();

        return Ok(productosSinDescripcion);
    }

    // Ejercicio 9: Obtener el cliente con más pedidos
    [HttpGet("ejercicio9")]
    public async Task<IActionResult> GetClienteConMasPedidos()
    {
        var pedidos = await _unitOfWork.Repository<Order>().GetAllAsync();

        var clienteConMasPedidos = pedidos
            .GroupBy(o => o.ClientId)
            .Select(g => new
            {
                ClientId = g.Key,
                CantidadPedidos = g.Count()
            })
            .OrderByDescending(g => g.CantidadPedidos)
            .FirstOrDefault();

        if (clienteConMasPedidos == null)
            return NotFound("No se encontraron pedidos.");

        var cliente = await _unitOfWork.Repository<Client>()
            .FindAsync(c => c.ClientId == clienteConMasPedidos.ClientId);

        var clienteInfo = cliente.FirstOrDefault();

        if (clienteInfo == null)
            return NotFound("Cliente no encontrado.");

        return Ok(new
        {
            Cliente = new
            {
                clienteInfo.ClientId,
                clienteInfo.Name,
                clienteInfo.Email
            },
            clienteConMasPedidos.CantidadPedidos
        });
    }

    // Ejercicio 10: Mostrar pedidos con detalles de productos (nombre y cantidad)
    [HttpGet("ejercicio10")]
    public async Task<IActionResult> GetPedidosConDetalles()
    {
        var detalles = await _unitOfWork
            .Repository<OrderDetail>()
            .FindAsync(x => true, od => od.Product);

        var resultado = detalles
            .Select(od => new
            {
                od.OrderId,
                Producto = od.Product.Name,
                od.Quantity
            })
            .ToList();

        return Ok(resultado);
    }
    
    [HttpGet("ejercicio11/{clienteId}")]
    public async Task<IActionResult> GetProductosVendidosPorCliente(int clienteId)
    {
        var detalles = await _unitOfWork.Repository<OrderDetail>().FindAsync(
            od => od.Order.ClientId == clienteId,
            od => od.Product,
            od => od.Order
        );

        var productos = detalles
            .Where(od => od.Product != null)
            .Select(od => od.Product.Name)
            .Distinct()
            .ToList();

        return Ok(productos);
    }
    
    
    [HttpGet("ejercicio12")]
    public async Task<IActionResult> GetProductoMasVendido()
    {
        var detalles = await _unitOfWork.Repository<OrderDetail>().FindAsync(
            od => od.Product != null,
            od => od.Product
        );

        var productoMasVendido = detalles
            .GroupBy(od => od.Product)
            .Select(g => new
            {
                Producto = g.Key.Name,
                TotalVendido = g.Sum(od => od.Quantity)
            })
            .OrderByDescending(p => p.TotalVendido)
            .FirstOrDefault();

        return Ok(productoMasVendido);
    }

}