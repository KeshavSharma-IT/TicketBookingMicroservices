using EventServices.Application.DTO.Venue;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.Validators
{
    public class CreateScreenDtoValidator :AbstractValidator<CreateScreenDto>
    {
        public CreateScreenDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be blank")
                .MaximumLength(100).WithMessage("Screen Name cannot exceed more then 100 Character");

            RuleFor(x => x.TotalSeats)
                .NotNull().WithMessage("Total Seats cannot be null")
                .GreaterThan(0).WithMessage("TotalSeat no must be greater the 0")
                .LessThan(1000).WithMessage("Total Seat No cannot be higher then 1000");
        }
    }
}
