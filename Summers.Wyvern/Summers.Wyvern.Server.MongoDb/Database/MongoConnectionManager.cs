using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	internal class MongoConnectionManager : IMongoConnectionManager
	{
		private const string _adminDatabaseName = "admin";
		private const string _adminUserName = "GodMode";
		private const string _adminPassword = "BallisticGamesFTW";
		private IMongoClient _clientInstance;
		private Dictionary<string, IMongoDatabase> _databaseCollection = new Dictionary<string, IMongoDatabase>();

		/// <summary>
		/// Initializes a new instance of the <see cref="MongoConnectionManager"/> class.
		/// </summary>
		public MongoConnectionManager()
		{
		}

		#region IMongoConnectionManager Implementation

		/// <summary>
		/// Gets a value indicating whether this instance is connected to a MongoDB Service.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
		/// </value>
		public bool IsConnected
		{
			get; private set;
		}

		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="port">The port.</param>
		public void Connect(string host, int port)
		{
			Connect(new MongoServerAddress(host, port));
		}

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
		public void Connect(MongoServerAddress serverAddress)
		{
			//Defensive coding
			if (_clientInstance != null)
				throw new InvalidOperationException("Cannot connect more than once");
			if (serverAddress == null)
				throw new ArgumentNullException("serverAddress");
			if (string.IsNullOrWhiteSpace(serverAddress.Host))
				throw new ArgumentException("Server Host must be valid");

			//Create the master user using an unauthenticated mongo client (if needed)
			CreateMasterUserIfNotCreated(AttemptConnection(serverAddress, null));
			//Cache the client instance, as it's thread safe and contains it's own connection pool to improve performance
			_clientInstance = AttemptConnection(serverAddress, MongoCredential.CreateCredential(_adminDatabaseName, _adminUserName, _adminPassword));
			IsConnected = true;
		}

		/// <summary>
		/// Gets the database list.
		/// </summary>
		/// <returns>A collection of database names in string format</returns>
		public IEnumerable<string> GetDatabaseList()
		{
			return _clientInstance.ListDatabases().ToList().Select(i=>i["name"].ToString());
		}

		/// <summary>
		/// Gets the MongoDB database based on the provided database name.
		/// </summary>
		/// <param name="databaseName">Name of the database.</param>
		/// <returns><see cref="IMongoDatabase"/> instance </returns>
		/// <exception cref="InvalidOperationException">Cannot get database if not connected</exception>
		/// <exception cref="ArgumentException">database name must be valid</exception>
		public IMongoDatabase GetDatabase(string databaseName)
		{
			if (_clientInstance == null)
				throw new InvalidOperationException("Cannot get database if not connected");
			if (string.IsNullOrWhiteSpace(databaseName))
				throw new ArgumentException("database name must be valid");

			//We're caching the database instance, as it's thread safe and uses the client instance connection pool to improve performance
			if (_databaseCollection.ContainsKey(databaseName))
				return _databaseCollection[databaseName];

			var database = _clientInstance.GetDatabase(databaseName);
			_databaseCollection.Add(databaseName, database);
			return database;
		}
		#endregion


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			IsConnected = false;
		}

		/// <summary>
		/// Creates the master user if not created.
		/// </summary>
		/// <param name="serverAddress">The server address.</param>
		[ExcludeFromCodeCoverage]
		private void CreateMasterUserIfNotCreated(IMongoClient unauthenticatedClient)
		{
			var adminDb = unauthenticatedClient.GetDatabase(_adminDatabaseName);
			var response = adminDb.RunCommand<BsonDocument>(new BsonDocument { { "usersInfo", _adminUserName } });
			//scan the returned BsonDocument structure for our admin user
			if (!response.Values.Any(v=>v.ToString().Contains(_adminUserName)))	
			{
				//User doesn't exist, so create the user
				adminDb.RunCommand<BsonDocument>(new BsonDocument
				{
					{ "createUser", _adminUserName },
					{ "pwd", _adminPassword },
					{"roles", new BsonArray
						{
							new BsonDocument
							{
								/*
								 * We have to give ourselves global admin privileges 
								 *	because we cannot know what databases the consumer will create.
								 * We could create the necessary privileges when a database is created for the first time
								 *	but in order to do that, we need global admin privileges.
								 * We could limit ourselves to read/write any database, but we'd lose the ability to create databases.
								 */ 
								{"role", "dbAdminAnyDatabase" },
								{"db",_adminDatabaseName }
							}
						}
					}
				});
			}
		}

		/// <summary>
		/// Attempts to connect to a MongoDb service with the specified address and credentials.
		/// </summary>
		/// <param name="serverAddress">The server address.</param>
		/// <param name="credential">The credential.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Failed to connect to database with specified address. 
		/// Possible causes could be the service isn't running on the target system, 
		/// or the provided access credentials aren't configured up on the target system</exception>
		private IMongoClient AttemptConnection(MongoServerAddress serverAddress, MongoCredential credential)
		{
			try
			{
				var clientInstance = new MongoClient(new MongoClientSettings
				{
					Credential = credential,
					Server = serverAddress,
					ServerSelectionTimeout = new TimeSpan(0, 0, 10),
					ConnectTimeout = new TimeSpan(0, 0, 10),
					MaxConnectionPoolSize = 10000
				});

				//We attempt to query for a collection to ensure we have a valid connection.
				//The driver only authenticates us when we try to do something, not when we connect!
				//Stupid eh?
				var db = clientInstance.GetDatabase("local");
				db.GetCollection<BsonDocument>("StartupLog").AsQueryable().FirstOrDefault();
				return clientInstance;
			}
			catch(TimeoutException tEx)
			{
				throw new InvalidOperationException(@"Failed to connect to database with specified address. 
Possible causes could be the service isn't running on the target system, 
or the provided access credentials aren't configured on the target system", tEx);
			}
		}
	}
}
