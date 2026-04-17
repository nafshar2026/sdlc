using Application.Requests;
using FluentValidation;

namespace Application.Validators;

public sealed class DocumentMetadataRequestValidator : AbstractValidator<DocumentMetadataRequest>
{
    public DocumentMetadataRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .MaximumLength(64)
            .Matches("^[a-zA-Z0-9-]+$");
    }
}
