using System.Net.Http;
using System.Net.Http.Json;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// HTTP-backed implementation of <see cref="ICategoryService"/>.
/// Uses the named HttpClient <c>"FakeStoreApi"</c> registered in
/// <see cref="MauiProgram"/>.
/// </summary>
public class CategoryService : ICategoryService
{
    private const string HttpClientName = "FakeStoreApi";

    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<string>> GetAllAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient(HttpClientName);
            var categories = await client.GetFromJsonAsync<List<string>>("products/categories");
            return categories ?? new List<string>();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"No se pudieron obtener las categorías: {ex.Message}", ex);
        }
    }
}
