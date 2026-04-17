# Architecture Pattern Standards

> Standards for service design, event-driven systems, API contracts, resilience, and data architecture. These apply when designing new services or refactoring existing systems.

---

## Service Design

- MUST define clear service boundaries aligned to business domains (bounded contexts from Domain-Driven Design).
- MUST NOT create shared databases between services — each service owns and exclusively manages its data store.
- MUST communicate between services via well-defined APIs (synchronous) or events (asynchronous) — no direct database access, shared file systems, or other back-channel communication.
- MUST design services to be independently deployable — deploying service A should never require simultaneously deploying service B.
- SHOULD follow the strangler fig pattern when incrementally decomposing monoliths — wrap and replace, don't rewrite.

## Event-Driven Architecture

- MUST implement idempotent event consumers — duplicate message delivery MUST NOT cause duplicate side effects (use idempotency keys, deduplication checks, or naturally idempotent operations).
- MUST include event schema versioning — consumers MUST handle unknown fields gracefully (forward compatibility).
- MUST implement dead-letter queues (DLQ) for failed event processing with monitoring and alerting on DLQ depth.
- MUST NOT rely on event ordering unless the messaging platform explicitly guarantees it for the configured topic/partition.
- SHOULD use event sourcing for audit-critical business domains where a complete history of state changes has regulatory or business value.
- MUST define and document event contracts (schema, semantics, delivery guarantees) for all published events.

## API Design

- MUST use a consistent API versioning strategy across the organization (URL path-based e.g., `/v1/` or header-based — pick one and enforce it).
- MUST follow consistent resource naming conventions and HTTP method semantics (GET reads, POST creates, PUT replaces, PATCH updates, DELETE removes).
- MUST return a standard error response structure across all APIs (consistent fields for error code, message, details, request ID).
- MUST document all APIs using OpenAPI/Swagger or an equivalent machine-readable specification.
- MUST NOT introduce breaking changes to published API versions — use a new version instead.
- MUST include request and response examples in API documentation.

## Resilience

- MUST implement circuit breakers for all external service calls — prevent cascade failures when a dependency is down.
- MUST define explicit timeouts for every network call — no indefinite waits. Document the timeout value and rationale.
- MUST implement retry with exponential backoff and jitter for transient failures — do not retry non-transient errors (4xx responses).
- MUST design for graceful degradation — partial failure of a dependency should not cause total system outage. Define fallback behavior.
- MUST NOT create single points of failure in critical paths — identify and eliminate or mitigate them.
- MUST implement bulkhead isolation — failures in one component should not consume resources needed by other components.

## Data Architecture

- MUST separate read and write models (CQRS) for systems with significantly different read/write performance characteristics or access patterns.
- MUST implement database connection pooling with appropriate pool sizes — no unbounded connection creation.
- MUST NOT perform unbounded queries — always paginate results and enforce query result limits.
- MUST have a data migration strategy that supports zero-downtime deployments (expand/contract pattern for schema changes).
- SHOULD use database transactions only for operations that truly require atomicity — prefer eventual consistency where business rules allow.
- MUST NOT use distributed transactions (two-phase commit) across service boundaries — use sagas or choreography patterns instead.
