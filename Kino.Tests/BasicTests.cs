using System.Threading.Tasks;
using Xunit;

namespace Kino.Tests;

public class BasicTests : IClassFixture<AppFactory>
{
    private readonly AppFactory _factory;
    
    public BasicTests(AppFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/Test")]
    public async Task Get_Test(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync(url);
        
        // Assert
        response.EnsureSuccessStatusCode();
        string responseText = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World!", responseText);
    }
}