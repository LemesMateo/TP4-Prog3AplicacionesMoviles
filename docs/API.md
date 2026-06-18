# Documentación de endpoints consumidos

Esta página documenta la relación entre cada **página Blazor** de la app y el
**endpoint de la API REST** que consume. La API consumida es
[FakeStoreAPI](https://fakestoreapi.com/), y en particular su recurso
`/products` (CRUD completo) y `/products/categories`.

Base URL: `https://fakestoreapi.com/`

## Páginas y endpoints

| Página                       | Ruta                                  | Método HTTP | Endpoint                                            | Descripción                                                                 |
|------------------------------|---------------------------------------|-------------|-----------------------------------------------------|-----------------------------------------------------------------------------|
| `Pages/Home.razor`           | `/`                                   | —           | —                                                   | Landing estática; no consume la API.                                       |
| `Pages/About.razor`          | `/about`                              | —           | —                                                   | Página estática con información del proyecto.                              |
| `Pages/Products/Index.razor` | `/products`                           | **GET**     | `/products`                                         | Lista todos los productos. Acción **Eliminar** envía `DELETE /products/{id}`. |
| `Pages/Products/Index.razor` | `/products` (botón Eliminar)          | **DELETE**  | `/products/{id}`                                    | Elimina el producto de la fila correspondiente.                             |
| `Pages/Products/Details.razor` | `/products/details/{Id:int}`        | **GET**     | `/products/{id}`                                    | Muestra el detalle de un producto.                                         |
| `Pages/Products/Create.razor`  | `/products/create`                  | **POST**    | `/products`                                         | Crea un producto nuevo a partir del formulario.                            |
| `Pages/Products/Edit.razor`    | `/products/edit/{Id:int}`           | **GET** + **PUT** | `GET /products/{id}` y `PUT /products/{id}`   | Carga el producto y luego envía los cambios con PUT.                       |
| `Pages/Products/CategoryFilter.razor` | `/products/category/{Category}` | **GET**     | `/products` (filtro client-side por `Category`)     | Lista los productos cuya categoría coincide con el parámetro de ruta.      |
| `Layout/NavMenu.razor`         | (carga inicial)                    | **GET**     | `/products/categories`                              | Llena dinámicamente la lista de categorías del menú lateral.               |

## Servicios

| Servicio                       | Interfaz                | Responsabilidad                                                                  |
|--------------------------------|-------------------------|----------------------------------------------------------------------------------|
| `Services/ProductService.cs`   | `IProductService`       | `GetAll`, `GetById`, `GetCategories`, `Create`, `Update`, `Delete` sobre productos. |
| `Services/CategoryService.cs`  | `ICategoryService`      | `GetAll` sobre `/products/categories` para alimentar el NavMenu.                 |

Ambas clases reciben `IHttpClientFactory` y obtienen el cliente HTTP con nombre
`"FakeStoreApi"` (registrado en `MauiProgram.cs` con `BaseAddress =
https://fakestoreapi.com/`).

## Ejemplo de cuerpo JSON (POST / PUT)

```json
{
  "title": "Camiseta TP4",
  "price": 19.99,
  "description": "Camiseta de algodón con el logo de la materia.",
  "category": "men's clothing",
  "image": "https://example.com/camiseta.png"
}
```

## Capturas

Pendiente: agregar capturas de pantalla en `docs/screenshots/`.
