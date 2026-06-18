using TP4_ProgMoviles.Models;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// Contract for the FakeStoreAPI Products endpoint.
/// All methods are async and may throw <see cref="InvalidOperationException"/>
/// on transport or HTTP errors (the underlying <see cref="HttpRequestException"/>
/// is wrapped and re-thrown with a localized message).
/// </summary>
public interface IProductService
{
    /// <summary>GET /products — list every product.</summary>
    Task<List<Product>> GetAllAsync();

    /// <summary>GET /products/{id} — fetch a single product by id.</summary>
    Task<Product> GetByIdAsync(int id);

    /// <summary>GET /products/categories — list all category names.</summary>
    Task<List<string>> GetCategoriesAsync();

    /// <summary>POST /products — create a new product. Returns the API response (with assigned id).</summary>
    Task<Product> CreateAsync(Product product);

    /// <summary>PUT /products/{id} — update an existing product. Returns the API response.</summary>
    Task<Product> UpdateAsync(int id, Product product);

    /// <summary>DELETE /products/{id} — delete a product. Returns true on success.</summary>
    Task<bool> DeleteAsync(int id);
}
