using Application.Models;

namespace Application.Interfaces;

public interface IDocumentService
{
    Task<IReadOnlyList<DocumentMetadataDto>> GetMetadataAsync(string accountId, CancellationToken cancellationToken);
    Task<DocumentFileDto?> GetDocumentAsync(string accountId, string fileName, CancellationToken cancellationToken);
}
