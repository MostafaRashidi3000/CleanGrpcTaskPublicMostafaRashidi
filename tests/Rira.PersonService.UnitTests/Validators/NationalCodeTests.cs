using Rira.PersonService.Domain.ValueObjects;
using Xunit;
namespace Rira.PersonService.UnitTests.Validators;
public sealed class NationalCodeTests
{
 private static string Generate(string prefix){var sum=0;for(var i=0;i<9;i++)sum+=(prefix[i]-'0')*(10-i);var r=sum%11;return prefix+(r<2?r:11-r).ToString();}
 [Fact] public void AcceptsValidIranianNationalCode(){Assert.True(NationalCode.IsValid(Generate("123456789")));}
 [Theory][InlineData(null)][InlineData("")][InlineData("123")][InlineData("0000000000")][InlineData("1234567890")]
 public void RejectsInvalidNationalCode(string? value){Assert.False(NationalCode.IsValid(value));}
 [Fact] public void CreateThrowsForInvalidValue(){Assert.Throws<ArgumentException>(()=>NationalCode.Create("1234567890"));}
}
