# CustomerEntities Rules

- Only define **domain entities for Customer API**.
- Include data validation attributes:
  - [Key], [Required], [MaxLength], [ForeignKey].
- Avoid business logic inside entities, only include domain-specific behavior.
- Prefer simple POCOs (Plain Old CLR Objects).