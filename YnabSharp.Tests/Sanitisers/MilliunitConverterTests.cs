using YnabSharpa.Sanitisers;

namespace YnabSharp.Tests.Sanitisers;

[TestFixture]
public class MilliunitConverterTests
{
    [Test]
    public void GivenMillunitValue_WhenCalculate_ConvertsToDecimal()
    {
        var expectedValue = 12.90m;
        var convertedToEquivelant = expectedValue * 1000m;
        var milliunitValue = (int)convertedToEquivelant;

        var result = MilliunitConverter.Calculate(milliunitValue);

        Assert.That(result, Is.EqualTo(expectedValue));
    }
}