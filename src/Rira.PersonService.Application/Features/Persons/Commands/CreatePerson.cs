using FluentValidation;
using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Application.Common.Results;
using Rira.PersonService.Domain.Common.Errors;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Application.Features.Persons;

public static class CreatePerson
{
	public sealed record Command(string FirstName, string LastName, string NationalCode, DateOnly BirthDate);
	public sealed class Handler(IPersonRepository repo, IValidator<Command> validator)
	{

		public async Task<Result<Person>> Handle(Command c, CancellationToken ct) { var vr = await validator.ValidateAsync(c, ct); if (!vr.IsValid) return Result<Person>.Failure(Error.Validation(string.Join("; ", vr.Errors.Select(e => e.ErrorMessage)))); if (await repo.GetByNationalCodeAsync(c.NationalCode, ct) is not null) return Result<Person>.Failure(Error.Conflict); var person = Person.Create(c.FirstName, c.LastName, c.NationalCode, c.BirthDate); await repo.AddAsync(person, ct); await repo.SaveChangesAsync(ct); return Result<Person>.Success(person); }
	}
}
