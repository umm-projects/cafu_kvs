using System;
using CAFU.Core.Data.Entity;
using CAFU.Core.Domain.UseCase;
using CAFU.KeyValueStore.Domain.Repository;

namespace CAFU.KeyValueStore.Domain.UseCase {

    public interface IKeyValueUseCase : IUseCase {

        TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity;

        void SetEntity<TEntity>(string key, TEntity value) where TEntity : class, IEntity;

        void Save();

        void Load();

    }

    public class DefaultKeyValueUseCase : IKeyValueUseCase {

        public class Factory : DefaultUseCaseFactory<DefaultKeyValueUseCase> {

            private string savePath;

            protected override void Initialize(DefaultKeyValueUseCase instance) {
                base.Initialize(instance);

                instance.Repository = new DefaultKeyValueRepository.Factory().Create(savePath);
            }

            public override DefaultKeyValueUseCase Create() {
                throw new InvalidOperationException("Please use Factory.Create(savePath)");
            }

            public DefaultKeyValueUseCase Create(string savePath) {
                this.savePath = savePath;
                return base.Create();
            }

        }

        private IKeyValueRepository Repository { get; set; }

        public TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity {
            return this.Repository.GetEntity<TEntity>(key);
        }

        public void SetEntity<TEntity>(string key, TEntity value) where TEntity : class, IEntity {
            this.Repository.SetEntity(key, value);
        }

        public void Save() {
            this.Repository.Save();
        }

        public void Load() {
            this.Repository.Load();
        }

    }

}