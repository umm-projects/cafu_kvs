using System;
using System.IO;
using CAFU.Core.Data.DataStore;
using CAFU.Core.Data.Entity;
using NUnit.Framework;
using UnityEngine;

namespace CAFU.KeyValueStore.Data.DataStore
{
    public class KeyValueDataStoreTest
    {
        private static readonly string DATA_PATH = Application.persistentDataPath + "/test.kv";

        private IKeyValueDataStore dataStore = new CustomKeyValueDataStore();

        class CustomKeyValueDataStore : KeyValueDataStore
        {
            public class Factory : DefaultDataStoreFactory<CustomKeyValueDataStore>
            {
            }

            protected override string CreatePath()
            {
                return DATA_PATH;
            }
        }

        [Serializable]
        class SampleEntity : IEntity
        {
            public int Id;

            public string Name;

            public SampleEntity(int id, string name)
            {
                this.Id = id;
                this.Name = name;
            }
        }

        [SetUp]
        public void SetUp()
        {
            this.dataStore = new CustomKeyValueDataStore.Factory().Create();
            this.dataStore.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            this.dataStore.Clear();
        }

        [Test]
        public void SaveLoadSetGetEntityTest()
        {
            this.dataStore.SetEntity("key", new SampleEntity(1, "user1"));

            Assert.IsFalse(File.Exists(DATA_PATH));
            this.dataStore.Save();
            Assert.IsTrue(File.Exists(DATA_PATH));

            var dataStore2 = new CustomKeyValueDataStore();
            dataStore2.Load();

            var result = dataStore2.GetEntity<SampleEntity>("key");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("user1", result.Name);
        }
    }
}