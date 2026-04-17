# Testing Standards

> Requirements for test coverage, CI/CD quality gates, and test design. These apply to all production codebases.

---

## Test Requirements

- MUST have unit tests for all business logic, targeting 80%+ code coverage on critical paths (authentication, authorization, payment processing, data validation).
- MUST have integration tests for all API endpoints covering happy path, error cases, and edge cases.
- MUST have security-focused tests: authentication bypass attempts, authorization boundary violations, injection payloads, boundary/overflow conditions.
- MUST NOT have tests that depend on live external services — use mocks, stubs, or fakes for external dependencies.
- SHOULD have contract tests for all service-to-service boundaries to catch breaking changes early.
- SHOULD have smoke tests that verify critical paths after each deployment.

## CI/CD Quality Gates

These are hard gates — the build MUST fail if any are not met:

- All tests MUST pass before merge to protected branches.
- Static analysis and linting MUST pass with zero errors.
- Dependency vulnerability scan MUST pass with no critical or high severity CVEs.
- Code coverage MUST NOT decrease on the target branch (ratchet — only goes up).
- SAST (Static Application Security Testing) scan MUST pass.
- SHOULD include DAST (Dynamic Application Security Testing) in staging/pre-production pipelines.

## Test Quality

- Tests MUST be deterministic — flaky tests are not permitted on the main branch. A flaky test MUST be fixed or quarantined immediately.
- Tests MUST NOT contain hardcoded secrets, real credentials, or real PII — use fixtures or generated test data.
- Tests MUST be independent — no ordering dependencies between tests. Each test sets up and tears down its own state.
- SHOULD follow the Arrange-Act-Assert (or Given-When-Then) pattern for readability and consistency.
- Test names MUST describe the behavior being tested, not the implementation detail (e.g., `test_expired_token_returns_401` not `test_check_token`).
- MUST NOT disable or skip tests to make a build pass — fix the test or fix the code.
