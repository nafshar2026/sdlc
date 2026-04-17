namespace Application.Models;

public sealed record DocumentFileDto(
    string FileName,
    string ContentType,
    byte[] Content);
