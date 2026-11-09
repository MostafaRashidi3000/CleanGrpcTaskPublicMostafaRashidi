namespace Rira.PersonService.Domain.Common.Errors;
public sealed record Error(string Code, string Description)
{
 public static readonly Error None = new("", "");
 public static Error NotFound(string entity) => new("not_found", $"{entity} was not found.");
 public static Error Validation(string message) => new("validation", message);
 public static readonly Error Conflict = new("conflict", "A person with this national code already exists.");
}
