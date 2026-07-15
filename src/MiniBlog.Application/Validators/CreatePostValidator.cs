using FluentValidation;
using MiniBlog.Application.DTOs;

namespace MiniBlog.Application.Validators
{
    public class CreatePostValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title Required")
                .MaximumLength(150);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content Required")
                .MinimumLength(10);
        }
    }
}
