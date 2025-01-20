using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	internal interface IMongoConnectionManager : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this instance is connected to a MongoDB Service.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
		/// </value>
		bool IsConnected { get; }

		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="port">The port.</param>
		void Connect( string host, int port );

		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details.
		/// </summary>
		/// <param name="serverAddress">The server address.</param>
		/// <exception cref="InvalidOperationException">
		/// Cannot connect more than once
		/// or
		/// Failed to connect to database with specified address. 
		/// Possible causes could be the service isn't running on the target system, 
		/// or the standard access credentials aren't set up on the target system
		/// </exception>
		/// <exception cref="ArgumentNullException">serverAddress</exception>
		/// <exception cref="ArgumentException">Server Host must be valid</exception>
		void Connect( MongoServerAddress serverAddress );

		/// <summary>
		/// Gets the database list.
		/// </summary>
		/// <returns>A collection of database names in string format</returns>
		IEnumerable<string> GetDatabaseList();

		/// <summary>
		/// Gets the MongoDB database based on the provided database name.
		/// </summary>
		/// <param name="databaseName">Name of the database.</param>
		/// <returns><see cref="IMongoDatabase"/> instance </returns>
		/// <exception cref="InvalidOperationException">Cannot get database if not connected</exception>
		/// <exception cref="ArgumentException">database name must be valid</exception>
		IMongoDatabase GetDatabase(string databaseName);
	}
}
