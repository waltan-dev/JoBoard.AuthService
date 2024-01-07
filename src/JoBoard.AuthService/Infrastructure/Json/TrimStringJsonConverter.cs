using System.Text.Json;
using System.Text.Json.Serialization;

namespace JoBoard.AuthService.Infrastructure.Json;

public class TrimStringJsonConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
        => reader.GetString()?.Trim();

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) 
        => writer.WriteStringValue(value);
}