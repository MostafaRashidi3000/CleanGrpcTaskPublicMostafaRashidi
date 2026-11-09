using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Application.Common.Interfaces;
public interface IPersonRepository
{
 Task<Person?> GetByIdAsync(Guid id,CancellationToken ct);
 Task<Person?> GetByNationalCodeAsync(string code,CancellationToken ct);
 Task<IReadOnlyList<Person>> ListAsync(int page,int pageSize,CancellationToken ct);
 Task AddAsync(Person person,CancellationToken ct);
 void Remove(Person person);
 Task<int> SaveChangesAsync(CancellationToken ct);
}
