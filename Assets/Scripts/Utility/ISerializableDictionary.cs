using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ExtraLinq;
using UnityEngine;

namespace CAFU.KeyValueStore.Utility {

    public interface ISerializableDictionary<TKey, TEntity> : IDictionary<TKey, TEntity> {

    }

    /// <summary>
    /// JsonUtilityでもSerialize可能な汎用的なDictionary. 無駄なチェックがあるので少し遅い
    /// </summary>
    /// <typeparam name="TKey">dictionary key</typeparam>
    /// <typeparam name="TEntity">dictionary value</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TEntity> : ISerializableDictionary<TKey, TEntity>, ISerializationCallbackReceiver {

        [SerializeField]
        private List<TKey> keys;

        public ICollection<TKey> Keys => this.keys;

        [SerializeField]
        private List<TEntity> values;

        public ICollection<TEntity> Values => this.values;

        public TEntity this[TKey key] {
            get {
                this.DeserializeSyncIfEmpty();
                return this.dictionary[key];
            }
            set {
                this.dictionary[key] = value;
                this.SerializeSync();
            }
        }

        public int Count {
            get {
                this.DeserializeSyncIfEmpty();
                return this.dictionary.Count;
            }
        }

        public bool IsReadOnly => this.dictionary.IsReadOnly;

        private IDictionary<TKey, TEntity> dictionary;

        public SerializableDictionary() {
            this.dictionary = new Dictionary<TKey, TEntity>();
        }

        public SerializableDictionary(IDictionary<TKey, TEntity> dictionary) {
            this.dictionary = dictionary;
            this.SerializeSync();
        }

        public IEnumerator<KeyValuePair<TKey, TEntity>> GetEnumerator() {
            this.DeserializeSyncIfEmpty();
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            this.DeserializeSyncIfEmpty();
            return this.dictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TEntity> item) {
            this.dictionary[item.Key] = item.Value;
            this.SerializeSync();
        }

        public void Add(TKey key, TEntity value) {
            this.dictionary[key] = value;
            this.SerializeSync();
        }

        public void Clear() {
            this.dictionary.Clear();
            this.SerializeSync();
        }

        public bool Contains(KeyValuePair<TKey, TEntity> item) {
            this.DeserializeSyncIfEmpty();
            return this.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TEntity>[] array, int arrayIndex) {
            this.DeserializeSyncIfEmpty();
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public bool ContainsKey(TKey key) {
            this.DeserializeSyncIfEmpty();
            return this.dictionary.ContainsKey(key);
        }

        public bool Remove(KeyValuePair<TKey, TEntity> item) {
            var result = this.dictionary.Remove(item);
            this.SerializeSync();
            return result;
        }

        public bool Remove(TKey key) {
            var result = this.dictionary.Remove(key);
            this.SerializeSync();
            return result;
        }

        public bool TryGetValue(TKey key, out TEntity value) {
            this.DeserializeSyncIfEmpty();
            return this.dictionary.TryGetValue(key, out value);
        }

        public void SerializeSync() {
            this.keys = this.dictionary.Keys.ToList();
            this.values = this.dictionary.Values.ToList();
        }

        public void DeserializeSync() {
            var count = Math.Min(this.keys.Count, this.values.Count);

            for (var i = 0; i < count; i++) {
                this.dictionary[this.keys[i]] = this.values[i];
            }
        }

        // XXX: check first time deserialization on everytime accessing dictionary properties
        private void DeserializeSyncIfEmpty() {
            if (this.dictionary.IsEmpty() && this.keys != null && !this.keys.IsEmpty()) {
                this.DeserializeSync();
            }
        }

        // XXX: JsonUtility is not handling the method..
        [OnSerializing]
        public void OnSerializing(StreamingContext context) {
            this.SerializeSync();
        }

        // XXX: JsonUtility is not handling the method..
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context) {
            this.DeserializeSync();
        }

        public void OnBeforeSerialize() {
            this.SerializeSync();
        }

        public void OnAfterDeserialize() {
            this.DeserializeSync();
        }

    }

}