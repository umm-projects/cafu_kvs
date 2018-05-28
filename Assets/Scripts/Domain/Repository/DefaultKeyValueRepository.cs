using System;
using CAFU.Core.Data.Entity;
using CAFU.Core.Domain.Repository;
using CAFU.KeyValueStore.Data.DataStore;

namespace CAFU.KeyValueStore.Domain.Repository
{
    public interface IKeyValueRepository : IRepository
    {
        TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity;

        void SetEntity<TEntity>(string key, TEntity value) where TEntity : class, IEntity;

        void Save();

        void Load();
    }

    public class DefaultKeyValueRepository : IKeyValueRepository
    {
        public class Factory : DefaultRepositoryFactory<DefaultKeyValueRepository>
        {
            private string savePath;

            protected override void Initialize(DefaultKeyValueRepository instance)
            {
                base.Initialize(instance);

                var dataStore = new DefaultKeyValueDataStore();
                dataStore.Initialize(savePath);
                instance.DataStore = dataStore;
            }

            /// <summary>
            /// Please use Create(savePath)
            /// </summary>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public override DefaultKeyValueRepository Create()
            {
                throw new NotImplementedException("Please use Factory.Create(savePath)");
            }

            public DefaultKeyValueRepository Create(string savePath)
            {
                this.savePath = savePath;
                return base.Create();
            }
        }

        public IKeyValueDataStore DataStore { get; set; }

        public TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity
        {
            return this.DataStore.GetEntity<TEntity>(key);
        }

        public void SetEntity<TEntity>(string key, TEntity value) where TEntity : class, IEntity
        {
            this.DataStore.SetEntity(key, value);
        }

        public void Save()
        {
            this.DataStore.Save();
        }

        public void Load()
        {
            this.DataStore.Load();
        }
    }
}