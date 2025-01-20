using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	public interface IDatabase
	{
		/// <summary>
		/// Creates the collection.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <returns><c>True</c> if successful</returns>
		bool CreateCollection<T>(string collectionName) where T : EntityBase;

		/// <summary>
		/// Creates a capped collection.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="maxDocs">The maximum docs.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		bool CreateCappedCollection<T>( string collectionName, int maxDocs ) where T : EntityBase;

		/// <summary>
		/// Creates an index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="ascending">if set to <c>true</c> [ascending].</param>
		/// <returns></returns>
		Task CreateIndex<T>(string collectionName, string fieldName, bool ascending) where T : EntityBase;

		/// <summary>
		/// Drops a collection.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <returns><c>True</c> if successful</returns>
		bool DropCollection<T>(string collectionName) where T : EntityBase;

        /// <summary>
        /// Retrieves the full list of collections for the currently connected database
        /// </summary>
        /// <returns><c>IEnumerable</c> collection of document collections </returns>
        IEnumerable<string> CollectionList();

        /// <summary>
        /// Counts the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        long CountItems<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase;
		
        /// <summary>
        /// Counts the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="whereExpression">The where clause.</param>
        /// <returns></returns>
        Task<long> CountItemsAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase;

		/// <summary>
		/// Inserts the specified collection name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		void Insert<T>(string collectionName, T item) where T : EntityBase;

		/// <summary>
		/// Inserts the asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		Task InsertAsync<T>(string collectionName, T item) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection 
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="bsonSearchDefinition">The bson search definition.</param>
		/// <returns>
		/// Collection of items that match the search definition
		/// </returns>
		IEnumerable<T> Read<T>(string collectionName, string bsonSearchDefinition) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection and sorts them based on the sort definition
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="bsonSearchDefinition">The bson search definition.</param>
		/// <param name="bsonSortDefinition">The bson sort definition.</param>
		/// <returns>
		/// Collection of items that match the search definition
		/// </returns>
		IEnumerable<T> Read<T>(string collectionName, string bsonSearchDefinition, string bsonSortDefinition) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection and sorts them based on the sort definition returning items up to the specified item limit
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="bsonSearchDefinition">The bson search definition.</param>
		/// <param name="bsonSortDefinition">The bson sort definition.</param>
		/// <param name="itemLimit">The item limit.</param>
		/// <returns>
		/// Collection of items that match the search definition
		/// </returns>
		IEnumerable<T> Read<T>(string collectionName, string bsonSearchDefinition, string bsonSortDefinition, int itemLimit) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <returns>Collection of items that match the expression</returns>
		IEnumerable<T> Read<T>(string collectionName, Func<T, bool> wherePredicate) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns>Collection of items that match the expression</returns>
		Task<IEnumerable<T>> ReadAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <param name="selectPredicate">The select predicate.</param>
		/// <returns>Collection of items that match the expression</returns>
		IEnumerable<S> Read<T, S>(string collectionName, Func<T, bool> wherePredicate, Func<T, S> selectPredicate) where T : EntityBase;

		/// <summary>
		/// Reads all matching items from the collection asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <param name="selectExpression">The select expression.</param>
		/// <returns>Collection of items that match the expression</returns>
		Task<IEnumerable<S>> ReadAsync<T, S>(string collectionName, Expression<Func<T, bool>> whereExpression, Expression<Func<T, S>> selectExpression) where T : EntityBase;

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
		IEnumerable<S> ReadMany<T, S>(string collectionName, Func<T, bool> wherePredicate, Func<T, IEnumerable<S>> selectPredicate) where T : EntityBase;

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
		Task<IEnumerable<S>> ReadManyAsync<T, S>(string collectionName, Expression<Func<T, bool>> whereExpression, Expression<Func<T, IEnumerable<S>>> selectExpression) where T : EntityBase;

		/// <summary>
		/// Reads the first item from the collection, or returns a default item if no match can be found
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <returns>Collection of items that match the expression</returns>
		T ReadFirstOrDefault<T>(string collectionName, Func<T, bool> wherePredicate) where T : EntityBase;

		/// <summary>
		/// Reads the first item from the collection asynchronously, or returns a default item if no match can be found
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns>Collection of items that match the expression</returns>
		Task<T> ReadFirstOrDefaultAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase;

		/// <summary>
		/// Reads the first item from the collection, or returns a default item if no match can be found
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="wherePredicate">The where predicate.</param>
		/// <param name="selectPredicate">The select predicate.</param>
		/// <returns>Item that match the expression</returns>
		S ReadFirstOrDefault<T, S>(string collectionName, Func<T, bool> wherePredicate, Func<T, S> selectPredicate) where T : EntityBase;

		/// <summary>
		/// Reads the first item from the collection asynchronously, or returns a default item if no match can be found.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <typeparam name="S">The expected return type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <param name="selectExpression">The select expression.</param>
		/// <returns>Item that match the expression</returns>
		Task<S> ReadFirstOrDefaultAsync<T, S>(string collectionName, Expression<Func<T, bool>> whereExpression, Expression<Func<T, S>> selectExpression) where T : EntityBase;

		/// <summary>
		/// Upsert item
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>True</c> if successful</returns>
		bool Write<T>(string collectionName, T item) where T : EntityBase;

		/// <summary>
		/// Upsert item asynchronously.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		Task<ReplaceOneResult> WriteAsync<T>(string collectionName, T item) where T : EntityBase;

		/// <summary>
		/// Upsert multiple items
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="items">The items.</param>
		/// <returns><c>True</c> if successful</returns>
		bool WriteMany<T>(string collectionName, IEnumerable<T> items) where T : EntityBase;

		/// <summary>
		/// Upsert multiple items asynchronously.
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="items">The items.</param>
		Task<BulkWriteResult<T>> WriteManyAsync<T>(string collectionName, IEnumerable<T> items) where T : EntityBase;

		/// <summary>
		/// Deletes the specified item from the database
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>True</c> if successful</returns>
		bool Delete<T>(string collectionName, T item) where T : EntityBase;

		/// <summary>
		/// Deletes the specified item from the database asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		Task<DeleteResult> DeleteAsync<T>(string collectionName, T item) where T : EntityBase;

		/// <summary>
		/// Deletes all matching items from the collection
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns><c>True</c> if successful</returns>
		bool Delete<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase;

		/// <summary>
		/// Deletes all matching items from the collection asynchronously
		/// </summary>
		/// <typeparam name="T">The collection entity type</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="whereExpression">The where expression.</param>
		/// <returns>
		///   <c>True</c> if successful
		/// </returns>
		Task<DeleteResult> DeleteAsync<T>(string collectionName, Expression<Func<T, bool>> whereExpression) where T : EntityBase;
	}
}
