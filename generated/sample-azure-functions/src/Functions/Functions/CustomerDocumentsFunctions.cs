using Application.Handlers;
using Application.Requests;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Functions.Functions;

public sealed class CustomerDocumentsFunctions(
    GetDocumentMetadataHandler metadataHandler,
    GetDocumentFileHandler fileHandler,
    ILogger<CustomerDocumentsFunctions> logger)
{
    [Function("GetCustomerDocumentMetadata")]
    public async Task<HttpResponseData> GetMetadataAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "customers/accounts/{accountId}/documents/metadata")] HttpRequestData req,
        string accountId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await metadataHandler.HandleAsync(new DocumentMetadataRequest(accountId), cancellationToken);

            var ok = req.CreateResponse(HttpStatusCode.OK);
            await ok.WriteAsJsonAsync(result, cancellationToken);
            return ok;
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while requesting metadata for account {AccountId}", accountId);
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new { error = "ValidationFailed", details = ex.Errors.Select(x => x.ErrorMessage) }, cancellationToken);
            return badRequest;
        }
    }

    [Function("GetCustomerDocumentFile")]
    public async Task<HttpResponseData> GetFileAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "customers/accounts/{accountId}/documents/{fileName}")] HttpRequestData req,
        string accountId,
        string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await fileHandler.HandleAsync(new DocumentDownloadRequest(accountId, fileName), cancellationToken);
            if (result is null)
            {
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteAsJsonAsync(new { error = "DocumentNotFound" }, cancellationToken);
                return notFound;
            }

            var ok = req.CreateResponse(HttpStatusCode.OK);
            ok.Headers.Add("Content-Type", result.ContentType);
            ok.Headers.Add("Content-Disposition", $"attachment; filename={result.FileName}");
            await ok.WriteBytesAsync(result.Content, cancellationToken);
            return ok;
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while requesting file {FileName} for account {AccountId}", fileName, accountId);
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteAsJsonAsync(new { error = "ValidationFailed", details = ex.Errors.Select(x => x.ErrorMessage) }, cancellationToken);
            return badRequest;
        }
    }
}
