using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.DTOs;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;

public interface IClientService
{
    Task<IEnumerable<Client>> GetClientsByNamePrefixAsync(string startsWith);
    Task<(int clientId, int orderCount)> GetClientWithMostOrdersAsync();
    Task<IEnumerable<object>> GetProductsSoldToClientAsync(int clientId);
    Task<IEnumerable<object>> GetClientsByProductAsync(int productId);
    Task<IEnumerable<ClientOrderDto>> GetClientOrdersAsync();
}