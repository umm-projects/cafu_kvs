namespace CAFU.KeyValueStore.Utility
{
    public interface IStringDataSerializer
    {
        /// <summary>
        /// Serialize value to string
        /// </summary>
        /// <param name="value">value to serialize</param>
        /// <typeparam name="TValue">any value which can serialize to string</typeparam>
        /// <returns></returns>
        string Serialize<TValue>(TValue value);
        
        /// <summary>
        /// Deserialize string to value
        /// </summary>
        /// <param name="text">text to deserialize</param>
        /// <typeparam name="TValue">any value which can serialize to string</typeparam>
        /// <returns></returns>
        TValue Deserialize<TValue>(string text);
    }
}