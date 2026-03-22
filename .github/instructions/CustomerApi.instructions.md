# CustomersAPI Rules

- Use [ApiController] and [Route("api/[controller]")] attributes.
- Prefer **async Task<IActionResult>** endpoints.
- Follow REST conventions:
  - GET → return 200 or 404.
  - POST → return 201 with created object.
  - PUT → return 204 or 404.
  - DELETE → return 204 or 404.
- **Error responses must use ProblemDetails** (e.g., return BadRequest(new ProblemDetails { Title = "...", Detail = "...", Status = 400 })).
- Delegate business logic to Services, never inside Controllers.
- Validate request models using FluentValidation 
- Suggest unit test stubs for every service.
- Controllers should be thin, only orchestrating calls to services and returning appropriate HTTP responses.
- Services should contain all business logic, interact with dbcontext, and handle exceptions.
- DTOs should be used for API input/output, and mapped to/from entities in services.
- 
