using FluentValidation;
namespace Rira.PersonService.Application.Features.Persons.Validators;
public sealed class CreatePersonValidator : AbstractValidator<CreatePerson.Command>
{
 public CreatePersonValidator(){RuleFor(x=>x.FirstName).NotEmpty().MaximumLength(100);RuleFor(x=>x.LastName).NotEmpty().MaximumLength(100);RuleFor(x=>x.NationalCode).Must(Rira.PersonService.Domain.ValueObjects.NationalCode.IsValid).WithMessage("Invalid Iranian national code.");RuleFor(x=>x.BirthDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));}
}
