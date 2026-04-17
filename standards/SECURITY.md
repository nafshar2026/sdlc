# Security Standards — OWASP Top 10 2021 Alignment

> Language-agnostic security rules mapped to the OWASP Top 10 (2021). Every rule marked MUST is a hard gate.

---

## A01: Broken Access Control

- Every endpoint MUST enforce authorization checks server-side.
- MUST NOT rely on client-side access control (hidden UI elements, client-side route guards alone).
- MUST default-deny on all resources — explicitly grant access, never implicitly allow.
- Role and permission checks MUST be enforced at the service layer, not just the UI layer.
- MUST validate that the authenticated user has access to the specific resource being requested (object-level authorization).

## A02: Cryptographic Failures

- MUST NOT hardcode secrets, keys, or credentials in source code.
- MUST use TLS 1.2 or higher for all data in transit.
- MUST use AES-256 or equivalent for data at rest encryption.
- MUST NOT implement custom cryptographic algorithms — use platform-provided or well-vetted libraries only.
- MUST NOT use MD5 or SHA1 for any security-related purpose (password hashing, integrity verification, digital signatures).
- MUST use purpose-built password hashing algorithms (bcrypt, scrypt, Argon2) for credential storage.

## A03: Injection

- All external input MUST be validated and sanitized before use.
- MUST use parameterized queries or prepared statements for all database operations.
- MUST NEVER construct SQL, LDAP, OS, or other interpreter queries via string concatenation with user input.
- MUST apply context-aware output encoding (HTML encoding for HTML contexts, URL encoding for URLs, etc.).
- MUST validate input against an allowlist of expected formats when possible, rather than a denylist of known-bad patterns.

## A04: Insecure Design

- Threat modeling MUST be performed for new features and significant changes.
- MUST maintain separation of concerns between trust boundaries (e.g., public-facing vs. internal services).
- MUST implement fail-secure defaults — when a security check fails or errors, deny access.
- MUST implement rate limiting on all public-facing endpoints.
- MUST NOT trust client-provided data for security-critical decisions (e.g., pricing, authorization level).

## A05: Security Misconfiguration

- MUST NOT deploy with default credentials or default configuration.
- MUST disable unnecessary features, ports, services, and debug endpoints in production.
- MUST include security headers on all HTTP responses (Content-Security-Policy, X-Content-Type-Options, X-Frame-Options, Strict-Transport-Security, etc.).
- Error messages MUST NOT leak stack traces, database details, file paths, or internal architecture information.
- MUST remove sample applications, unused frameworks, and development tools from production deployments.

## A06: Vulnerable and Outdated Components

- MUST NOT deploy with known-vulnerable dependencies (CVE scan required in CI/CD pipeline).
- MUST pin all dependency versions — no floating version ranges in production.
- MUST have an automated process for detecting and updating vulnerable dependencies (Dependabot, Renovate, Snyk, etc.).
- MUST perform license compliance checks on all dependencies.
- MUST maintain a software bill of materials (SBOM) for production deployments.

## A07: Identification and Authentication Failures

- MUST require multi-factor authentication (MFA) for privileged operations and administrative access.
- Session tokens MUST have a defined expiration and MUST be invalidated on logout.
- MUST enforce password policies server-side (minimum length, complexity, breach database checks).
- MUST implement account lockout or progressive delays after repeated failed authentication attempts.
- MUST NOT expose whether a username or email exists during login/registration flows (use generic error messages).

## A08: Software and Data Integrity Failures

- MUST verify the integrity of all build artifacts, dependencies, and deployment packages.
- MUST require code review by at least one other person before merge to protected branches.
- MUST use signed commits for production branches.
- MUST NOT include unsigned or unverified third-party code in production deployments.
- CI/CD pipelines MUST be protected — changes require the same review process as application code.

## A09: Security Logging and Monitoring Failures

- MUST log all authentication events (login, logout, failed attempts).
- MUST log all access control failures (unauthorized access attempts).
- MUST log all input validation failures (potential injection attempts).
- MUST NEVER log secrets, tokens, passwords, API keys, or PII in plaintext.
- MUST store logs in tamper-evident storage with access controls.
- Logs MUST include sufficient detail for forensic analysis: timestamp, source IP, user identity, action, outcome.

## A10: Server-Side Request Forgery (SSRF)

- MUST validate and allowlist all URLs used in server-side requests.
- MUST NOT allow user-controlled input to specify server-side request destinations without validation against an allowlist.
- MUST block requests to internal/private network ranges (10.x.x.x, 172.16-31.x.x, 192.168.x.x, 127.x.x.x, ::1, metadata endpoints like 169.254.169.254).
- MUST NOT follow redirects from external URLs to internal network addresses.
