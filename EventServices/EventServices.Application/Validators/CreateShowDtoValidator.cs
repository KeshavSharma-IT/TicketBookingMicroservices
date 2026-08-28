using EventServices.Application.DTO.Show;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.Validators
{
    public class CreateShowDtoValidator :AbstractValidator<CreateShowDto>
    {
        public CreateShowDtoValidator()
        {
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price Cannot be null")                
                .GreaterThan(0).WithMessage("Price must be greater then 0");

            RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event Id is required."); // NotEmpty checks for Guid.Empty

            RuleFor(x => x.ScreenId)
                .NotEmpty().WithMessage("Screen Id is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.");

        }

    }
}
