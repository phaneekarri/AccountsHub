# CustomerEntities Rules

- Only define **domain entities for Customer API**.
- Use plain POCOs; 
- Use fluent API for entity configuration if needed, but keep it simple.
- Keep entities focused on data representation for Customer API.
- Avoid business logic inside entities, only include domain-specific behavior.
- Entity Configuration (if needed) should be in `Customer.Entities.Configurations` namespace and folder.
- Entities should be in the `CustomerEntities.Models` namespace and folder.