using Microsoft.EntityFrameworkCore;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly LinqExampleContext _context;

    public OrderRepository(LinqExampleContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetProductosPorClienteAsync(int clientId)
    {
        // Buscar las órdenes del cliente con sus detalles e incluir los productos
        var ordenes = await _context.Orders
            .Where(o => o.ClientId == clientId)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product) // Incluir productos en los detalles de la orden
            .ToListAsync();

        // Obtener los productos de los detalles de la orden
        var productos = ordenes
            .SelectMany(o => o.OrderDetails)
            .Where(od => od.Product != null) // Asegurarse de que el producto no sea nulo
            .Select(od => od.Product.Name) // Obtener los nombres de los productos
            .Distinct() // Eliminar duplicados
            .ToList(); // Ejecutar la consulta y obtener la lista

        return productos;
    }
}