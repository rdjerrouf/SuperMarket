using SuperMarket.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarket.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(string query);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
    }
}