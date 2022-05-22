using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Kino.Tests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public BasicTests(WebApplicationFactory<Program> factory)
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