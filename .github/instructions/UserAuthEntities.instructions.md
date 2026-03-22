# UserAuthEntities Rules

- Only define **domain entities for UserAuth API**.
- Avoid business logic inside entities.
- Use plain POCOs; 
- Use fluent API for entity configuration if needed, but keep it simple.
- Keep entities focused on data representation for UserAuth API.
-Entity Configuration (if needed) should be in `UserAuth.Entities.Configurations` namespace and folder.
- Keep entities focused on data representation for UserAuth API.
- Avoid business logic inside entities, only include domain-specific behavior.
-Entities should be in the `UserAuth.Entities` namespace and folder.
