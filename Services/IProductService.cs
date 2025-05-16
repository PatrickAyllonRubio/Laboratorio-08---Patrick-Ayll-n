using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsByMinPriceAsync(decimal minPrice);
    Task<Product?> GetMostExpensiveProductAsync();
    Task<decimal> GetAveragePriceAsync();
    Task<IEnumerable<Product>> GetProductsWithoutDescriptionAsync();

}