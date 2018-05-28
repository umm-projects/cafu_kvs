using System;
using System.IO;
using CAFU.Core.Data.DataStore;
using CAFU.Core.Data.Entity;
using CAFU.KeyValueStore.Utility;
using ExtraLinq;
using UnityEngine;

namespace CAFU.KeyValueStore.Data.DataStore
{
    public interface IKeyValueDataStore : IDataStore
    {
        /// <summary>
        /// Save dictionary data into file
        /// </summary>
        void Save();

        /// <summary>
        /// Load dictionary data into memory
        /// </summary>
        void Load();

        /// <summary>
        /// Clear dictionary memory
        /// </summary>
        void Clear();

        /// <summary>
        /// Get Entity by key. Entity is serialized from memory.
        /// </summary>
        /// <param name="key">dictionary key</param>
        /// <typeparam name="TEntity">The entity to fetch</typeparam>
        /// <returns>Fetched entity</returns>
        TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity;

        void SetEntity<TEntity>(string key, TEntity entity) where TEntity : class, IEntity;
    }

    public abstract class KeyValueDataStore : IKeyValueDataStore
    {
        [Serializable]
        class KeyValueDictionary : SerializableDictionary<string, string>
        {
        }

        private KeyValueDictionary dictionary = new KeyValueDictionary();

        /// <summary>
        /// Implement key value data store location.
        /// e.g. `return UnityEngine.Application.persistentDataPath + "/default.kv"`
        /// </summary>
        /// <returns></returns>
        protected abstract string CreatePath();

        public void Load()
        {
            var path = this.CreatePath();

            if (!File.Exists(path))
            {
                this.dictionary.Clear();
                return;
            }

            // FIXME: 難読化する
            var text = File.ReadAllText(path);
            this.dictionary = JsonUtility.FromJson<KeyValueDictionary>(text);
            this.dictionary.DeserializeSync();
        }

        public void Save()
        {
            var path = this.CreatePath();

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            // FIXME: 難読化する
            File.WriteAllText(path, JsonUtility.ToJson(this.dictionary));
        }

        public void Clear()
        {
            var path = this.CreatePath();

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            this.dictionary.Clear();
        }

        public TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity
        {
            if (!this.dictionary.ContainsKey(key))
            {
                return default(TEntity);
            }

            var value = this.dictionary[key];
            return JsonUtility.FromJson<TEntity>(value);
        }

        public void SetEntity<TEntity>(string key, TEntity entity) where TEntity : class, IEntity
        {
            var value = JsonUtility.ToJson(entity);
            this.dictionary[key] = value;
        }
    }

    // 毎回DataStoreをimplementするのが面倒
    public class DefaultKeyValueDataStore : KeyValueDataStore
    {
        private string savePath;

        public DefaultKeyValueDataStore()
        {
        }

        public void Initialize(string savePath)
        {
            this.savePath = savePath;
        }

        protected override string CreatePath()
        {
            if (this.savePath.IsNullOrEmpty())
                throw new InvalidOperationException("Please initialize save path");
            return this.savePath;
        }
    }
}