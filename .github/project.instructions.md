# Solution-Level Rules

## General Rules
- This guidance applies **only within this solution**.
- Maintain **separation of concerns**:
  - API layer → Services → Entities.
  - No business logic in Controllers or Minimal APIs.
- Use **async/await** for all database or service calls.
- Follow **clean code principles**: small methods, readable names, clear responsibilities.
- Logging: Use ILogger<T> in services.
- Exception handling: wrap DB or external errors, return proper HTTP response codes in APIs.
- DTOs must be separate from Entities; map explicitly or with AutoMapper. Prefer explicit mapping for clarity.
- Always validate models and inputs before processing.
- Use dependency injection for services and dbcontext, never new up instances in controllers or services.