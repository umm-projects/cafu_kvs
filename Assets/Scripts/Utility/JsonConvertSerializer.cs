using Newtonsoft.Json;

namespace CAFU.KeyValueStore.Utility
{
    public class JsonConvertSerializer : IStringDataSerializer
    {
        public string Serialize<TValue>(TValue value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public TValue Deserialize<TValue>(string text)
        {
            return JsonConvert.DeserializeObject<TValue>(text);
        }
    }
}