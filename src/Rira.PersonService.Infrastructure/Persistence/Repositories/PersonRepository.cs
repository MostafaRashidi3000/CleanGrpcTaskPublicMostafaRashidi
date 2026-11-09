using Microsoft.EntityFrameworkCore;
using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Infrastructure.Persistence.Repositories;
public sealed class PersonRepository(AppDbContext db):IPersonRepository
{
 public Task<Person?> GetByIdAsync(Guid id,CancellationToken ct)=>db.Persons.SingleOrDefaultAsync(x=>x.Id==id,ct);
 public Task<Person?> GetByNationalCodeAsync(string code,CancellationToken ct)=>db.Persons.SingleOrDefaultAsync(x=>x.NationalCode==code,ct);
 public async Task<IReadOnlyList<Person>> ListAsync(int page,int pageSize,CancellationToken ct)=>await db.Persons.AsNoTracking().OrderBy(x=>x.LastName).ThenBy(x=>x.FirstName).Skip((page-1)*pageSize).Take(pageSize).ToListAsync(ct);
 public Task AddAsync(Person person,CancellationToken ct)=>db.Persons.AddAsync(person,ct).AsTask();
 public void Remove(Person person)=>db.Persons.Remove(person);
 public Task<int> SaveChangesAsync(CancellationToken ct)=>db.SaveChangesAsync(ct);
}
