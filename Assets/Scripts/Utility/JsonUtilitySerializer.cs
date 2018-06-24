using UnityEngine;

namespace CAFU.KeyValueStore.Utility
{
    public class JsonUtilitySerializer : IStringDataSerializer
    {
        public string Serialize<TValue>(TValue value)
        {
            return JsonUtility.ToJson(value);
        }

        public TValue Deserialize<TValue>(string text)
        {
            return JsonUtility.FromJson<TValue>(text);
        }
    }
}