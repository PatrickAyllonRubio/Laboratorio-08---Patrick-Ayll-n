using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.DTOs;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;

public interface IOrderService
{
    Task<IEnumerable<object>> GetOrderProductsAsync(int orderId);
    Task<int> GetTotalProductQuantityByOrderAsync(int orderId);
    Task<List<Order>> GetOrdersAfterDateAsync(DateTime date);
    Task<IEnumerable<Object>> GetAllOrderDetailsAsync();
    Task<IEnumerable<OrderDetailsDto>> GetOrdersWithDetailsAsync();
    Task<IEnumerable<object>> GetClientsWithTotalProductsAsync();
    Task<IEnumerable<SalesByClientDto>> GetTotalSalesByClientAsync();
}