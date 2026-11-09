namespace Rira.PersonService.Domain.Entities;

public sealed class Person
{
	private Person() { }

	public Guid Id { get; private set; }
	public string FirstName { get; private set; } = "";
	public string LastName { get; private set; } = "";
	public string NationalCode { get; private set; } = "";
	public DateOnly BirthDate { get; private set; }

	public static Person Create(string firstName, string lastName, string nationalCode, DateOnly birthDate)
	{
		// اینجا می‌توانی اعتبارسنچی ساده یا Trim را انجام دهی
		return new Person
		{
			Id = Guid.NewGuid(),
			FirstName = firstName.Trim(),
			LastName = lastName.Trim(),
			NationalCode = nationalCode.Trim(),
			BirthDate = birthDate
		};
	}

	public void Update(string firstName, string lastName, string nationalCode, DateOnly birthDate)
	{
		FirstName = firstName.Trim();
		LastName = lastName.Trim();
		NationalCode = nationalCode.Trim();
		BirthDate = birthDate;
	}
}
