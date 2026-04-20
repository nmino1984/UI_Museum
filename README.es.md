# 🖼️ Museum UI — Resumen para evaluador

Interfaz web desarrollada como parte de la prueba técnica para **Xtra Travel**.
Consume la [Museum API](https://github.com/nmino1984/Museum) y expone todas las
operaciones de gestión de museos y artículos a través de vistas Razor.

---

## ✅ Qué cubre del challenge

### Funcionalidades obligatorias

- CRUD completo de **Museos** (listar, crear, editar, eliminar, ver detalle)
- CRUD completo de **Artículos** (listar, crear, editar, eliminar)
- Relación Museo → Artículos visible en el listado de museos
- Filtrado de museos por tema (`Art`, `Natural Sciences`, `History`)

### Funcionalidades opcionales implementadas

- **Soft delete** (`Remove`): marca el registro con `DeletedAt` sin borrarlo físicamente, tanto en museos como en artículos
- **Hard delete** (`Delete`): elimina físicamente el registro
- **BulkRegister**: asistente de dos pasos para crear múltiples artículos en un solo envío atómico
- **RelocateArticle**: formulario para mover un artículo a un museo diferente
- **MarkDamaged**: marca un artículo como dañado con un solo clic

---

## 🛠️ Stack tecnológico

| Elemento | Tecnología |
|---|---|
| Framework | ASP.NET Core 9.0 MVC |
| Lenguaje | C# 13 |
| Motor de vistas | Razor (`.cshtml`) |
| UI | Bootstrap 5 |
| Comunicación con API | `HttpClient` + Newtonsoft.Json 13 |
| Mapeo | AutoMapper 16 |
| Validación | FluentValidation 11 |

---

## 🚀 Instalación rápida

> **Requisito previo:** la [Museum API](https://github.com/nmino1984/Museum) debe estar en ejecución en `https://localhost:7297` antes de arrancar la UI.

```bash
# 1. Clonar el repositorio
git clone https://github.com/nmino1984/UI_Museum.git
cd UI_Museum

# 2. Restaurar paquetes NuGet
dotnet restore

# 3. (Opcional) Ajustar la URL de la API en appsettings.json
#    "ApiSettings": { "baseUrl": "https://localhost:TU_PUERTO/" }

# 4. Ejecutar la UI
dotnet run --project UI_Museum.csproj
```

La aplicación arranca en **`http://localhost:5265`**.

---

## 🔗 Cómo conecta con el backend

Todos los datos pasan por una capa de servicios (`MuseumService` / `ArticleService`)
que llaman a la API mediante `HttpClient`. La URL base se configura en un único sitio:

```json
// appsettings.json
{
  "ApiSettings": {
    "baseUrl": "https://localhost:7297/"
  }
}
```

Flujo de llamada:

```
Vista (Razor) → Controller → IMuseumService / IArticleService → HttpClient → Museum API
```

La API devuelve siempre un envelope `BaseResponse<T>` con `isSuccess`, `message`, `data`
y `errors`. Los servicios deserializan el campo `data` y devuelven el tipo concreto al controller.

---

## 📁 Estructura de carpetas

```
UI_Museum/
├── Controllers/       # MuseumController, ArticleController, HomeController
├── Interfaces/        # IMuseumService, IArticleService
├── Services/          # Implementaciones HTTP de los servicios
├── Models/            # ViewModels de request/response + BaseResponse<T>
├── Views/
│   ├── Museum/        # Index, Museum (form), Details, MuseumsByTheme
│   ├── Article/       # Index, Article (form), CreateSeveralArticles, RelocateArticle
│   └── Shared/        # _Layout.cshtml (Bootstrap)
├── Utiles/            # Enum Themes + helper SelectListItem
├── appsettings.json   # URL de la API
└── Program.cs         # Registro de DI y pipeline
```

---

## ✨ Características técnicas destacadas

**Capa de servicios desacoplada** — los controllers no conocen `HttpClient` ni JSON. Solo
hablan con una interfaz (`IMuseumService` / `IArticleService`), lo que permite cambiar la
fuente de datos sin tocar las vistas ni los controllers.

**BulkRegister en dos pasos** — el asistente primero pide el número de artículos; después
genera dinámicamente ese número de tarjetas en la misma página. El form usa nombres
indexados (`[0].Name`, `[1].Name`…) para que MVC enlace correctamente una `List<ArticleRequestViewModel>`.

**Soft delete con cascada** — al hacer Remove en un museo, la API elimina lógicamente
todos sus artículos activos. La UI confirma la acción con un diálogo antes de proceder.

**Sin lógica de negocio en la UI** — todas las validaciones y reglas de negocio viven
en la API. La UI solo muestra el resultado (`isSuccess`, `errors`).

---

🇬🇧 [Full English README](README.md)
