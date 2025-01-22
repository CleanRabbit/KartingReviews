using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using Summers.Wyvern.Server.MongoDb.Entities;
using Summers.Wyvern.Server.MongoDb.Repositories;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	public interface IDataAccessController
	{
		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details.
		/// On successful connection, will call InitializeRepositories before checking
		/// the database schema is up to date
		/// </summary>
		/// <param name="hostName">Name of the MongoDB host.</param>
		/// <param name="portNumber">The MongoDB host port number.</param>
		/// <param name="databaseName">Name of the MongoDB database.</param>
		void Connect(string hostName, int portNumber, string databaseName);

		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details.
		/// On successful connection, will call InitializeRepositories before checking
		/// the database schema is up to date
		/// </summary>
		/// <param name="config">Instance of application IConfiguration.</param>
		void Connect(IConfiguration config);

		/// <summary>
		/// Gets the database list on host.
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <param name="portNumber">The port number.</param>
		/// <returns></returns>
		IEnumerable<string> GetDatabaseListOnHost(string hostName, int portNumber);

		UserAccounts Users { get; }
		VenueReviews VenueReviews { get; }

		/// <summary>
		/// Gets the data context which is mapped to the database.
		/// </summary>
		/// <value>
		/// The context.
		/// </value>
		IDatabase Context { get; }

		/// <summary>
		/// Gets the current database host
		/// </summary>
		/// <value>
		/// The current host name, else <c>null</c> if not connected.
		/// </value>
		string CurrentHost { get; }


		/// <summary>
		/// Gets the current database.
		/// </summary>
		/// <value>
		/// The current database.
		/// </value>
		string CurrentDatabase { get; }

    }
}
