using System.Text.Json.Serialization;

namespace TP4_ProgMoviles.Models;

/// <summary>
/// DTO that mirrors the Product schema of https://fakestoreapi.com/products.
/// All JSON property names are explicitly mapped to camelCase via
/// <see cref="JsonPropertyNameAttribute"/> to keep the contract
/// independent of the C# naming policy.
/// </summary>
public class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;
}
