# CustomerEntities Rules

- Only define **domain entities for Customer API**.
- Include data validation attributes:
  - [Key], [Required], [MaxLength], [ForeignKey].
- Avoid business logic inside entities, only include domain-specific behavior.
- Prefer simple POCOs (Plain Old CLR Objects).
- Keep entities focused on data representation for Customer API.
- Entities should be in the `Customer.Entities` namespace and folder.
- Entity Configuration (if needed) should be in `Customer.Entities.Configurations` namespace and folder.