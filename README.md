# TP4 — ProgMoviles

App **Blazor Hybrid MAUI** escrita en C# / .NET 9 que consume la API pública
[FakeStoreAPI](https://fakestoreapi.com/) para demostrar el uso de
`HttpClient` con los verbos **GET**, **POST**, **PUT** y **DELETE**.

> Trabajo Práctico N° 4 — Programación Móvil.

---

## Características

- 🛒 **CRUD completo de productos** contra `https://fakestoreapi.com/products`.
- 📑 **5 páginas Blazor** con rutas dedicadas: `Index`, `Details`, `Create`,
  `Edit`, `CategoryFilter`.
- 🧭 **Menú lateral personalizado** con carga dinámica de categorías.
- 🪟 **Layout propio** (app bar + sidebar con gradiente).
- 🏠 **Landing page** + página **Acerca de** estáticas.
- 🔌 `HttpClient` configurado vía `IHttpClientFactory` con cliente nombrado
  `"FakeStoreApi"`.
- 📚 Documentación en [`docs/API.md`](docs/API.md).

## Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
- Visual Studio 2022 (17.12+) con la carga de trabajo **.NET Multi-platform
  App UI development** o **Visual Studio Code** con la extensión *C# Dev Kit*.
- Windows 10 build 19041 o superior (para el target `net9.0-windows10.0.19041.0`).
- Conexión a internet (la app consume `https://fakestoreapi.com/`).

## Cómo ejecutar

```powershell
# Restaurar dependencias
dotnet restore

# Compilar y ejecutar en Windows
dotnet run -f net9.0-windows10.0.19041.0
```

La ventana de MAUI Blazor se abrirá mostrando la landing. Desde el menú
lateral se puede navegar a **Productos** para empezar a probar el CRUD.

> Para otras plataformas, usar el target correspondiente, p. ej.
> `dotnet build -f net9.0-android` (necesita el SDK de Android y un emulador).

## Estructura del proyecto

```
TP4-ProgMoviles/
├── Models/
│   └── Product.cs                 # DTO con JsonPropertyName (camelCase)
├── Services/
│   ├── IProductService.cs         # Contrato del servicio de productos
│   ├── ProductService.cs          # Implementación HTTP (GET/POST/PUT/DELETE)
│   ├── ICategoryService.cs        # Contrato de categorías
│   └── CategoryService.cs         # GET /products/categories
├── Components/
│   ├── Pages/
│   │   ├── Home.razor             # @page "/" — landing con hero + 3 cards
│   │   ├── About.razor            # @page "/about" — info del proyecto
│   │   └── Products/
│   │       ├── Index.razor        # @page "/products" — listado + Delete
│   │       ├── Details.razor      # @page "/products/details/{Id:int}"
│   │       ├── Create.razor       # @page "/products/create" — POST
│   │       ├── Edit.razor         # @page "/products/edit/{Id:int}" — PUT
│   │       └── CategoryFilter.razor # @page "/products/category/{Category}"
│   ├── Layout/
│   │   ├── MainLayout.razor       # App bar + container — reescrito
│   │   ├── MainLayout.razor.css   # Estilos del layout — reescrito
│   │   ├── NavMenu.razor          # Menú lateral con categorías — reescrito
│   │   └── NavMenu.razor.css      # Estilos del menú — reescrito
│   ├── _Imports.razor             # Using globales (incluye Models y Services)
│   └── Routes.razor               # Router (sin cambios)
├── MauiProgram.cs                 # Registro de HttpClient + servicios
├── Platforms/                     # Shells por plataforma (sin cambios)
├── Resources/                     # Icono, splash, fuentes, imágenes
├── wwwroot/                       # index.html, CSS, Bootstrap
├── docs/
│   └── API.md                     # Tabla página↔endpoint
└── README.md                      # Este archivo
```

## Páginas y endpoints

Ver [`docs/API.md`](docs/API.md) para la tabla completa.

| Página            | Ruta                       | Verbo         | Endpoint                  |
|-------------------|----------------------------|---------------|---------------------------|
| `Home`            | `/`                        | —             | —                         |
| `About`           | `/about`                   | —             | —                         |
| `Products/Index`  | `/products`                | GET, DELETE   | `/products`, `/products/{id}` |
| `Products/Details`| `/products/details/{Id}`   | GET           | `/products/{id}`          |
| `Products/Create` | `/products/create`         | POST          | `/products`               |
| `Products/Edit`   | `/products/edit/{Id}`      | GET, PUT      | `/products/{id}`          |
| `CategoryFilter`  | `/products/category/{c}`   | GET (filtro)  | `/products`               |

## Notas sobre FakeStoreAPI

- API pública sin autenticación: `https://fakestoreapi.com/`.
- Devuelve productos seed (20 ítems) con imágenes y categorías reales.
- Las operaciones `POST` / `PUT` / `DELETE` no persisten en el servidor: la
  API simula la operación y devuelve un objeto representativo, por lo que
  después de un `DELETE` el producto puede seguir apareciendo en una nueva
  llamada `GET`. Esto es esperado.

## Repositorio

<!-- TODO: pegar URL del repo -->

## Autor

- **Mateo Ezequiel Lemes** — Trabajo Práctico N° 4, Programación Móvil.
