using System.Net.Http;
using System.Net.Http.Json;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// Returns the list of distinct product categories from the local
/// <see cref="ProductStore"/>. We derive them client-side so the NavMenu
/// updates automatically when a user creates a product in a new
/// category.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ProductStore _store;

    public CategoryService(ProductStore store)
    {
        _store = store;
    }

    public async Task<List<string>> GetAllAsync()
    {
        var products = await _store.GetAllAsync();
        return products
            .Select(p => p.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c)
            .ToList();
    }
}
