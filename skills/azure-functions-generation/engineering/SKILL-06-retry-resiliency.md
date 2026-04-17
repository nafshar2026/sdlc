# Retry and Resiliency

- Register a singleton Polly resilience pipeline in dependency injection.
- Inject the resilience pipeline into services; do not build per-call pipelines inline.
- If retry count is 0, skip retry strategy and execute once.
- Use Polly 8 directly when not using IHttpClientFactory policies.
- Log retry attempt number, max attempts, and delay in OnRetry.
