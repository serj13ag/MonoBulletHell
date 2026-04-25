using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoBulletHell.AnimatedValues;

public class AnimatedFloatConverter : JsonConverter<IAnimatedFloat>
{
    public override void WriteJson(JsonWriter writer, IAnimatedFloat value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    public override IAnimatedFloat ReadJson(JsonReader reader, Type objectType, IAnimatedFloat existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        switch (token.Type)
        {
            case JTokenType.Float or JTokenType.Integer:
                return new ConstantAnimatedFloat(token.Value<float>());
            case JTokenType.Object:
                var data = token.ToObject<CurveAnimatedFloatData>(serializer);
                return new CurveAnimatedFloat(data);
            default:
                throw new JsonException($"Cannot convert token of type {token.Type} to IAnimatedFloat");
        }
    }
}