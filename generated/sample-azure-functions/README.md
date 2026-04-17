# Sample Azure Functions Output

This sample was generated from the requirements workflow in the repository.

## Solution Layout
- `src/Functions` : Azure Functions isolated worker HTTP triggers
- `src/Application` : request models, validation, handlers, interfaces
- `src/Domain` : domain entities
- `src/Infrastructure` : service implementation with Polly resilience pipeline
- `tests/UnitTests` : validator-focused unit tests
- `tests/IntegrationTests` : service flow integration-style tests

## Run Locally
```powershell
cd ./generated/sample-azure-functions/src/Functions
dotnet run
```

## Build and Test
```powershell
cd ./generated/sample-azure-functions
dotnet build ./Sample.AzureFunctions.slnx
dotnet test ./Sample.AzureFunctions.slnx
```
