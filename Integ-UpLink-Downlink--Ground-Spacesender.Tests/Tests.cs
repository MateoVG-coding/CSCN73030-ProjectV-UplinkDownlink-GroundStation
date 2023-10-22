using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class Tests
    : IClassFixture<WebApplicationFactory<MainProgram>>
{
    private readonly WebApplicationFactory<MainProgram> _factory;

    public Tests(WebApplicationFactory<MainProgram> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/About")]
    [InlineData("/Privacy")]
    [InlineData("/Contact")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual("text/html; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    public async Task GroundSender_Starts_Tranmissions()
    {
        //Arrange
        var sender = new GroundSender("test");

        //Act

        //Assert
    }
}