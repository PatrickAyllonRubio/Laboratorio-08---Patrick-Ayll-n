using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.UnitofWork;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.DTOs;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly LinqExampleContext _context;

    public ClientService(IUnitOfWork unitOfWork, LinqExampleContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetClientsByNamePrefixAsync(string startsWith)
    {
        return await _unitOfWork
            .Repository<Client>()
            .FindAsync(c => c.Name.StartsWith(startsWith));
    }

    public async Task<(int clientId, int orderCount)> GetClientWithMostOrdersAsync()
    {
        var orders = await _unitOfWork.Repository<Order>().GetAllAsync();

        var grouped = orders
            .GroupBy(o => o.ClientId)
            .Select(g => new
            {
                ClientId = g.Key,
                OrderCount = g.Count()
            })
            .OrderByDescending(g => g.OrderCount)
            .FirstOrDefault();

        if (grouped == null)
            return (0, 0);

        return (grouped.ClientId, grouped.OrderCount);
    }
    
    public async Task<IEnumerable<object>> GetProductsSoldToClientAsync(int clientId)
    {
        // Obtener todas las órdenes del cliente
        var orders = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.ClientId == clientId);

        if (!orders.Any())
            return Enumerable.Empty<object>();

        // Obtener los IDs de los productos comprados
        var productIds = orders
            .Select(o => o.ProductId)
            .Distinct()
            .ToList();

        // Obtener todos los productos
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();

        // Filtrar solo los comprados y proyectar
        var result = products
            .Where(p => productIds.Contains(p.ProductId))
            .Select(p => new
            {
                ProductName = p.Name
            })
            .ToList();

        return result;
    }

    
    public async Task<IEnumerable<object>> GetClientsByProductAsync(int productId)
    {
        // Obtener órdenes que contienen el producto especificado
        var orders = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.ProductId == productId);

        if (!orders.Any())
            return Enumerable.Empty<object>();

        // Obtener los IDs únicos de los clientes que hicieron estas órdenes
        var clientIds = orders
            .Select(o => o.ClientId)
            .Distinct()
            .ToList();

        // Obtener todos los clientes
        var clients = await _unitOfWork.Repository<Client>().GetAllAsync();

        // Filtrar y proyectar con objeto anónimo
        var result = clients
            .Where(c => clientIds.Contains(c.ClientId))
            .Select(c => new
            {
                ClientName = c.Name
            })
            .ToList();

        return result;
    }

    
    public async Task<IEnumerable<ClientOrderDto>> GetClientOrdersAsync()
    {
        var clients = await _context.Clients
            .AsNoTracking()
            .Select(client => new ClientOrderDto
            {
                ClientName = client.Name,
                Orders = client.Orders
                    .Select(order => new OrderDto
                    {
                        OrderId = order.OrderId,
                        OrderDate = order.OrderDate
                    })
                    .ToList()
            })
            .ToListAsync();

        return clients;
    }
    
}