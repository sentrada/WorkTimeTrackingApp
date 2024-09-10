using System.Text.Json;
using System.Text.Json.Serialization;
using CQRS.Core.Events;
using UserManagement.Common.Events;

namespace UserManagement.QueryService.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }

        public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions? options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out JsonDocument? doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}!");
            }

            if (!doc.RootElement.TryGetProperty("Type", out JsonElement type))
            {
                throw new JsonException("Could not detect the Type discriminator property!");
            }

            string? typeDiscriminator = type.GetString();
            string json = doc.RootElement.GetRawText();
            return (typeDiscriminator switch
            {
                nameof(UserCreatedEvent) => JsonSerializer.Deserialize<UserCreatedEvent>(json, options),
                nameof(UserUpdatedEvent) => JsonSerializer.Deserialize<UserUpdatedEvent>(json, options),
                nameof(UserDeletedEvent) => JsonSerializer.Deserialize<UserDeletedEvent>(json, options),
                _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
            });
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}