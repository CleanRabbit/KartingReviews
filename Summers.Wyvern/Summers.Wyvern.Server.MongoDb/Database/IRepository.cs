using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	/// <summary>
	/// The IRepository defines the CRUD methods for the Entity and Model.  Each model which is created in the 
	/// framework must have an Entity object which is mapped by the context.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	internal interface IRepository<TModel, TEntity>
	{
		/// <summary>
		/// Creates the model in the database.
		/// </summary>
		/// <param name="model">The model.</param>
		TModel Create(TModel model);
		
        /// <summary>
        /// Creates the model in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        Task<TModel> CreateAsync(TModel model);

		/// <summary>
		/// Reads the model from the database by object identifier.
		/// </summary>
		/// <param name="objectID">The object identifier.</param>
		/// <returns></returns>
		TModel Read(string objectID);

        /// <summary>
        /// Reads the model from the database by object identifier.
        /// </summary>
        /// <param name="objectID">The object identifier.</param>
        /// <returns></returns>
        Task<TModel> ReadAsync(string objectId);

		/// <summary>
		/// Reads the model(s) from the database by a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns></returns>
		IEnumerable<TModel> Read(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Reads the model(s) from the database by a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<IEnumerable<TModel>> ReadAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Updates the model in the database.
		/// </summary>
		/// <param name="model">The model.</param>
		void Update(TModel model);

        /// <summary>
        /// Updates the model in the database.
        /// </summary>
        /// <param name="model">The model.</param>
        Task UpdateAsync(TModel model);

		/// <summary>
		/// Deletes the model in the database.
		/// </summary>
		/// <param name="objectID">The object identifier.</param>
		void Delete(string objectID);
		
        /// <summary>
        /// Deletes the model in the database.
        /// </summary>
        /// <param name="objectID">The object identifier.</param>
        Task DeleteAsync(string objectID);
	}
}