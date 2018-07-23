using System;
using CAFU.Core.Data.Entity;
using NUnit.Framework;
using UnityEngine;

namespace CAFU.KeyValueStore.Domain.Repository
{
    public class KeyValueRepositoryTest
    {
        private static readonly string DataPath = Application.persistentDataPath + "/test.kv";

        private IKeyValueRepository KeyValueRepository { get; set; }

        [Serializable]
        private class SampleEntity : IEntity
        {
            [SerializeField] private int id;

            public int Id
            {
                get { return id; }
                set { id = value; }
            }

            [SerializeField] private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public SampleEntity()
            {
            }

            public SampleEntity(int id, string name) : this()
            {
                Id = id;
                Name = name;
            }
        }


        [SetUp]
        public void SetUp()
        {
            KeyValueRepository = new DefaultKeyValueRepository.Factory().Create(DataPath);
            KeyValueRepository.Load();
        }

        [TearDown]
        public void TearDown()
        {
            ((DefaultKeyValueRepository)KeyValueRepository).DataStore.Clear();
        }

        [Test]
        public void GetOrCreateEntityTest()
        {
            {
                var entity = KeyValueRepository.GetEntity<SampleEntity>("key");
                Assert.IsNull(entity);
            }

            {
                KeyValueRepository.Load();
                var entity = KeyValueRepository.GetOrCreateEntity<SampleEntity>("key");
                Assert.IsNotNull(entity);
                Assert.AreEqual(0, entity.Id);
                Assert.AreEqual(null, entity.Name);

                entity.Id = 1;
                entity.Name = "foo";
                Assert.AreEqual(1, entity.Id);
                Assert.AreEqual("foo", entity.Name);

                // Save without SetEntity
                KeyValueRepository.Save();
            }

            {
                KeyValueRepository.Load();
                var entity = KeyValueRepository.GetEntity<SampleEntity>("key");
                Assert.IsNull(entity);
            }

            {
                KeyValueRepository.Load();
                var entity = KeyValueRepository.GetOrCreateEntity<SampleEntity>("key");
                Assert.IsNotNull(entity);
                Assert.AreEqual(0, entity.Id);
                Assert.AreEqual(null, entity.Name);
                entity.Id = 1;
                entity.Name = "foo";

                KeyValueRepository.SetEntity("key", entity);
                KeyValueRepository.Save();
            }

            {
                KeyValueRepository.Load();
                var entity = KeyValueRepository.GetEntity<SampleEntity>("key");
                Assert.IsNotNull(entity);
                Assert.AreEqual(1, entity.Id);
                Assert.AreEqual("foo", entity.Name);
            }
        }
    }
}