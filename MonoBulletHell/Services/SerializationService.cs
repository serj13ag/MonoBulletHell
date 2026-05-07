using Newtonsoft.Json;

namespace MonoBulletHell.Services;

public interface ISerializationService
{
    T DeserializeObject<T>(string json);
    string SerializeObject<T>(T data);
}

public class SerializationService : ISerializationService
{
    public T DeserializeObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Error,
        });
    }

    public string SerializeObject<T>(T data)
    {
        return JsonConvert.SerializeObject(data, Formatting.Indented);
    }
}