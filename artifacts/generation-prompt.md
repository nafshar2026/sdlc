# Azure Functions Code Generation Prompt

Use the following requirements and loaded skill context to generate a complete .NET 10 Azure Functions solution.

## Output Location
./generated/sample-azure-functions

## Expected Solution Structure
```text
./generated/sample-azure-functions\
  src\
    Functions\
    Application\
    Domain\
    Infrastructure\
  tests\
    UnitTests\
    IntegrationTests\
  Directory.Build.props
  Directory.Build.targets
  README.md
```

## Requirements

Generate a production-ready .NET 10 Azure Functions solution using the isolated worker model.

Architecture and structure:
- Use Clean Architecture with separate Domain, Application, Infrastructure, and Functions trigger projects where appropriate.
- Use feature folders for application behavior.
- Keep Azure Functions trigger classes thin and move business logic into application handlers and services.
- Application layer defines interfaces and Infrastructure implements them.
- Domain layer must have zero external dependencies.

Functional requirements:
- Implement HTTP-triggered Azure Functions for customer document retrieval operations.
- Support a metadata endpoint that returns available document entries for a customer account.
- Support a file download endpoint that returns the requested document stream.
- Validate all route and query inputs.
- Reject invalid file names or path traversal attempts.

Technical requirements:
- Use FluentValidation for request validation.
- Use Polly for retry handling around transient external operations.
- Use Azure Application Insights for telemetry.
- Use ILogger for structured logging.
- Use Azure Key Vault with User Assigned Managed Identity for secrets.
- Keep configuration in appsettings.json and appsettings.Development.json, with secrets resolved from environment or Key Vault.
- Use OpenAPI support for documenting the HTTP-triggered functions.

Testing and quality:
- Add unit tests for validators and application handlers.
- Add integration tests for HTTP-triggered function endpoints.
- Use xUnit and FluentAssertions.
- Produce SonarQube-compatible coverage output using coverlet.msbuild.
- Ensure regex usage includes explicit match timeouts where applicable.

Output requirements:
- Generate the solution under ./generated/sample-azure-functions.
- Include README documentation describing the solution structure, configuration, local run steps, and test execution.

## Loaded Skill Context

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

## Generation Instructions

- Generate full source code, tests, configuration, and solution files.
- Write the generated solution to the output location shown above.
- Follow the requirements section first and use the loaded skill context as implementation guidance.
- Keep the trigger layer thin and place business logic in the appropriate application and infrastructure layers.
- Include README documentation for setup, local execution, and test execution.
