# Loaded Skill Context

Manifest: ./skills/azure-functions-generation/LOAD-ORDER.md
Loaded count: 4

## BEGIN SKILL-01-scope.md

# Scope

- Assume there is no MuleSoft code, RAML, MUnit, flows, or connector config available.
- Generate a production-ready .NET 10 Azure Functions solution.

## END SKILL-01-scope.md

## BEGIN SKILL-02-tech-stack.md

# Tech Stack

- .NET 10 Azure Functions (Isolated Worker)
- FluentValidation for request validation
- OpenAPI 3.x support for function endpoints
- xUnit + FluentAssertions for testing
- ILogger and Azure Application Insights for logging and telemetry
- Polly for resiliency and retries

## END SKILL-02-tech-stack.md

## BEGIN SKILL-03-architecture-rules.md

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

## END SKILL-03-architecture-rules.md

## BEGIN SKILL-10-output-target.md

# Output Target

- Generate the .NET solution under a repo-relative path such as ./generated/sample-azure-functions.

## END SKILL-10-output-target.md

