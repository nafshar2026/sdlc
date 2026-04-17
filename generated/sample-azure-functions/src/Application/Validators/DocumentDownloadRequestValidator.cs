using Application.Requests;
using FluentValidation;

namespace Application.Validators;

public sealed class DocumentDownloadRequestValidator : AbstractValidator<DocumentDownloadRequest>
{
    public DocumentDownloadRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .MaximumLength(64)
            .Matches("^[a-zA-Z0-9-]+$");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(128)
            .Matches("^[a-zA-Z0-9._-]+$")
            .Must(fileName => !fileName.Contains("..", StringComparison.Ordinal))
            .WithMessage("FileName contains invalid traversal pattern.");
    }
}
