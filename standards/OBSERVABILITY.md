# Observability Standards

> Rules for structured logging, monitoring, alerting, and audit trails. Aligned to SOC 2 and SOX audit requirements.

---

## Structured Logging

- MUST use structured logging format (JSON or key-value pairs) — no unstructured `print`, `console.log`, or `System.out.println` statements in production code.
- MUST include a correlation ID (request ID, trace ID) in every log entry to enable distributed tracing across services.
- MUST log the following events:
  - All authentication events (login success, login failure, logout, token refresh)
  - All authorization failures (access denied, permission check failures)
  - All input validation failures (rejected payloads, malformed requests)
  - All system errors (unhandled exceptions, service unavailability, timeout errors)
- MUST NOT log:
  - Secrets, API keys, tokens, or passwords — in any form, at any log level
  - Full credit card numbers, SSNs, or other PII in plaintext
  - Request/response bodies that may contain sensitive data without redaction
- MUST use consistent, well-defined log levels across all services:
  - **ERROR** — Something is broken and requires human action.
  - **WARN** — Something is degraded or unusual but the system is functioning.
  - **INFO** — Significant business events (order placed, user registered, payment processed).
  - **DEBUG** — Development diagnostics only. MUST NOT be enabled in production.

## Monitoring & Alerting

- MUST expose health check endpoints for all services (liveness and readiness probes).
- MUST monitor and track:
  - Error rates (total and per-endpoint)
  - Latency percentiles (p50, p95, p99)
  - Request throughput
  - Resource utilization (CPU, memory, disk, connections)
- MUST alert on:
  - Error rate exceeding baseline thresholds
  - Latency degradation beyond defined SLOs
  - Authentication failure patterns (potential brute-force attempts)
  - Dependency/downstream service failures
- SHOULD maintain dashboards for all production services showing key metrics.
- SHOULD define and publish SLOs (Service Level Objectives) for all customer-facing services.

## Audit Trail (SOC 2 / SOX Alignment)

- MUST maintain immutable audit logs for:
  - Data access (who read what sensitive data, when)
  - Data modification (who changed what, previous value, new value)
  - Administrative actions (user creation, permission grants, configuration changes)
  - System configuration changes (deployment, scaling, feature flag changes)
- Every audit log entry MUST include:
  - Timestamp (UTC, ISO 8601 format)
  - Actor identity (user ID, service account, API key identifier — NOT the key itself)
  - Action performed
  - Resource affected (type and identifier)
  - Outcome (success or failure, with reason for failure)
- MUST retain audit logs per regulatory requirements (typically 1 year for SOC 2, 7 years for SOX/PCI-DSS).
- MUST NOT allow application code to modify or delete audit log entries.
- Audit log storage MUST have independent access controls from the application.