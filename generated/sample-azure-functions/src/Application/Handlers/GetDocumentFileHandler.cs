using Application.Interfaces;
using Application.Models;
using Application.Requests;
using Application.Validators;
using FluentValidation;

namespace Application.Handlers;

public sealed class GetDocumentFileHandler(
    IDocumentService documentService,
    DocumentDownloadRequestValidator validator)
{
    public async Task<DocumentFileDto?> HandleAsync(
        DocumentDownloadRequest request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        return await documentService.GetDocumentAsync(request.AccountId, request.FileName, cancellationToken);
    }
}
