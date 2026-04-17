using Application.Interfaces;
using Application.Models;
using Application.Requests;
using Application.Validators;
using FluentValidation;

namespace Application.Handlers;

public sealed class GetDocumentMetadataHandler(
    IDocumentService documentService,
    DocumentMetadataRequestValidator validator)
{
    public async Task<IReadOnlyList<DocumentMetadataDto>> HandleAsync(
        DocumentMetadataRequest request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        return await documentService.GetMetadataAsync(request.AccountId, cancellationToken);
    }
}
