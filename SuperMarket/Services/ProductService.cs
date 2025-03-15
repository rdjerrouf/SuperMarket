using SuperMarket.Core.Interfaces;
using SuperMarket.DataAccess.Data;
using SuperMarket.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMarket.Services
{
    /// <summary>
    /// Provides functionalities to manage product data.
    /// Implements IProductService interface
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context; // DbContext instance

        /// <summary>
        /// Initializes a new instance of the ProductService class
        /// </summary>
        /// <param name="context">DbContext for database operations</param>
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }


        /// <summary>
        /// Retrieves a product by its unique identifier
        /// </summary>
        /// <param name="productId">Unique ID of the product</param>
        /// <returns>Product object</returns>
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        /// <summary>
        /// Search products based on the given query
        /// </summary>
        /// <param name="query">String to search for within product names or descriptions</param>
        /// <returns>List of products that match the search query</returns>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
        {
            // perform search within product name and description using a contains operation
            return await _context.Products
                                 .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                                 .ToListAsync();
        }

        /// <summary>
        /// Add new product to the database.
        /// </summary>
        /// <param name="product">Product object</param>
        /// <returns>Boolean flag indicating if product was added successfully</returns>
        public async Task<bool> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update an existing product in the database.
        /// </summary>
        /// <param name="product">Product object with updated properties</param>
        /// <returns>Boolean flag indicating if product was updated successfully</returns>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="productId">Unique ID of the product to be deleted</param>
        /// <returns>Boolean flag indicating if the product was deleted successfully</returns>
        public async Task<bool> DeleteProductAsync(int productId)
        {
            var productToDelete = await _context.Products.FindAsync(productId);
            if (productToDelete == null) return false;
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}