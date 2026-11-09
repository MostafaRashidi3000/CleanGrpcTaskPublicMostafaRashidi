using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Application.Features.Persons;
public static class ListPersons
{public sealed record Query(int Page=1,int PageSize=50);public sealed class Handler(IPersonRepository repo){public Task<IReadOnlyList<Person>> Handle(Query q,CancellationToken ct){var page=Math.Max(1,q.Page);var size=Math.Clamp(q.PageSize,1,200);return repo.ListAsync(page,size,ct);}}}
