namespace Application.Models;

public sealed record DocumentMetadataDto(
    string FileName,
    string ContentType,
    DateTimeOffset CreatedOn,
    long SizeBytes);
