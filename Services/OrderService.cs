using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.UnitofWork;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.DTOs;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly LinqExampleContext _context;

    public OrderService(IUnitOfWork unitOfWork, LinqExampleContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<object>> GetOrderProductsAsync(int orderId)
    {
        var detalles = await _unitOfWork.Repository<OrderDetail>()
            .GetAllAsync(
                predicate: od => od.OrderId == orderId,
                include: query => query.Include(od => od.Product)
            );

        return detalles.Select(od => new
        {
            Producto = od.Product.Name,
            Cantidad = od.Quantity
        }).ToList();
    }
    
    public async Task<int> GetTotalProductQuantityByOrderAsync(int orderId)
    {
        var detalles = await _unitOfWork.Repository<OrderDetail>()
            .GetAllAsync(predicate: od => od.OrderId == orderId);
        
        if (detalles == null || !detalles.Any())
        {
            throw new KeyNotFoundException($"No se encontraron detalles para la orden con ID {orderId}.");
        }

        int totalCantidad = detalles.Sum(od => od.Quantity);

        return totalCantidad;
    }
    
    public async Task<List<Order>> GetOrdersAfterDateAsync(DateTime date)
    {
        var orders = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.OrderDate > date);

        return orders.ToList();
    }
    
    public async Task<IEnumerable<object>> GetAllOrderDetailsAsync()
    {
        var details = await _unitOfWork
            .Repository<OrderDetail>()
            .GetAllAsync();

        var products = await _unitOfWork
            .Repository<Product>()
            .GetAllAsync();

        var result = details.Join(
            products,
            detail => detail.ProductId,
            product => product.ProductId,
            (detail, product) => new
            {
                OrderId = detail.OrderId,
                ProductName = product.Name,
                Quantity = detail.Quantity
            }).ToList();

        return result;
    }

    
    public async Task<IEnumerable<OrderDetailsDto>> GetOrdersWithDetailsAsync()
    {
        var result = await _context.Orders
            .Include(order => order.OrderDetails)
            .ThenInclude(detail => detail.Product)
            .AsNoTracking()
            .Select(order => new OrderDetailsDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Products = order.OrderDetails
                    .Select(od => new ProductDto
                    {
                        ProductName = od.Product.Name,
                        Quantity = od.Quantity,
                        Price = od.Product.Price
                    }).ToList()
            }).ToListAsync();

        return result;
    }
    
    public async Task<IEnumerable<object>> GetClientsWithTotalProductsAsync()
    {
        var result = await _context.Clients
            .AsNoTracking()
            .Select(client => new
            {
                ClientName = client.Name,
                TotalProducts = client.Orders
                    .Sum(order => order.OrderDetails
                    .Sum(detail => detail.Quantity))
            })
            .ToListAsync();

        return result;
    }
    
    public async Task<IEnumerable<SalesByClientDto>> GetTotalSalesByClientAsync()
    {
        var result = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .AsNoTracking()
            .GroupBy(order => order.ClientId)
            .Select(group => new SalesByClientDto
            {
                ClientName = _context.Clients
                    .FirstOrDefault(c => c.ClientId == group.Key).Name,

                TotalSales = group.Sum(order => order.OrderDetails
                    .Sum(detail => detail.Quantity * detail.Product.Price))
            })
            .OrderByDescending(dto => dto.TotalSales)
            .ToListAsync();

        return result;
    }
}
