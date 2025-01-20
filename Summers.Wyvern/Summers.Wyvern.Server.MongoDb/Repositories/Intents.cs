using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Summers.Wyvern.Server.MongoDb.Database;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb.Repositories
{
	public class Intents : DataRepositoryBase<IntentEntity, IntentEntity>
	{
		private readonly string _collectionName = "Intents";

		public Intents(IDataAccessController dataAccessLayer) : base(dataAccessLayer)
		{
		}

		public override IntentEntity Create(IntentEntity model)
		{
			Context.Insert(_collectionName, model);
			return model;
		}

        public override async Task<IntentEntity> CreateAsync(IntentEntity model)
        {
            await Context.InsertAsync(_collectionName, model);
            return model;
        }

        public override IntentEntity Read(string objectID)
		{
			return Context.ReadFirstOrDefault<IntentEntity>(_collectionName, x => x.Id.Equals(objectID));
		}

        public override async Task<IntentEntity> ReadAsync(string objectID)
        {
            return await Context.ReadFirstOrDefaultAsync<IntentEntity>(_collectionName, x => x.Id.Equals(objectID));
        }

        public override async Task<IEnumerable<IntentEntity>> ReadAsync(Expression<Func<IntentEntity, bool>> predicate)
        {
            return await Context.ReadAsync<IntentEntity>(_collectionName, predicate);
        }

        public async Task<IntentEntity> ReadFirstOrDefaultAsync(Expression<Func<IntentEntity, bool>> predicate)
        {
            return await Context.ReadFirstOrDefaultAsync<IntentEntity>(_collectionName, predicate);
        }

        public long Count(Expression<Func<IntentEntity, bool>> predicate)
        {
            return Context.CountItems(_collectionName, predicate);
        }

        public async Task<long> CountAsync(Expression<Func<IntentEntity, bool>> predicate)
        {
            return await Context.CountItemsAsync<IntentEntity>(_collectionName, predicate);
        }

        public override void Update(IntentEntity model)
        {
            Context.Write(_collectionName, model);
        }

        public override async Task UpdateAsync(IntentEntity model)
        {
            await Context.WriteAsync(_collectionName, model);
        }

        public override void Delete(string objectID)
		{
            Context.Delete<IntentEntity>(_collectionName, x=>x.Id.Equals(objectID));
		}

        public override async Task DeleteAsync(string objectID)
        {
            await Context.DeleteAsync<IntentEntity>(_collectionName, x=>x.Id.Equals(objectID));
        }

        public override IEnumerable<IntentEntity> Read(Expression<Func<IntentEntity, bool>> predicate)
		{
            return Context.Read<IntentEntity>(_collectionName, predicate.Compile());
		}

		public override IEnumerable<IntentEntity> Read(string bsonSearchDefinition, string bsonSortDefinition)
		{
            return Context.Read<IntentEntity>(_collectionName,bsonSearchDefinition, bsonSortDefinition);
		}

        public override void UpdateCollection(IEnumerable<IntentEntity> modelCollection)
		{
			throw new NotImplementedException();
		}

        public override Task UpdateCollectionAsync(IEnumerable<IntentEntity> modelCollection)
        {
            throw new NotImplementedException();
        }

        protected override IntentEntity EntityToModel(IntentEntity entity)
		{
			throw new NotImplementedException();
		}

		protected override IntentEntity ModelToEntity(IntentEntity model)
		{
			throw new NotImplementedException();
		}
	}
}
