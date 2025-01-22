using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Summers.Wyvern.Common;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	internal class MongoDatabase : Guarded, IDatabase
	{
        private readonly IMongoDatabase _dataBase;

		/// <summary>
		/// Initializes a new instance of the <see cref="MongoDatabase"/> class.
		/// </summary>
		/// <param name="connectionManager">The connection manager.</param>
		/// <param name="databaseName">Name of the database.</param>
		public MongoDatabase(IMongoConnectionManager connectionManager, string databaseName)
		{
            Guard(connectionManager, nameof(connectionManager));
			Guard(databaseName, nameof(databaseName));

            _dataBase = connectionManager.GetDatabase(databaseName);
		}

		/// <summary>
		/// Creates the collection.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool CreateCollection<T>(string collectionName) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			_dataBase.CreateCollection(collectionName);
			return true;
		}

        /// <summary>
        /// Retrieves the full list of collections for the currently connected database
        /// </summary>
        /// <returns><c>IEnumerable</c> collection of document collections </returns>
        public IEnumerable<string> CollectionList()
        {
            return _dataBase.ListCollections().ToList().Select(i => i["name"].ToString());
        }

		/// <summary>
		/// Creates a capped collection.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="maxDocs">The maximum docs.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool CreateCappedCollection<T>( string collectionName, int maxDocs ) where T : EntityBase
		{
			if( string.IsNullOrWhiteSpace( collectionName ) )
				LogAndThrow<ArgumentNullException>( nameof( collectionName ) );
			
			long maximumCollectionBytesSize = maxDocs;
			maximumCollectionBytesSize *= 256;

			_dataBase.CreateCollection( collectionName, 
				new CreateCollectionOptions
				{
					Capped = true,
					MaxSize = maximumCollectionBytesSize,
					MaxDocuments = maxDocs
				});
			return true;
		}

		/// <summary>
		/// Creates index for a collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="ascending">if set to <c>true</c> [ascending].</param>
		/// <returns></returns>
		public async Task CreateIndex<T>(string collectionName, string fieldName, bool ascending) where T : EntityBase
		{
			//TODO: INDEX THIS STUFF!
			await Task.FromResult(true);

			//await _dataBase.GetCollection<T>(collectionName)
			//	.Indexes
			//	.CreateOneAsync(ascending?
			//	Builders<T>.IndexKeys.Ascending(fieldName) :
			//	Builders<T>.IndexKeys.Descending(fieldName));
		}

		/// <summary>
		/// Deletes the specified item from the database
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool Delete<T>(string collectionName, T item) where T : EntityBase
		{
			//note: MongoDB doesn't error when you delete an item that doesn't exist!
			Guard(collectionName, nameof(collectionName));
			Guard(item, nameof(item));

			var filter = new ObjectFilterDefinition<T>(item);

			_dataBase.GetCollection<T>(collectionName).FindOneAndDelete(filter);
			return true;
		}

		/// <summary>
		/// Deletes all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool Delete<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase
		{
			//note: MongoDB doesn't error when you delete an item that doesn't exist!
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));

			var filter = Builders<T>.Filter.Where(whereExpression);

			_dataBase.GetCollection<T>(collectionName).DeleteMany(filter);
			return true;
		}

		/// <summary>
		/// Deletes the specified item from the database asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public Task<DeleteResult> DeleteAsync<T>(string collectionName, T item) where T : EntityBase
		{
			//note: MongoDB doesn't error when you delete an item that doesn't exist!
			Guard(collectionName, nameof(collectionName));
			Guard(item, nameof(item));

			var filter = new ObjectFilterDefinition<T>(item);

			return _dataBase.GetCollection<T>(collectionName).DeleteOneAsync(filter);
		}

		/// <summary>
		/// Deletes all matching items from the collection asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public Task<DeleteResult> DeleteAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase
		{
			//note: MongoDB doesn't error when you delete an item that doesn't exist!
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));

			var filter = Builders<T>.Filter.Where(whereExpression);

			return _dataBase.GetCollection<T>(collectionName).DeleteManyAsync(filter);
		}

		/// <summary>
		/// Drops a collection.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool DropCollection<T>(string collectionName) where T : EntityBase
		{
			//note: MongoDB doesn't error when you drop a collection that doesn't exist!
			Guard(collectionName, nameof(collectionName));

			return true;
		}

		/// <summary>
		/// Reads all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="bsonSearchDefinition">The bson search definition.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public IEnumerable<T> Read<T>(string collectionName, string bsonSearchDefinition) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(bsonSearchDefinition, nameof(bsonSearchDefinition));

			//We create a filter definition model to validate the provided definition string
			FilterDefinition<T> filter = bsonSearchDefinition;
			return _dataBase.GetCollection<T>(collectionName).Find(filter).ToEnumerable();
		}

		/// <summary>
		/// Reads all matching items from the collection and sorts them based on the sort definition
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="bsonSearchDefinition">The bson search definition.</param>
		/// <param name="bsonSortDefinition">The bson sort definition.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public IEnumerable<T> Read<T>(string collectionName, string bsonSearchDefinition, string bsonSortDefinition) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(bsonSearchDefinition, nameof(bsonSearchDefinition));
			Guard(bsonSortDefinition, nameof(bsonSortDefinition));

			//We create a filter definition model to validate the provided definition string
			FilterDefinition<T> filter = bsonSearchDefinition;			
			//We create a sort definition model to validate the provided definition string
			SortDefinition<T> sort = bsonSortDefinition;
			return _dataBase.GetCollection<T>(collectionName).Find(filter).Sort(sort).ToEnumerable();
		}

        /// <summary>
        /// Reads all matching items from the collection and sorts them based on the sort definition
        /// </summary>
        /// <typeparam name="T">The collection entity type</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="bsonSearchDefinition">The bson search definition.</param>
        /// <param name="bsonSortDefinition">The bson sort definition.</param>
        /// <param name="itemLimit">Item Limit</param>
        /// <returns>
        /// Collection of items that match the expression
        /// </returns>
        public IEnumerable<T> Read<T>(string collectionName, string bsonSearchDefinition, string bsonSortDefinition, int itemLimit) where T : EntityBase
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				LogAndThrow<ArgumentNullException>(nameof(collectionName));
			if (string.IsNullOrWhiteSpace(bsonSearchDefinition))
				LogAndThrow<ArgumentNullException>(nameof(bsonSearchDefinition));
			if (string.IsNullOrWhiteSpace(bsonSortDefinition))
				LogAndThrow<ArgumentNullException>(nameof(bsonSortDefinition));

			//We create a filter definition model to validate the provided definition string
			FilterDefinition<T> filter = bsonSearchDefinition;
			//We create a sort definition model to validate the provided definition string
			SortDefinition<T> sort = bsonSortDefinition;
			return _dataBase.GetCollection<T>(collectionName).Find(filter).Sort(sort).Limit(itemLimit).ToEnumerable();
		}

		/// <summary>
		/// Reads all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public IEnumerable<T> Read<T>(string collectionName, Func<T, bool> wherePredicate) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(wherePredicate, nameof(wherePredicate));

			return _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(wherePredicate);
		}

		/// <summary>
		/// Reads all matching items from the collection asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where predicate.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public async Task<IEnumerable<T>> ReadAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));

			return await _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(whereExpression).ToListAsync();
		}

		/// <summary>
		/// Reads all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <param name="selectPredicate">The select predicate.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public IEnumerable<S> Read<T, S>(string collectionName, Func<T, bool> wherePredicate, Func<T, S> selectPredicate) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(wherePredicate, nameof(wherePredicate));
			Guard(selectPredicate, nameof(selectPredicate));

			return _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(wherePredicate).Select(selectPredicate);
		}

		/// <summary>
		/// Reads all matching items from the collection asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <param name="selectExpression">The select expression.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public async Task<IEnumerable<S>> ReadAsync<T, S>(string collectionName, Expression<Func<T, bool>> whereExpression, Expression<Func<T, S>> selectExpression) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));
			Guard(selectExpression, nameof(selectExpression));

			IEnumerable<S> resultSet = null;
			await _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(whereExpression).ToListAsync().ContinueWith(i=>
			{
				resultSet = i.Result.Select( selectExpression.Compile() );
			});
			return resultSet;
		}


		/// <summary>
		/// Reads all matching items from the collection, collapsing a sub-collection into one
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <param name="selectPredicate">The select predicate.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public IEnumerable<S> ReadMany<T, S>(string collectionName, Func<T, bool> wherePredicate, Func<T, IEnumerable<S>> selectPredicate) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(wherePredicate, nameof(wherePredicate));
			Guard(selectPredicate, nameof(selectPredicate));

			return _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(wherePredicate).SelectMany(selectPredicate);
		}


		/// <summary>
		/// Reads all matching items from the collection, collapsing a sub-collection into one, asynchronously.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <param name="selectExpression">The select expression.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public async Task<IEnumerable<S>> ReadManyAsync<T, S>(string collectionName, Expression<Func<T, bool>> whereExpression, Expression<Func<T, IEnumerable<S>>> selectExpression) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));
			Guard(selectExpression, nameof(selectExpression));

			IEnumerable<S> resultSet = null;
			await _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(whereExpression).ToListAsync().ContinueWith(i=>
			{
				resultSet = i.Result.SelectMany( selectExpression.Compile() );
			});

			return resultSet;
		}

		/// <summary>
		/// Reads the first item from the collection, or returns a default item if no match can be found
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public T ReadFirstOrDefault<T>(string collectionName, Func<T, bool> wherePredicate) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(wherePredicate, nameof(wherePredicate));
            
            return _dataBase.GetCollection<T>(collectionName).AsQueryable().FirstOrDefault(wherePredicate);
		}

		/// <summary>
		/// Reads the first item from the collection asynchronously, or returns a default item if no match can be found
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns>
		/// Collection of items that match the expression
		/// </returns>
		public async Task<T> ReadFirstOrDefaultAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));

			return await _dataBase.GetCollection<T>(collectionName).AsQueryable().FirstOrDefaultAsync(whereExpression);
		}

		/// <summary>
		/// Reads the first item from the collection, or returns a default item if no match can be found
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <param name="selectPredicate">The select predicate.</param>
		/// <returns>
		/// Item that match the expression
		/// </returns>
		public S ReadFirstOrDefault<T, S>(string collectionName, Func<T, bool> wherePredicate, Func<T, S> selectPredicate) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(wherePredicate, nameof(wherePredicate));
			Guard(selectPredicate, nameof(selectPredicate));

			return _dataBase.GetCollection<T>(collectionName).AsQueryable().Where(wherePredicate).Select(selectPredicate).FirstOrDefault();
		}

		/// <summary>
		/// Reads the first item from the collection asynchronously, or returns a default item if no match can be found.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <param name="selectExpression">The select expression.</param>
		/// <returns>
		/// Item that match the expression
		/// </returns>
		public async Task<S> ReadFirstOrDefaultAsync<T, S>(string collectionName, Expression<Func<T, bool>> whereExpression, Expression<Func<T, S>> selectExpression) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(whereExpression, nameof(whereExpression));
			Guard(selectExpression, nameof(selectExpression));

			S resultSet = default;
			await _dataBase.GetCollection<T>( collectionName ).AsQueryable().Where( whereExpression ).FirstOrDefaultAsync().ContinueWith( i =>
			{
				resultSet = i.Result == null? default : selectExpression.Compile().Invoke( i.Result );
			} );
			return resultSet;
		}

		

		/// <summary>
		/// Inserts one item into collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		public void Insert<T>(string collectionName, T item) where T : EntityBase
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				LogAndThrow<ArgumentNullException>(nameof(collectionName));
			if (item == null)
				LogAndThrow<ArgumentNullException>(nameof(item));

			_dataBase.GetCollection<T>(collectionName).InsertOne(item);
		}

		/// <summary>
		/// Inserts one item into collection async.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public async Task InsertAsync<T>(string collectionName, T item) where T : EntityBase
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				LogAndThrow<ArgumentNullException>(nameof(collectionName));
			if (item == null)
				LogAndThrow<ArgumentNullException>(nameof(item));

			await _dataBase.GetCollection<T>(collectionName).InsertOneAsync(item);
		}

		/// <summary>
		/// Counts the items in a collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The Where clause</param>
		/// <returns></returns>
		public long CountItems<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase
		{
			var collection = _dataBase.GetCollection<T>(collectionName);
			return (int)collection.CountDocuments(whereExpression);
		}


		/// <summary>
		/// Counts the items in a collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where clause.</param>
		/// <returns></returns>
		public async Task<long> CountItemsAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase
        {
            var collection = _dataBase.GetCollection<T>(collectionName);
            return await collection.CountDocumentsAsync(whereExpression);
        }

		/// <summary>
		/// Upsert item
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool Write<T>(string collectionName, T item) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(item, nameof(item));

			//We're treating all Write operations as an Upsert, which requires us to use the ReplaceOne() method
			//In order to use that, we need to generate a filter so the ReplaceOne() use case when Updating can identify the item of interest.
			var filter = GetFilterDefinition(item );
			_dataBase.GetCollection<T>( collectionName ).ReplaceOne( filter, item, new ReplaceOptions { IsUpsert = true } );	
			
			return true;
		}

		/// <summary>
		/// Upsert item asynchronously.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public async Task<ReplaceOneResult> WriteAsync<T>(string collectionName, T item) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(item, nameof(item));

			//We're treating all Write operations as an Upsert, which requires us to use the ReplaceOne() method
			//In order to use that, we need to generate a filter so the ReplaceOne() use case when Updating can identify the item of interest.
			//var filter = GetFilterDefinition(item);
			//return await _dataBase.GetCollection<T>(collectionName).ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true });
			
            return await _dataBase.GetCollection<T>(collectionName).ReplaceOneAsync(x => x.Id.Equals(item.Id), item, new ReplaceOptions { IsUpsert = true });
        }

		/// <summary>
		/// Upsert multiple items
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="items">The items.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		public bool WriteMany<T>(string collectionName, IEnumerable<T> items) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(items, nameof(items));

			var replaceModels = items.Select(d => new ReplaceOneModel<T>(GetFilterDefinition(d), d) { IsUpsert = true });

			var result = _dataBase.GetCollection<T>(collectionName).BulkWrite(replaceModels);
			return result.IsAcknowledged;
		}

        /// <summary>
        /// Upsert item asynchronously.
        /// </summary>
        /// <typeparam name="T">The collection entity type</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="items"></param>
        /// <returns>
        ///   <c>True</c> if successful
        /// </returns>
        public async Task<BulkWriteResult<T>> WriteManyAsync<T>(string collectionName, IEnumerable<T> items) where T : EntityBase
		{
			Guard(collectionName, nameof(collectionName));
			Guard(items, nameof(items));

			var replaceModels = items.Select(d => new ReplaceOneModel<T>(GetFilterDefinition(d), d) { IsUpsert = true });

			return await _dataBase.GetCollection<T>(collectionName).BulkWriteAsync(replaceModels);
		}

        /// <summary>
        /// Gets the filter definition for Write operations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The generated filter definition</returns>
        private FilterDefinition<T> GetFilterDefinition<T>(T item) where T : EntityBase
		{
			var idElement = item.ToBsonDocument().Elements.FirstOrDefault(e => e.Name.Equals("_id"));
			if (idElement.Value == null || string.IsNullOrWhiteSpace(idElement.Value.ToString()))
				return new ObjectFilterDefinition<T>(item);
			else
				return Builders<T>.Filter.Eq("Id", ObjectId.Parse(idElement.Value.ToString()));
		}
	}
}
