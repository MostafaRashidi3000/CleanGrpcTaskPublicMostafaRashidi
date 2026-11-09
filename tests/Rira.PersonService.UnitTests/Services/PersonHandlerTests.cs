using FluentValidation;
using FluentValidation.Results;
using Rira.PersonService.Application.Common.Interfaces;
using Rira.PersonService.Application.Features.Persons;
using Rira.PersonService.Domain.Entities;
using Xunit;
namespace Rira.PersonService.UnitTests.Services;

public sealed class PersonHandlerTests
{
    [Fact]
    public async Task CreateReturnsFailureForInvalidCommand()
    {
        var repo = new FakeRepository(); var validator = new InlineValidator<CreatePerson.Command>(); validator.RuleFor(x => x.FirstName).NotEmpty();
        var handler = new CreatePerson.Handler(repo, validator); var result = await handler.Handle(new("", "Doe", "1234567890", new DateOnly(2000, 1, 1)), CancellationToken.None);
        Assert.True(result.IsFailure); Assert.Equal("validation", result.Error.Code);
    }
    private sealed class FakeRepository : IPersonRepository
    {
        public Task<Person?> GetByIdAsync(Guid id, CancellationToken ct) => Task.FromResult<Person?>(null);
        public Task<Person?> GetByNationalCodeAsync(string c, CancellationToken ct) => Task.FromResult<Person?>(null);
        public Task<IReadOnlyList<Person>> ListAsync(int p, int s, CancellationToken ct) => Task.FromResult<IReadOnlyList<Person>>([]);
        public Task AddAsync(Person p, CancellationToken ct) => Task.CompletedTask; public void Remove(Person p) { }
        public Task<int> SaveChangesAsync(CancellationToken ct) => Task.FromResult(0);
    }
}



