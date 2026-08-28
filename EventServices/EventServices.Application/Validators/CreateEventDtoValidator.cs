using EventServices.Application.DTO.Event;
using FluentValidation;
using System;

namespace EventServices.Application.Validators
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required.");

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes.")
                .LessThan(600).WithMessage("Duration cannot exceed 10 hours."); // Prevents unrealistic durations

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required.")
                .Must(date => date != default).WithMessage("Please provide a valid release date.");
        }
    }
}
