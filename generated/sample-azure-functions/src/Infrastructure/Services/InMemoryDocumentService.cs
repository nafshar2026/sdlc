using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Polly;

namespace Infrastructure.Services;

public sealed class InMemoryDocumentService(ResiliencePipeline resiliencePipeline) : IDocumentService
{
    private static readonly IReadOnlyList<DocumentEntry> SeedDocuments =
    [
        new(
            AccountId: "A-1001",
            FileName: "statement-2026-03.pdf",
            ContentType: "application/pdf",
            CreatedOn: new DateTimeOffset(2026, 03, 31, 0, 0, 0, TimeSpan.Zero),
            Content: "Sample statement content"u8.ToArray()),
        new(
            AccountId: "A-1001",
            FileName: "statement-2026-04.pdf",
            ContentType: "application/pdf",
            CreatedOn: new DateTimeOffset(2026, 04, 15, 0, 0, 0, TimeSpan.Zero),
            Content: "Sample newer statement"u8.ToArray())
    ];

    public Task<IReadOnlyList<DocumentMetadataDto>> GetMetadataAsync(string accountId, CancellationToken cancellationToken)
    {
        return resiliencePipeline.ExecuteAsync(async _ =>
        {
            var docs = SeedDocuments
                .Where(x => string.Equals(x.AccountId, accountId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new DocumentMetadataDto(x.FileName, x.ContentType, x.CreatedOn, x.Content.LongLength))
                .ToList()
                .AsReadOnly();

            return await Task.FromResult<IReadOnlyList<DocumentMetadataDto>>(docs);
        }, cancellationToken).AsTask();
    }

    public Task<DocumentFileDto?> GetDocumentAsync(string accountId, string fileName, CancellationToken cancellationToken)
    {
        return resiliencePipeline.ExecuteAsync(async _ =>
        {
            var doc = SeedDocuments.FirstOrDefault(x =>
            string.Equals(x.AccountId, accountId, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.FileName, fileName, StringComparison.OrdinalIgnoreCase));

            if (doc is null)
            {
                return await Task.FromResult<DocumentFileDto?>(null);
            }

            return await Task.FromResult<DocumentFileDto?>(new DocumentFileDto(doc.FileName, doc.ContentType, doc.Content));
        }, cancellationToken).AsTask();
    }
}
