using Microsoft.EntityFrameworkCore;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Application.Common.Interfaces;
public interface IAppDbContext
{
 DbSet<Person> Persons { get; }
 Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
}
