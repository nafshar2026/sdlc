using Application.Interfaces;
using Polly;

namespace IntegrationTests;

public sealed class ServiceFlowTests
{
    [Fact]
    public async Task InMemoryService_ReturnsMetadataAndFile_ForExistingAccount()
    {
        var pipeline = new ResiliencePipelineBuilder().Build();
        IDocumentService service = new Infrastructure.Services.InMemoryDocumentService(pipeline);

        var metadata = await service.GetMetadataAsync("A-1001", CancellationToken.None);
        var file = await service.GetDocumentAsync("A-1001", "statement-2026-03.pdf", CancellationToken.None);

        Assert.NotEmpty(metadata);
        Assert.NotNull(file);
        Assert.Equal("application/pdf", file!.ContentType);
    }
}
