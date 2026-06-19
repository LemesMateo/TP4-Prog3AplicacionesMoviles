using TP4_ProgMoviles.Models;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// Implementation of <see cref="IProductService"/> that delegates to the
/// local <see cref="ProductStore"/>. The store handles the real API call
/// (so the network tab shows the request) AND the local persistence.
/// <para>
/// Note: <see cref="GetCategoriesAsync"/> is implemented here too for
/// interface compatibility — the UI consumes it via
/// <c>ICategoryService</c> instead.
/// </para>
/// </summary>
public class ProductService : IProductService
{
    private readonly ProductStore _store;

    public ProductService(ProductStore store)
    {
        _store = store;
    }

    public Task<List<Product>> GetAllAsync() => _store.GetAllAsync();

    public async Task<Product> GetByIdAsync(int id)
    {
        var p = await _store.GetByIdAsync(id);
        return p ?? throw new InvalidOperationException($"El producto {id} no existe.");
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        // Derive categories from the local store so the nav menu reflects
        // any new product a user has created (rather than only the four
        // seed categories from the API).
        var products = await _store.GetAllAsync();
        return products
            .Select(p => p.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c)
            .ToList();
    }

    public Task<Product> CreateAsync(Product product) => _store.CreateAsync(product);

    public Task<Product> UpdateAsync(int id, Product product) => _store.UpdateAsync(id, product);

    public Task<bool> DeleteAsync(int id) => _store.DeleteAsync(id);
}
