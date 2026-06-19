using TP4_ProgMoviles.Models;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// Contract for product CRUD. Backed by <see cref="ProductStore"/>,
/// which wraps the real FakeStoreAPI HTTP calls AND a local snapshot
/// (FakeStoreAPI is a mock and does not persist writes).
/// </summary>
public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<List<string>> GetCategoriesAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
}
