using System;
using System.Collections.Generic;
using CAFU.Core.Data.Entity;
using NUnit.Framework;
using UnityEngine;

namespace CAFU.KeyValueStore.Utility {

    public class SerializableDictionaryTest {

        [Serializable]
        public class Hoge : IEntity {

            public int Id;

            public string Name;

            public Hoge(int id, string name) {
                this.Id = id;
                this.Name = name;
            }

            public override bool Equals(object value) {
                Hoge entity = value as Hoge;

                return (entity != null)
                    && (this.Id == entity.Id)
                    && (this.Name == entity.Name)
                    ;
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }

        }

        [Serializable]
        public class SampleDictionary : SerializableDictionary<string, Hoge> {

            public SampleDictionary() : base() {
            }

            public SampleDictionary(IDictionary<string, Hoge> dictionary) : base(dictionary) {
            }

        }

        [Test]
        public void SerializeTest() {
            // []
            {
                var dictionary = new SampleDictionary();
                dictionary["key"] = new Hoge(1, "user1");
                Assert.AreEqual("{\"keys\":[\"key\"],\"values\":[{\"Id\":1,\"Name\":\"user1\"}]}", JsonUtility.ToJson(dictionary));
            }

            // Add
            {
                var dictionary = new SampleDictionary();
                dictionary.Add("key", new Hoge(1, "user1"));
                Assert.AreEqual("{\"keys\":[\"key\"],\"values\":[{\"Id\":1,\"Name\":\"user1\"}]}", JsonUtility.ToJson(dictionary));
            }

            // Add
            {
                var dictionary = new SampleDictionary();
                dictionary.Add(new KeyValuePair<string, Hoge>("key", new Hoge(1, "user1")));
                Assert.AreEqual("{\"keys\":[\"key\"],\"values\":[{\"Id\":1,\"Name\":\"user1\"}]}", JsonUtility.ToJson(dictionary));
            }

            // Clear
            {
                var dictionary = new SampleDictionary();
                dictionary.Add("key", new Hoge(1, "user1"));
                Assert.AreEqual("{\"keys\":[\"key\"],\"values\":[{\"Id\":1,\"Name\":\"user1\"}]}", JsonUtility.ToJson(dictionary));
            }

            // Remove
            {
                var dictionary = new SampleDictionary();
                var entity = new Hoge(1, "user1");
                dictionary.Add("key", entity);
                dictionary.Remove(new KeyValuePair<string, Hoge>("key", entity));
                Assert.AreEqual("{\"keys\":[],\"values\":[]}", JsonUtility.ToJson(dictionary));
            }

            // new
            {
                var dictionary = new SampleDictionary(
                    new Dictionary<string, Hoge> {
                        {
                            "key1", new Hoge(1, "user1")
                        }, {
                            "key2", new Hoge(2, "user2")
                        }
                    });

                Assert.AreEqual(
                    "{" +
                    "\"keys\":[\"key1\",\"key2\"]," +
                    "\"values\":[{\"Id\":1,\"Name\":\"user1\"},{\"Id\":2,\"Name\":\"user2\"}]" +
                    "}",
                    JsonUtility.ToJson(dictionary)
                );
            }
        }

        [Test]
        public void DeserializeTest() {
            var sample = JsonUtility.FromJson<SampleDictionary>("{\"keys\":[\"key\"],\"values\":[{\"Id\":1,\"Name\":\"user1\"}]}");
            Assert.AreEqual(1, sample.Count);
            Assert.IsTrue(sample.ContainsKey("key"));
            Assert.AreEqual(1, sample["key"].Id);
            Assert.AreEqual("user1", sample["key"].Name);
        }

    }

}