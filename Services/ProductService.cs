using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.UnitofWork;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Product>> GetProductsByMinPriceAsync(decimal minPrice)
    {
        return await _unitOfWork
            .Repository<Product>()
            .FindAsync(p => p.Price > minPrice);
    }
    
    public async Task<Product?> GetMostExpensiveProductAsync()
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();
        
        var mostExpensiveProduct = products
            .OrderByDescending(p => p.Price)
            .FirstOrDefault();

        return mostExpensiveProduct;
    }
    
    public async Task<decimal> GetAveragePriceAsync()
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();

        if (products == null || !products.Any())
            return 0m;

        return products.Average(p => p.Price);
    }
    
    public async Task<IEnumerable<Product>> GetProductsWithoutDescriptionAsync()
    {
        return await _unitOfWork.Repository<Product>()
            .FindAsync(p => string.IsNullOrEmpty(p.Description));
    }
    
}