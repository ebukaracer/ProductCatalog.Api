### Description

A Clean, Scalable, and Maintainable **ASP.NET Web API** that supports **product catalog** and **order processing** while preventing overselling of stock. 

This project demonstrates:
- Clean separation of concerns
- DTO–Entity mapping & validation
- Partial updates (PATCH-like behaviour via PUT)
- Optimistic concurrency using version tokens
- Efficient database queries
- EF-Core based seeding & migrations
- Global exception handling middleware
- Best practices for startup, DI, controllers, and domain logic

### Tech Stacks
ASP.NET Core Version - `9.0.10`\
C# Version - `13`\
API Architecture  - `Controllers`\
API Testing with `Scalar`

| Layer        | Technology                       | Why                                   |
| ------------ | -------------------------------- | ------------------------------------- |
| API Layer    | ASP.NET Core Web API             | Fast, modern, minimal boilerplate     |
| Data Access  | Entity Framework Core            | Built-in migrations, code-first, LINQ |
| Database     | SQLite                           | Easy local development                |
| Architecture | Clean Architecture               | Maintainability, testability          |
| Mapping      | Lightweight mapping helpers      | Avoid business logic in controllers   |
| Validation   | Data annotations + manual checks | Clear, explicit validations           |

### Setup Instructions
TL;DR: If you prefer not to clone this repository and set it up manually, visit this [URL](https://productcatalogapi.tryasp.net) to directly test the API.

- Clone this repo to your desired directory
- Open the `.sln` file using your desired C#  code editor(rider, vscode, visual studio, etc)
- In your code editor's terminal, use `dotnet restore` command to restore packages' dependencies (in case any are missing)
- Use `dotnet run` command to run the project; the API will start at http://localhost:5216 and you will be automatically redirected to `scalar UI`(for testing)
- You can also verify that the API is running by navigating to: http://localhost:5216/health

In the `appsettings.json` file, migrations and initial data seeding are enabled by default. You can control them in that file.

### API Endpoints
#### Products:
```
GET /api/Products
POST /api/Products
GET /api/Products/{id}
PUT /api/Products/{id}
DELETE /api/Products/{id}
```
NB: Update(**PUT**) **must include** `version` (optimistic concurrency; 409 on mismatch)
#### Orders:
```
POST /api/Orders 
GET /api/Orders
GET /api/Orders/{orderId}
```
### Assumptions

- The API uses **GUID-based version tokens** for optimistic concurrency.
- Use of **Db transactions** for orders ensures stock deductions + order creation are atomic; avoids oversell in high concurrency.
- **PUT** is used for **partial update** so only the fields the client sets get applied.
- Application follows clean architecture but is intentionally lightweight (no over-engineering).
