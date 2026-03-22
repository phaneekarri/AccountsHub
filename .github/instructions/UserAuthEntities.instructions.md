# UserAuthEntities Rules

- Only define **domain entities for UserAuth API**.
- Include data validation attributes:
  - [Required], [EmailAddress], [MaxLength], etc.
- Avoid business logic inside entities.
- Use plain POCOs; no EF Core navigation unless required.