namespace InventorySystemUnitTests;

public class ProductUnitTests
{
    [Fact]
    public void CreateNewProduct()
    {
        string name = "Tea";
        decimal value = 12m;
        Product testProduct = new(name, value);
        Assert.Equal(name, testProduct.Name);
        Assert.Equal(value, testProduct.Price);
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateInvalidName(string value)
    {
        decimal price = 0m;
        Assert.Throws<ArgumentException>(() => new Product(value, price));
    }
    [Fact]
    public void CheckProductName()
    {
        string name = "Tea";
        decimal value = 12m;
        Product testProduct = new(name, value);
        Assert.True(testProduct.IsEqual(name));
    }
    [Fact]
    public void CheckPrice()
    {
        string name = "Tea";
        decimal value = 12m;
        Product testProduct = new(name, value);
        Assert.True(testProduct.IsEqual(value));
    }
    [Fact]
    public void NegativePrice()
    {
        string name = "Tea";
        decimal value = -12m;
        Assert.Throws<ArgumentException>(() => new Product(name, value));
    }

}