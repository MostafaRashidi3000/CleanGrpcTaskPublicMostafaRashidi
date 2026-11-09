using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rira.PersonService.Domain.Entities;

namespace Rira.PersonService.Infrastructure.Persistence.Configurations;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> builder)
	{
		builder.HasKey(p => p.Id);

		// صریحاً نوع ستون را برای SQL Server مشخص می‌کنیم
		builder.Property(p => p.Id)
			.HasColumnType("uniqueidentifier")
			.IsRequired();

		builder.Property(p => p.FirstName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.LastName)
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(p => p.NationalCode)
			.HasMaxLength(10)
			.IsRequired();

		builder.HasIndex(p => p.NationalCode)
			.IsUnique();

		builder.Property(p => p.BirthDate)
			.IsRequired();
	}
}
