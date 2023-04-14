using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Config.JSON
{
    public class JsonTimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString()!, "t");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("t"));
        }
    }
}
