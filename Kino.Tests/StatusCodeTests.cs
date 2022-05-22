using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Kino.Tests;

public class StatusCodeTests : IClassFixture<AppFactory>
{
    private readonly AppFactory _factory;
    
    public StatusCodeTests(AppFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("GET", "/Test", HttpStatusCode.OK)]
    
    [InlineData("GET", "/Films", HttpStatusCode.OK)]
    [InlineData("GET", "/Halls", HttpStatusCode.OK)]
    [InlineData("GET", "/Reviews", HttpStatusCode.OK)]
    [InlineData("GET", "/Tickets", HttpStatusCode.OK)]
    
    [InlineData("POST", "/Films", HttpStatusCode.Unauthorized)]
    [InlineData("PUT", "/Films/0", HttpStatusCode.Unauthorized)]
    [InlineData("DELETE", "/Films/0", HttpStatusCode.Unauthorized)]
    [InlineData("GET", "/Films/0", HttpStatusCode.NotFound)]
    
    [InlineData("POST", "/Reviews", HttpStatusCode.Unauthorized)]
    [InlineData("PUT", "/Reviews/0", HttpStatusCode.Unauthorized)]
    [InlineData("DELETE", "/Reviews/0", HttpStatusCode.Unauthorized)]
    
    [InlineData("POST", "/Screenings", HttpStatusCode.Unauthorized)]
    [InlineData("PUT", "/Screenings/0", HttpStatusCode.Unauthorized)]
    [InlineData("DELETE", "/Screenings/0", HttpStatusCode.Unauthorized)]
    [InlineData("GET", "/Screenings/0", HttpStatusCode.NotFound)]
    
    [InlineData("POST", "/Sessions", HttpStatusCode.UnsupportedMediaType)]
    
    [InlineData("POST", "/Tickets", HttpStatusCode.UnsupportedMediaType)]
    [InlineData("PUT", "/Tickets/0", HttpStatusCode.Unauthorized)]
    [InlineData("DELETE", "/Tickets/0", HttpStatusCode.Unauthorized)]
        
    [InlineData("POST", "/Users", HttpStatusCode.UnsupportedMediaType)]
    [InlineData("PUT", "/Users/0", HttpStatusCode.Unauthorized)]
    [InlineData("DELETE", "/Users/0", HttpStatusCode.Unauthorized)]
    public async Task Send_EmptyRequest_ExpectedStatusCode(string method, string url, HttpStatusCode expectedStatusCode)
    {
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(new HttpMethod(method), url);

        var response = await client.SendAsync(request);

        Assert.Equal(expectedStatusCode, response.StatusCode);
    }
}