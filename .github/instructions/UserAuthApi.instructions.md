# UserAuthAPI Rules

- Use Minimal API style with `app.MapGet`, `app.MapPost`, `app.MapGroup`.
- Use **route grouping** for clarity (e.g., `/auth/login`, `/auth/register`).
- Handle authentication via JWT or Identity-style tokens.
- Return proper HTTP codes:
  - 200 → success
  - 400 → bad request
  - 401 → unauthorized
- **Error responses must use ProblemDetails** (e.g., `Results.Problem(...)` in Minimal APIs).
- Delegate business logic to services.
- Validate request models using DataAnnotations.
- Suggest repository and service patterns in Infra.
- Suggest unit test stubs for endpoints.