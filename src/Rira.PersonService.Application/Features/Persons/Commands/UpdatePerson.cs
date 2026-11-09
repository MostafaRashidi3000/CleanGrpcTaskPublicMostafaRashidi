using FluentValidation;
using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Application.Common.Results;
using Rira.PersonService.Domain.Common.Errors;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Application.Features.Persons;

public static class UpdatePerson
{
	public sealed record Command(Guid Id, string FirstName, string LastName, string NationalCode, DateOnly BirthDate);
	public sealed class Handler(IPersonRepository repo, IValidator<Command> validator)
	{




		public async Task<Result<Person>> Handle(Command c, CancellationToken ct)
		{
			var vr = await validator.ValidateAsync(c, ct);
			if (!vr.IsValid) return

					Result<Person>.Failure(Error.Validation(string.Join("; ", vr.Errors.Select(e => e.ErrorMessage))));
			var p = await repo.GetByIdAsync(c.Id, ct); if (p is null)
				return Result<Person>.Failure(Error.NotFound("Person"));
			var duplicate = await repo.GetByNationalCodeAsync(c.NationalCode, ct);
			if (duplicate is not null && duplicate.Id != c.Id) return Result<Person>.Failure(Error.Conflict);
			p.Update(c.FirstName, c.LastName, c.NationalCode, c.BirthDate);
			await repo.SaveChangesAsync(ct); return Result<Person>.Success(p);


		}
		}
	}

