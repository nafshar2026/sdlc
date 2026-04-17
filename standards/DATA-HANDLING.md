# Data Handling Standards

> Rules for secrets management, PII protection, PCI-DSS compliance, SOX controls, and access governance. These standards satisfy controls from SOC 2, ISO 27001, PCI-DSS, and SOX.

---

## Secrets Management

- MUST NOT store secrets in source code, configuration files committed to version control, or environment variables in plaintext at rest.
- MUST use a dedicated secrets management service (HashiCorp Vault, AWS Secrets Manager, Azure Key Vault, GCP Secret Manager, or equivalent).
- MUST rotate secrets on a defined schedule (at minimum: annually for low-risk, quarterly for high-risk, immediately upon suspected compromise).
- MUST revoke compromised credentials immediately — no "we'll rotate it next cycle."
- MUST NOT log secrets, tokens, API keys, or passwords — even in debug mode, even in development environments.
- MUST audit access to secrets — who accessed what, when.

## PII & Sensitive Data

- MUST classify all data by sensitivity level:
  - **Public** — No restrictions.
  - **Internal** — Not for external distribution. Standard access controls.
  - **Confidential** — Business-sensitive. Encryption required, access logged.
  - **Restricted** — Regulated data (PII, PCI, PHI). Maximum controls apply.
- MUST encrypt Confidential and Restricted data at rest and in transit.
- MUST mask or redact PII in logs, error messages, and non-production environments.
- MUST implement data retention policies — no indefinite storage of sensitive data. Define and enforce retention periods.
- MUST NOT store cardholder data (PAN) unless there is a documented business requirement; if stored, full PCI-DSS controls apply.
- MUST NOT copy production PII into development or test environments without anonymization/pseudonymization.

## PCI-DSS Specific Controls

- MUST NOT store CVV/CVC, PIN data, or full magnetic stripe data after transaction authorization — ever.
- MUST tokenize or truncate PANs for display (show only last 4 digits).
- MUST restrict access to cardholder data environments to personnel with a documented need-to-know.
- MUST maintain a complete audit trail for all access to cardholder data.
- MUST segment cardholder data environments from general-purpose networks.

## SOX Controls

- MUST maintain an immutable audit trail for all financial data changes: who changed what, when, from what value to what value.
- MUST enforce segregation of duties — the person who writes code MUST NOT be the same person who approves it to production.
- MUST preserve data integrity — no silent modification, deletion, or backdating of financial records.
- MUST have documented change management for all production changes affecting financial systems.
- MUST retain financial audit records per regulatory requirements (typically 7 years).

## Access Control

- MUST implement least-privilege principle — every service account, user, and role gets only the permissions required for its function.
- MUST review and re-certify access grants quarterly.
- MUST disable or remove accounts within 24 hours of personnel departure.
- MUST NOT share service accounts or credentials between humans or between services.
- MUST use unique, attributable identities for all access to production systems.
