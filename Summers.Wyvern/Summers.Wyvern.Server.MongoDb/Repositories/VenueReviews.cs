using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Summers.Wyvern.Server.MongoDb.Database;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb.Repositories
{
	public class VenueReviews : DataRepositoryBase<VenueReview, VenueReview>
	{
		private readonly string _collectionName = "VenueReviews";

		public VenueReviews(IDataAccessController dataAccessLayer) : base(dataAccessLayer)
		{
		}

		public override VenueReview Create(VenueReview model)
		{
			Context.Insert(_collectionName, model);
			return model;
		}

        public override async Task<VenueReview> CreateAsync(VenueReview model)
        {
            await Context.InsertAsync(_collectionName, model);
            return model;
        }

        public override VenueReview Read(string objectID)
		{
			return Context.ReadFirstOrDefault<VenueReview>(_collectionName, x => x.Id.Equals(objectID));
		}

        public override async Task<VenueReview> ReadAsync(string objectID)
        {
            return await Context.ReadFirstOrDefaultAsync<VenueReview>(_collectionName, x => x.Id.Equals(objectID));
        }

        public override async Task<IEnumerable<VenueReview>> ReadAsync(Expression<Func<VenueReview, bool>> predicate)
        {
            return await Context.ReadAsync<VenueReview>(_collectionName, predicate);
        }

        public async Task<VenueReview> ReadFirstOrDefaultAsync(Expression<Func<VenueReview, bool>> predicate)
        {
            return await Context.ReadFirstOrDefaultAsync<VenueReview>(_collectionName, predicate);
        }

        public long Count(Expression<Func<VenueReview, bool>> predicate)
        {
            return Context.CountItems(_collectionName, predicate);
        }

        public async Task<long> CountAsync(Expression<Func<VenueReview, bool>> predicate)
        {
            return await Context.CountItemsAsync<VenueReview>(_collectionName, predicate);
        }

        public override void Update(VenueReview model)
        {
            Context.Write(_collectionName, model);
        }

        public override async Task UpdateAsync(VenueReview model)
        {
            await Context.WriteAsync(_collectionName, model);
        }

        public override void Delete(string objectID)
		{
            Context.Delete<VenueReview>(_collectionName, x=>x.Id.Equals(objectID));
		}

        public override async Task DeleteAsync(string objectID)
        {
            await Context.DeleteAsync<VenueReview>(_collectionName, x=>x.Id.Equals(objectID));
        }

        public override IEnumerable<VenueReview> Read(Expression<Func<VenueReview, bool>> predicate)
		{
            return Context.Read<VenueReview>(_collectionName, predicate.Compile());
		}

		public override IEnumerable<VenueReview> Read(string bsonSearchDefinition, string bsonSortDefinition)
		{
            return Context.Read<VenueReview>(_collectionName,bsonSearchDefinition, bsonSortDefinition);
		}

        public override void UpdateCollection(IEnumerable<VenueReview> modelCollection)
		{
			throw new NotImplementedException();
		}

        public override Task UpdateCollectionAsync(IEnumerable<VenueReview> modelCollection)
        {
            throw new NotImplementedException();
        }

        protected override VenueReview EntityToModel(VenueReview entity)
		{
			throw new NotImplementedException();
		}

		protected override VenueReview ModelToEntity(VenueReview model)
		{
			throw new NotImplementedException();
		}
	}
}
