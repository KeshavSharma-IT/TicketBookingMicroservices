using EventServices.Application.DTO.Venue;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.Validators
{
    public class CreateVenueDtoValidator :AbstractValidator<CreateVenueDto>
    {
        public CreateVenueDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is Required")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 Character");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address cannot be blank")
                .MaximumLength(1000).WithMessage("Address cannot exceed 1000 character");

                RuleFor(x => x.City)
                .NotEmpty().WithMessage("city cannot be blank")
                .MaximumLength(20).WithMessage("City cannot exceed 20 character");
        }
    }
}
