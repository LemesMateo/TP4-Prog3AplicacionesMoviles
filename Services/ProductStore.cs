using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json;
using TP4_ProgMoviles.Models;

namespace TP4_ProgMoviles.Services;

/// <summary>
/// Local in-memory + on-disk store for <see cref="Product"/>.
/// <para>
/// FakeStoreAPI is a MOCK service: POST / PUT / DELETE return 2xx status codes
/// but the server does NOT persist any changes. To make the CRUD UI feel
/// real to the user, this store sits between the UI and the HTTP layer:
/// </para>
/// <list type="bullet">
///   <item>Reads (GET) come from the store, which is seeded once from the
///         real API and then mutated by every successful write.</item>
///   <item>Writes (POST/PUT/DELETE) hit the real API (so the network tab
///         shows the requests) AND update the store. The store is the
///         source of truth for subsequent reads.</item>
/// </list>
/// <para>
/// Persistence: a JSON snapshot is written to
/// <see cref="FileSystem.AppDataDirectory"/>/<c>products.local.json</c>
/// after every mutation, so changes survive an app restart. On first
/// launch (or if the file is missing/corrupt) the store seeds itself by
/// calling the FakeStoreAPI <c>GET /products</c> endpoint.
/// </para>
/// </summary>
public class ProductStore
{
    private const string StoreFileName = "products.local.json";
    private const string HttpClientName = "FakeStoreApi";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _storePath;
    private readonly SemaphoreSlim _gate = new(1, 1);

    // Sorted by Id; using a dictionary for O(1) lookup.
    private readonly SortedDictionary<int, Product> _products = new();
    private bool _seeded;

    public ProductStore(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _storePath = Path.Combine(FileSystem.AppDataDirectory, StoreFileName);
    }

    /// <summary>True if the in-memory copy is non-empty.</summary>
    public bool IsSeeded => _seeded && _products.Count > 0;

    /// <summary>Path of the on-disk JSON snapshot (for diagnostics).</summary>
    public string StoreFilePath => _storePath;

    /// <summary>
    /// Ensure the store is loaded — either from disk (if a snapshot exists)
    /// or by fetching the seed list from FakeStoreAPI.
    /// Thread-safe.
    /// </summary>
    public async Task EnsureSeededAsync()
    {
        if (_seeded) return;
        await _gate.WaitAsync();
        try
        {
            if (_seeded) return;

            if (File.Exists(_storePath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(_storePath);
                    var list = JsonSerializer.Deserialize<List<Product>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (list is not null && list.Count > 0)
                    {
                        _products.Clear();
                        foreach (var p in list)
                            _products[p.Id] = p;
                        _seeded = true;
                        return;
                    }
                }
                catch
                {
                    // Corrupt snapshot — fall through to seed from API.
                }
            }

            await SeedFromApiAsync();
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task SeedFromApiAsync()
    {
        var client = _httpClientFactory.CreateClient(HttpClientName);
        var list = await client.GetFromJsonAsync<List<Product>>("products")
                   ?? new List<Product>();
        _products.Clear();
        foreach (var p in list)
            _products[p.Id] = p;
        _seeded = true;
        await PersistAsync();
    }

    private async Task PersistAsync()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_storePath)!);
            var json = JsonSerializer.Serialize(_products.Values.ToList(),
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_storePath, json);
        }
        catch
        {
            // Persistence is best-effort. The in-memory store still works.
        }
    }

    // -------- Reads (source of truth = local store) --------

    public async Task<List<Product>> GetAllAsync()
    {
        await EnsureSeededAsync();
        await _gate.WaitAsync();
        try
        {
            return _products.Values.ToList();
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await EnsureSeededAsync();
        await _gate.WaitAsync();
        try
        {
            return _products.TryGetValue(id, out var p) ? p : null;
        }
        finally
        {
            _gate.Release();
        }
    }

    // -------- Writes (hit API, then mutate local store) --------

    /// <summary>Insert a new product. Hits POST /products and persists locally.</summary>
    public async Task<Product> CreateAsync(Product product)
    {
        await EnsureSeededAsync();
        var client = _httpClientFactory.CreateClient(HttpClientName);

        // Hit the real API (best-effort — FakeStoreAPI is a mock so it
        // returns a 201 with an echoed body, not the real persistence).
        int assignedId = product.Id;
        try
        {
            var response = await client.PostAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();
            var fromApi = await response.Content.ReadFromJsonAsync<Product>();
            if (fromApi is not null && fromApi.Id > 0)
                assignedId = fromApi.Id;
        }
        catch
        {
            // Continue with locally-assigned id even if the API fails.
        }

        // If the API didn't assign a usable id, pick the next free one.
        if (assignedId <= 0)
        {
            await _gate.WaitAsync();
            try
            {
                assignedId = _products.Keys.LastOrDefault() + 1;
                if (assignedId <= 0) assignedId = 1;
            }
            finally { _gate.Release(); }
        }

        var stored = new Product
        {
            Id = assignedId,
            Title = product.Title,
            Price = product.Price,
            Description = product.Description,
            Category = product.Category,
            Image = product.Image,
        };

        await _gate.WaitAsync();
        try
        {
            _products[stored.Id] = stored;
            await PersistAsync();
        }
        finally
        {
            _gate.Release();
        }
        return stored;
    }

    /// <summary>Update an existing product. Hits PUT /products/{id} and persists locally.</summary>
    public async Task<Product> UpdateAsync(int id, Product product)
    {
        await EnsureSeededAsync();
        var client = _httpClientFactory.CreateClient(HttpClientName);

        try
        {
            var response = await client.PutAsJsonAsync($"products/{id}", product);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            // API is a mock; continue to local persistence.
        }

        var stored = new Product
        {
            Id = id,
            Title = product.Title,
            Price = product.Price,
            Description = product.Description,
            Category = product.Category,
            Image = product.Image,
        };

        await _gate.WaitAsync();
        try
        {
            _products[id] = stored;
            await PersistAsync();
        }
        finally
        {
            _gate.Release();
        }
        return stored;
    }

    /// <summary>Delete a product. Hits DELETE /products/{id} and removes from the local store.</summary>
    public async Task<bool> DeleteAsync(int id)
    {
        await EnsureSeededAsync();
        var client = _httpClientFactory.CreateClient(HttpClientName);

        try
        {
            var response = await client.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            // API is a mock; continue to local delete.
        }

        await _gate.WaitAsync();
        try
        {
            var removed = _products.Remove(id);
            if (removed) await PersistAsync();
            return removed;
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <summary>
    /// Wipe the on-disk snapshot and re-seed from the API. Used by the
    /// "Reset local store" action so the user can recover the original
    /// list if they've made a mess of the local data.
    /// </summary>
    public async Task ResetAsync()
    {
        await _gate.WaitAsync();
        try
        {
            _products.Clear();
            _seeded = false;
            try { if (File.Exists(_storePath)) File.Delete(_storePath); } catch { }
        }
        finally
        {
            _gate.Release();
        }
        await EnsureSeededAsync();
    }
}
