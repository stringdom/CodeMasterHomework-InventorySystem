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
    [Fact]
    public void CreateEmptyName()
    {
        string name = "";
        decimal value = 12m;
        Assert.Throws<ArgumentException>((Action)(() => new Product(name,value)));
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
    public void NegativePrice()
    {
        string name = "Tea";
        decimal value = -12m;
        Assert.Throws<ArgumentException>((Action)(() => new Product(name, value)));
    }

}