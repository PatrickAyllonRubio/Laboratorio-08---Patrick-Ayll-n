namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories;

public interface IOrderRepository
{
    Task<List<string>> GetProductosPorClienteAsync(int clientId);
}
