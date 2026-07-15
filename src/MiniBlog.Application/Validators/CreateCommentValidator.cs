using FluentValidation;
using MiniBlog.Application.DTOs;

namespace MiniBlog.Application.Validators
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment's Text Required")
                .MaximumLength(500);
        }
    }
}
