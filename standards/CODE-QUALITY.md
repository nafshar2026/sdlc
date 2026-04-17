# Code Quality Standards

> Language-agnostic code quality rules for all production code. These apply equally to human-written and AI-generated code.

---

## Error Handling

- MUST use structured error handling mechanisms (try/catch, Result/Either types, error returns) — no silent failures.
- MUST NOT expose internal details in error responses visible to end users (stack traces, database connection strings, file paths, internal service names).
- MUST have a consistent error response format across all services in the organization.
- SHOULD use documented, actionable error codes/categories rather than freeform error messages.
- MUST NOT use catch-all exception handlers that swallow errors silently — log or propagate every error.

## Code Structure

- MUST follow single responsibility principle — each function/method does one thing, each class/module has one reason to change.
- MUST separate business logic from infrastructure concerns (database access, HTTP handling, message queues, file I/O).
- MUST NOT have circular dependencies between modules or packages.
- SHOULD limit function/method length to approximately 50 lines and class/module length to approximately 300 lines.
- MUST use consistent naming conventions within a project (casing, prefixes, terminology).
- MUST NOT duplicate business logic — extract shared logic into a single authoritative location.

## Dependency Management

- MUST pin all dependency versions explicitly — no floating ranges (^, ~, *, latest) in production lock files.
- MUST scan all dependencies for known vulnerabilities (CVE database) as part of the CI/CD pipeline.
- MUST NOT include unused dependencies in the project.
- SHOULD have an automated dependency update process (Dependabot, Renovate, or equivalent).
- MUST review dependency changes in pull requests — new dependencies require explicit approval.

## Configuration

- MUST externalize all environment-specific configuration — no environment-specific values baked into code or built artifacts.
- MUST NOT hardcode URLs, credentials, connection strings, or environment-specific values in source code.
- MUST use environment variables or a secure configuration service for secrets and sensitive configuration.
- MUST maintain separate configuration per environment (development, staging, production).
- MUST validate configuration at application startup — fail fast if required configuration is missing.
