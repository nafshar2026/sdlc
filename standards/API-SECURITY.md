# API Security Standards — OWASP API Top 10 2023 Alignment

> Language-agnostic API security rules mapped to the OWASP API Security Top 10 (2023). Every rule marked MUST is a hard gate.

---

## API1: Broken Object Level Authorization

- Every API endpoint MUST validate that the authenticated caller has access to the specific object being requested.
- MUST NOT rely on obscure or unpredictable object IDs as the sole means of access control.
- Authorization checks MUST occur on every request, not just at session creation.

## API2: Broken Authentication

- MUST use token-based authentication with defined expiration times.
- MUST NOT include API keys or tokens in URLs (use headers instead).
- MUST rotate API credentials and tokens on a defined schedule.
- MUST rate-limit authentication endpoints aggressively to prevent brute-force attacks.
- MUST invalidate all active sessions when credentials are reset.

## API3: Broken Object Property Level Authorization

- API responses MUST NOT expose object properties the caller is not authorized to see.
- MUST filter response fields based on the caller's permissions — never return the full object and rely on the client to hide fields.
- MUST validate that the caller is authorized to modify each property in write operations (mass assignment prevention).

## API4: Unrestricted Resource Consumption

- All API endpoints MUST have rate limits appropriate to their function.
- List/query endpoints MUST require pagination — no unbounded result sets.
- MUST enforce maximum payload size limits on all request bodies.
- MUST timeout long-running operations with a defined maximum execution time.
- MUST limit concurrent requests per client/token to prevent resource exhaustion.

## API5: Broken Function Level Authorization

- Administrative endpoints MUST be separated from user endpoints and independently authorized.
- MUST NOT allow privilege escalation through parameter manipulation (e.g., changing `role=admin` in request body).
- MUST verify function-level permissions on every request — not just at the controller/route level.

## API6: Unrestricted Access to Sensitive Business Flows

- MUST identify sensitive business flows (checkout, money transfer, account registration, data export) and apply additional protections.
- MUST implement anti-automation controls (CAPTCHA, device fingerprinting, rate limits) for sensitive flows where appropriate.
- MUST monitor for unusual patterns in sensitive flows (volume spikes, velocity anomalies).

## API7: Server-Side Request Forgery (SSRF)

- See SECURITY.md A10 — same rules apply to API endpoints.
- MUST validate and allowlist all outbound URLs initiated by API logic.
- MUST block server-side requests to internal/private network ranges and cloud metadata endpoints.

## API8: Security Misconfiguration

- CORS MUST be configured restrictively — no wildcard (`*`) origins in production.
- MUST disable HTTP methods not explicitly needed by the API (e.g., TRACE, OPTIONS without CORS need).
- MUST remove or disable debug/diagnostic endpoints in production.
- MUST enforce a consistent API versioning strategy across all services.
- MUST NOT expose detailed error information (stack traces, SQL errors) in API responses.

## API9: Improper Inventory Management

- All APIs MUST be documented in a central registry (OpenAPI/Swagger or equivalent).
- MUST explicitly deprecate old API versions with a published sunset timeline.
- MUST NOT have shadow or undocumented APIs in production — every endpoint must be inventoried.
- MUST track API consumers and notify them of deprecations and breaking changes.

## API10: Unsafe Consumption of APIs

- MUST validate and sanitize all data received from third-party APIs — treat external data as untrusted input.
- MUST NOT blindly trust or forward data from external API responses to internal systems.
- MUST implement circuit breakers for all third-party API integrations.
- MUST define timeouts for all outbound API calls — no indefinite waits.
- MUST have fallback behavior defined for when third-party APIs are unavailable.
