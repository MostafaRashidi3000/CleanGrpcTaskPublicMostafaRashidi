using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Application.Common.Results;
using Rira.PersonService.Domain.Common.Errors;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Application.Features.Persons;
public static class GetPersonById
{public sealed record Query(Guid Id);public sealed class Handler(IPersonRepository repo){public async Task<Result<Person>> Handle(Query q,CancellationToken ct){var p=await repo.GetByIdAsync(q.Id,ct);return p is null?Result<Person>.Failure(Error.NotFound("Person")):Result<Person>.Success(p);}}}
