# UserAuthEntities Rules

- Only define **domain entities for UserAuth API**.
- Include data validation attributes:
  - [Required], [EmailAddress], [MaxLength], etc.
- Avoid business logic inside entities.
- Use plain POCOs; 
- Keep entities focused on data representation for UserAuth API.
-Entities should be in the `UserAuth.Entities` namespace and folder.
-Entity Configuration (if needed) should be in `UserAuth.Entities.Configurations` namespace and folder.