namespace Domain.Entities;

public sealed record DocumentEntry(
    string AccountId,
    string FileName,
    string ContentType,
    DateTimeOffset CreatedOn,
    byte[] Content);
