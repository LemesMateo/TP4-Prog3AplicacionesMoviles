namespace TP4_ProgMoviles.Services;

/// <summary>
/// Contract for the FakeStoreAPI categories endpoint.
/// Kept separate from <see cref="IProductService"/> so the NavMenu
/// can render the category list independently of any product list.
/// </summary>
public interface ICategoryService
{
    /// <summary>GET /products/categories — list all category names.</summary>
    Task<List<string>> GetAllAsync();
}
