# Loaded Skill Context

Manifest: ./skills/azure-functions-generation/LOAD-ORDER.md
Loaded count: 10

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

## BEGIN SKILL-04-configuration.md

# Configuration

- appsettings.json contains production defaults only.
- appsettings.Development.json contains local and development overrides only.
- Secrets remain placeholders in config and are sourced from Key Vault or environment at runtime.

## END SKILL-04-configuration.md

## BEGIN SKILL-05-validation.md

# Validation

- Keep request validation in FluentValidation validators.
- Auto-register validators via assembly scanning.
- Ensure validation executes in the request pipeline.

## END SKILL-05-validation.md

## BEGIN SKILL-06-retry-resiliency.md

# Retry and Resiliency

- Register a singleton Polly resilience pipeline in dependency injection.
- Inject the resilience pipeline into services; do not build per-call pipelines inline.
- If retry count is 0, skip retry strategy and execute once.
- Use Polly 8 directly when not using IHttpClientFactory policies.
- Log retry attempt number, max attempts, and delay in OnRetry.

## END SKILL-06-retry-resiliency.md

## BEGIN SKILL-07-testing.md

# Testing

- Unit tests should cover domain logic and application handlers.
- Integration tests should cover end-to-end function endpoint behavior.
- Use FluentAssertions for readable assertions.
- Test naming convention: Method_Scenario_ExpectedResult.

## END SKILL-07-testing.md

## BEGIN SKILL-08-coverage-ci.md

# Coverage and CI Standards

- Use coverlet.msbuild, not coverlet.collector, for SonarQube-compatible coverage output.
- Remove any coverlet.collector references from test projects.
- Do not use XPlat Code Coverage collector commands that emit binary .coverage output.

## END SKILL-08-coverage-ci.md

## BEGIN SKILL-09-security-standards.md

# Security Standards

- Every Regex constructor must include a match timeout.
- Use a static readonly timeout and pass it to each Regex instantiation.

## END SKILL-09-security-standards.md

## BEGIN SKILL-10-output-target.md

# Output Target

- Generate the .NET solution under a repo-relative path such as ./generated/sample-azure-functions.

## END SKILL-10-output-target.md

