using System.Net.Http;
using System.Net.Http.Json;
using TP4_ProgMoviles.Models;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// HTTP-backed implementation of <see cref="IProductService"/>.
/// Uses the named HttpClient <c>"FakeStoreApi"</c> registered in
/// <see cref="MauiProgram"/>. All transport errors are wrapped in
/// <see cref="InvalidOperationException"/> with a user-friendly Spanish
/// message so Razor pages can display <c>ex.Message</c> directly.
/// </summary>
public class ProductService : IProductService
{
    private const string HttpClientName = "FakeStoreApi";

    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient(HttpClientName);

    public async Task<List<Product>> GetAllAsync()
    {
        try
        {
            var client = CreateClient();
            var products = await client.GetFromJsonAsync<List<Product>>("products");
            return products ?? new List<Product>();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudo obtener la lista de productos: {ex.Message}", ex);
        }
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        try
        {
            var client = CreateClient();
            var product = await client.GetFromJsonAsync<Product>($"products/{id}");
            return product ?? throw new InvalidOperationException($"El producto {id} no existe.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudo obtener el producto {id}: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        try
        {
            var client = CreateClient();
            var categories = await client.GetFromJsonAsync<List<string>>("products/categories");
            return categories ?? new List<string>();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudieron obtener las categorías: {ex.Message}", ex);
        }
    }

    public async Task<Product> CreateAsync(Product product)
    {
        try
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<Product>();
            return created ?? throw new InvalidOperationException("La API no devolvió el producto creado.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudo crear el producto: {ex.Message}", ex);
        }
    }

    public async Task<Product> UpdateAsync(int id, Product product)
    {
        try
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"products/{id}", product);
            response.EnsureSuccessStatusCode();
            var updated = await response.Content.ReadFromJsonAsync<Product>();
            return updated ?? throw new InvalidOperationException("La API no devolvió el producto actualizado.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudo actualizar el producto {id}: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"products/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudo eliminar el producto {id}: {ex.Message}", ex);
        }
    }
}
