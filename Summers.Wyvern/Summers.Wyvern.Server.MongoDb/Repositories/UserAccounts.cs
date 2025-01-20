using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Summers.Wyvern.Server.MongoDb.Database;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb.Repositories
{
	public class UserAccounts : DataRepositoryBase<UserEntity, UserEntity>
	{
		private readonly string _userAccounts = "UserAccounts";

		public UserAccounts(IDataAccessController dataAccessLayer) : base(dataAccessLayer)
		{
		}

		public override UserEntity Create(UserEntity model)
		{
			Context.Insert(_userAccounts, model);
			return model;
		}

        public override async Task<UserEntity> CreateAsync(UserEntity model)
        {
            await Context.InsertAsync(_userAccounts, model);
            return model;
        }

        public override UserEntity Read(string objectID)
		{
			return Context.ReadFirstOrDefault<UserEntity>(_userAccounts, x => x.Id.Equals(objectID));
		}

        public override async Task<UserEntity> ReadAsync(string objectID)
        {
            return await Context.ReadFirstOrDefaultAsync<UserEntity>(_userAccounts, x => x.Id.Equals(objectID));
        }

        public override async Task<IEnumerable<UserEntity>> ReadAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            return await Context.ReadAsync<UserEntity>(_userAccounts, predicate);
        }

        public async Task<UserEntity> ReadFirstOrDefaultAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            return await Context.ReadFirstOrDefaultAsync<UserEntity>(_userAccounts, predicate);
        }

        public long Count(Expression<Func<UserEntity, bool>> predicate)
        {
            return Context.CountItems(_userAccounts, predicate);
        }

        public async Task<long> CountAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            return await Context.CountItemsAsync<UserEntity>(_userAccounts, predicate);
        }

        public override void Update(UserEntity model)
        {
            Context.Write(_userAccounts, model);
        }

        public override async Task UpdateAsync(UserEntity model)
        {
            await Context.WriteAsync(_userAccounts, model);
        }

        public override void Delete(string objectID)
		{
            Context.Delete<UserEntity>(_userAccounts, x=>x.Id.Equals(objectID));
		}

        public override async Task DeleteAsync(string objectID)
        {
            await Context.DeleteAsync<UserEntity>(_userAccounts, x=>x.Id.Equals(objectID));
        }

        public override IEnumerable<UserEntity> Read(Expression<Func<UserEntity, bool>> predicate)
		{
            return Context.Read<UserEntity>(_userAccounts, predicate.Compile());
		}

		public override IEnumerable<UserEntity> Read(string bsonSearchDefinition, string bsonSortDefinition)
		{
            return Context.Read<UserEntity>(_userAccounts,bsonSearchDefinition, bsonSortDefinition);
		}

        public override void UpdateCollection(IEnumerable<UserEntity> modelCollection)
		{
			throw new NotImplementedException();
		}

        public override Task UpdateCollectionAsync(IEnumerable<UserEntity> modelCollection)
        {
            throw new NotImplementedException();
        }

        protected override UserEntity EntityToModel(UserEntity entity)
		{
			throw new NotImplementedException();
		}

		protected override UserEntity ModelToEntity(UserEntity model)
		{
			throw new NotImplementedException();
		}
	}
}
