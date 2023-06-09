using System.Globalization;

namespace InventorySystemUnitTests;

public class FileOperatorUnitTests
{
    [Fact]
    public void CorrectCulture()
    {
        Assert.Equal(CultureInfo.GetCultureInfo("es-ES"), FileOperator.Culture);
    }
}
