using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Application.Common.Results;
using Rira.PersonService.Domain.Common.Errors;
namespace Rira.PersonService.Application.Features.Persons;
public static class DeletePerson
{
 public sealed record Command(Guid Id);
 public sealed class Handler(IPersonRepository repo)
 {public async Task<Result> Handle(Command c,CancellationToken ct){var p=await repo.GetByIdAsync(c.Id,ct);if(p is null)return Result.Failure(Error.NotFound("Person"));repo.Remove(p);await repo.SaveChangesAsync(ct);return Result.Success();}}
}
