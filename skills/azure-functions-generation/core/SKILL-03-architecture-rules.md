# Architecture Rules

- Use Clean Architecture principles.
- Domain layer has zero external dependencies.
- Application layer defines interfaces; Infrastructure implements them.
- Keep trigger layer thin: function entry points only, with business logic in application handlers/services.
- Use package versions that are compatible and not deprecated or archived.
- Prefer centralized exception handling; avoid broad try/catch flow-control in business classes.
- Define properties and URLs in appsettings files and environment configuration, not hard-coded values.
- Use feature folders where practical.
- Keep Query, Command, and Handler in separate files within a feature folder.
- Use Azure Key Vault with User Assigned Managed Identity for secrets.
- Use Directory.Build.props and Directory.Build.targets at solution root for centralized package version management via PackageReference Update entries.
