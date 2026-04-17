using Application.Requests;
using Application.Validators;
using FluentValidation;

namespace UnitTests;

public sealed class ValidationTests
{
    [Fact]
    public void DocumentMetadataRequest_WithValidAccountId_PassesValidation()
    {
        var validator = new DocumentMetadataRequestValidator();
        var request = new DocumentMetadataRequest("A-1001");

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void DocumentDownloadRequest_WithPathTraversal_FailsValidation()
    {
        var validator = new DocumentDownloadRequestValidator();
        var request = new DocumentDownloadRequest("A-1001", "../secret.txt");

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("traversal", StringComparison.OrdinalIgnoreCase));
    }
}
