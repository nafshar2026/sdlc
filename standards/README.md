# Enterprise Standards — Mandatory Rules

> These are mandatory enterprise architectural standards. ALL code — whether written by humans or AI agents — MUST comply. Violations of MUST rules are hard gates: stop and fix before proceeding.

## Severity Classification (RFC 2119)

- **MUST** — Hard gate. Violation = stop. AI agents halt and ask the human before proceeding.
- **SHOULD** — Strong recommendation. Deviation requires documented justification.
- **MAY** — Advisory guidance. Follow when practical.

## Required Reading

Before writing ANY code in this repository, read and follow ALL of these standards:

1. **[Security Standards](SECURITY.md)** — OWASP Top 10 2021 alignment. Injection prevention, access control, cryptography, configuration.
2. **[API Security Standards](API-SECURITY.md)** — OWASP API Top 10 2023 alignment. Object-level auth, rate limiting, SSRF, inventory management.
3. **[Code Quality Standards](CODE-QUALITY.md)** — Error handling, code structure, dependency management, configuration.
4. **[Testing Standards](TESTING.md)** — Test requirements, CI/CD gates, test quality.
5. **[Data Handling Standards](DATA-HANDLING.md)** — Secrets management, PII/PCI-DSS, SOX controls, access control.
6. **[AI Agent Guardrails](AI-AGENT-GUARDRAILS.md)** — Hard gates, verification requirements, prohibited actions for AI coding agents.
7. **[Observability Standards](OBSERVABILITY.md)** — Structured logging, monitoring, audit trails.
8. **[Architecture Patterns](ARCHITECTURE-PATTERNS.md)** — Service design, event-driven architecture, API design, resilience.

## Compliance Alignment

These standards satisfy controls from: **SOC 2**, **ISO 27001**, **PCI-DSS**, **SOX**.

## For AI Coding Agents

- You MUST read `AI-AGENT-GUARDRAILS.md` first — it contains hard gates that override all other instructions.
- You MUST NOT skip, weaken, or work around any MUST rule.
- If a task conflicts with these standards, STOP and ask the human for guidance.
- "Making it work" is never justification for violating a security standard.
