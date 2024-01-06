using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using JoBoard.AuthService.Infrastructure;

namespace JoBoard.AuthService.UnitTests.API.Infrastructure;

public class TrimStringJsonConverterTests
{
    [Fact]
    public void TrimString()
    {
        var jsonLiteral = "\"   test value  \"";
        var converter = new TrimStringJsonConverter();
        var utf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonLiteral));
        utf8JsonReader.Read();
        
        var result = converter.Read(ref utf8JsonReader, typeof(string), new JsonSerializerOptions());
        
        Assert.Equal("test value", result);
    }
}