using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Bson;

using Summers.Wyvern.Common;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb.Database
{
    public abstract class DataRepositoryBase<TModel, TEntity> : Guarded, IRepository<TModel, TEntity>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataRepositoryBase{TModel, TEntity}"/> class.
		/// </summary>
		/// <param name="dataAccessLayer">The unit of work.</param>
		protected DataRepositoryBase( IDataAccessController dataAccessLayer )
		{
			DataAccess = dataAccessLayer ?? throw new ArgumentNullException(nameof(dataAccessLayer));
		}

		/// <summary>
		/// Gets the data context.
		/// </summary>
		/// <value>
		/// The data context.
		/// </value>
		protected IDatabase Context => DataAccess.Context;

		/// <summary>
		/// Gets the data access.
		/// </summary>
		/// <value>
		/// The data access.
		/// </value>
		protected IDataAccessController DataAccess
		{
			get;
		}

		/// <summary>
		/// Creates the model in the database.
		/// </summary>
		/// <param name="model">The model.</param>
		public abstract TModel Create(TModel model);

        /// <summary>
        /// Creates the model in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        public abstract Task<TModel> CreateAsync(TModel model);
        /// <summary>
        /// Reads the model from the database by object identifier.
        /// </summary>
        /// <param name="objectID">The object identifier.</param>
        /// <returns></returns>
        public abstract TModel Read(string objectID);
        /// <summary>
        /// Reads the model from the database by object identifier.
        /// </summary>
        /// <param name="objectID">The object identifier.</param>
        /// <returns></returns>
        public abstract Task<TModel> ReadAsync(string objectID);

        /// <summary>
        /// Updates the model in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        public abstract void Update(TModel model);

        /// <summary>
        /// Updates the model in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        public abstract Task UpdateAsync(TModel model);

        /// <summary>
        /// Deletes the model in the database.
        /// </summary>
        /// <param name="objectID">The object identifier.</param>
        public abstract void Delete(string objectID);

        /// <summary>
        /// Deletes the model in the database.
        /// </summary>
        /// <param name="objectID">The object identifier.</param>
        public abstract Task DeleteAsync(string objectID);

        /// <summary>
        /// Reads the model(s) from the database by a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public abstract IEnumerable<TModel> Read(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Reads the model(s) from the database by a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public abstract Task<IEnumerable<TModel>> ReadAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Reads the model(s) from the database based on the MongoDB Bson Search definition, 
        ///		ordering the results by the Bson Sort definition.
        /// </summary>
        /// <param name="bsonSearchDefinition">The bson search definition.</param>
        /// <param name="bsonSortDefinition">The bson sort definition.</param>
        /// <returns></returns>
        public abstract IEnumerable<TModel> Read(string bsonSearchDefinition, string bsonSortDefinition);

        /// <summary>
        /// Updates the collection of entities.
        /// </summary>
        /// <param name="modelCollection">The model collection.</param>
        public abstract void UpdateCollection(IEnumerable<TModel> modelCollection);

        /// <summary>
        /// Updates the collection of entities.
        /// </summary>
        /// <param name="modelCollection">The model collection.</param>
        public abstract Task UpdateCollectionAsync(IEnumerable<TModel> modelCollection);

		/// <summary>
		/// Converts the database entity to a model
		/// </summary>
		/// <param name="entity">The entity to be converted</param>
		/// <returns>The model that will be used in the app</returns>
		protected abstract TModel EntityToModel(TEntity entity);

		/// <summary>
		/// Converts the model to a database entity
		/// </summary>
		/// <param name="model">The model to be converted</param>
		/// <returns>The entity that can be saved to the database</returns>
		protected abstract TEntity ModelToEntity(TModel model);

		/// <summary>
		/// Requests the repository attempts to seed the database.
		///		
		/// NOTE: The seed order is critical where repositories depend on
		///		data created by others
		/// </summary>
		/// <param name="schemaVersion">The current executing schema version.</param>
		/// <param name="liveVersion">The version stored on the database.</param>
		[ExcludeFromCodeCoverage]
		protected virtual void SeedDatabase(Version schemaVersion, ref Schema liveVersion)
		{
		}

		/// <summary>
		/// Gets an ID, either from the string provided or by generating a new one
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>a valid ObjectID</returns>
		protected static string GetId(string id = "")
		{
			return string.IsNullOrWhiteSpace( id ) ? ObjectId.GenerateNewId().ToString() : id;
		}
	}
}