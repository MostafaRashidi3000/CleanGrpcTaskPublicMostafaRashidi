using Microsoft.EntityFrameworkCore;
using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Domain.Entities;
namespace Rira.PersonService.Infrastructure.Persistence;
public sealed class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options),IAppDbContext
{
 public DbSet<Person> Persons=>Set<Person>();
 protected override void OnModelCreating(ModelBuilder modelBuilder){modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);base.OnModelCreating(modelBuilder);}
}
