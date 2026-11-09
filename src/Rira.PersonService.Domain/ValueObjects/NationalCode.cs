using Rira.PersonService.Domain.Common.Errors;
namespace Rira.PersonService.Domain.ValueObjects;
public sealed record NationalCode
{
 public string Value { get; }
 private NationalCode(string value) => Value = value;
 public static bool IsValid(string? value)
 {
  if (string.IsNullOrWhiteSpace(value) || value.Length != 10 || !value.All(char.IsAsciiDigit) || value.All(c => c == value[0])) return false;
  var sum = 0;
  for (var i = 0; i < 9; i++) sum += (value[i] - '0') * (10 - i);
  var remainder = sum % 11; var check = value[9] - '0';
  return remainder < 2 ? check == remainder : check == 11 - remainder;
 }
 public static bool TryCreate(string? value, out NationalCode? code)
 { code = IsValid(value) ? new NationalCode(value!) : null; return code is not null; }
 public static NationalCode Create(string value) => TryCreate(value, out var code) ? code! : throw new ArgumentException("Invalid Iranian national code.", nameof(value));
 public override string ToString() => Value;
}
