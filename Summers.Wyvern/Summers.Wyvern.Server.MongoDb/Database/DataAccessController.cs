using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Extensions.Configuration;

using MongoDB.Bson;

using Summers.Wyvern.Server.MongoDb.Entities;
using Summers.Wyvern.Server.MongoDb.Repositories;
using Summers.Wyvern.SpeechManager;

namespace Summers.Wyvern.Server.MongoDb.Database
{
	public class DataAccessController : IDataAccessController
	{
		public static string GenerateNewObjectId()
		{
			return ObjectId.GenerateNewId().ToString();
		}

		protected static readonly Version _schemaVersion = new Version(1, 0, 0, 0);

		/// <summary>
		/// The internal Mongo Connection Manager
		/// </summary>
		private MongoConnectionManager _mcm;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataAccessController"/> class.
		/// </summary>
		public DataAccessController()
		{
		}

		/// <summary>
		/// Gets the database list.
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <param name="portNumber">The port number.</param>
		/// <returns>A collection of database names in string format</returns>
		public virtual IEnumerable<string> GetDatabaseListOnHost(string hostName, int portNumber)
		{
			var mcm = new MongoConnectionManager();
			mcm.Connect(hostName, portNumber);
			return mcm.GetDatabaseList();
		}

		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details.
		/// On successful connection, will call InitializeRepositories before checking
		/// the database schema is up to date
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <param name="portNumber">The port number.</param>
		/// <param name="databaseName">Name of the database.</param>
		[ExcludeFromCodeCoverage]
		public void Connect(string hostName, int portNumber, string databaseName)
		{
			_mcm = new MongoConnectionManager();
			_mcm.Connect(hostName, portNumber);
			Context = new MongoDatabase(_mcm, databaseName);
			CurrentHost = hostName;
			CurrentDatabase = databaseName;

			//Initialize data repositories before continuing
			InitializeRepositories();

			//Check database version to see if it needs seeding
			CheckDatabaseSchema();
		}

		/// <summary>
		/// Attempts to connect to a MongoDB service based on the provided connection details.
		/// On successful connection, will call InitializeRepositories before checking
		/// the database schema is up to date
		/// </summary>
		/// <param name="config">Instance of application IConfiguration.</param>
		[ExcludeFromCodeCoverage]
		public void Connect(IConfiguration config)
		{
			this.Connect(config[Identifiers.MongoDbHostConfigId],
				int.Parse(config[Identifiers.MongoDbPortConfigId]),
				config[Identifiers.MongoDbDatabaseNameConfigId]);
		}

		/// <summary>
		/// Switches the database.
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <param name="portNumber">The port number.</param>
		/// <param name="databaseName">Name of the database.</param>
		public void SwitchDatabase(string hostName, int portNumber, string databaseName)
		{
			if (_mcm != null)
			{
				Context = new MongoDatabase(_mcm, databaseName);
				CurrentDatabase = databaseName;
			}
			else
			{
				Connect(hostName, portNumber, databaseName);
			}
		}

		/// <summary>
		/// Initializes the repositories.
		/// </summary>
		protected void InitializeRepositories()
		{
			Users = new UserAccounts(this);
			Intents = new Intents(this);
		}

		public UserAccounts Users { get; private set; }
        public Intents Intents { get; private set; }

		/// <summary>
		/// Gets the data context.
		/// </summary>
		/// <value>
		/// The data context.
		/// </value>
		public IDatabase Context { get; protected set; }

		/// <summary>
		/// Gets the current database host
		/// </summary>
		/// <value>
		/// The current host name, else <c>null</c> if not connected.
		/// </value>
		public string CurrentHost { get; protected set; }

		/// <summary>
		/// Gets the current database.
		/// </summary>
		/// <value>
		/// The current database.
		/// </value>
		public string CurrentDatabase { get; protected set; }

		/// <summary>
		/// Checks the database schema and seeds the database if needed.
		/// </summary>
		[ExcludeFromCodeCoverage]
		private void CheckDatabaseSchema()
        {
            if (!Context.CollectionList().Contains("Schema"))
                Context.CreateCollection<Schema>("Schema");

			var liveVersion = Context.ReadFirstOrDefault<Schema>("Schema", s => true);
			if (liveVersion == null)
				liveVersion = new Schema();
			if (liveVersion.SeededComponents == null)
				liveVersion.SeededComponents = new List<string>();
			if (liveVersion.Id == null)
				liveVersion.Id = ObjectId.GenerateNewId();
			//always allow anyone who derives from this instance to determine their own database seeding requirements
			SeedDatabase(_schemaVersion, ref liveVersion);
		}

		/// <summary>
		/// Requests each repository attempts to seed the database.
		/// This method is called automatically after successfully connecting
		///		to the database when the database schema is out of date.
		///		
		/// NOTE: The seed order is critical where repositories depend on
		///		data created by others
		/// NOTE: Entities that override this method should always call the
		///		base method AFTER all other seeding has taken place
		/// </summary>
		/// <param name="schemaVersion">The current executing schema version.</param>
		/// <param name="liveVersion">The version stored on the database.</param>
		protected virtual void SeedDatabase(Version schemaVersion, ref Schema liveVersion)
        {
            //Grab the schema details from the database again for comparisons:
			var tmp = Context.ReadFirstOrDefault<Schema>("Schema", s => true);
			if (tmp == null || tmp.SeededComponents == null || !tmp.SeededComponents.Any())
			{
				//There was no schema to begin with: write it out
				liveVersion.Version = schemaVersion;
				Context.Write("Schema", liveVersion);
				return;
			}
			var newSeededComponentsList = liveVersion.SeededComponents;
			if (liveVersion.Version.CompareTo(schemaVersion) < 0 || !newSeededComponentsList.All(c => tmp.SeededComponents.Contains(c)))
			{
				//database was just updated or seeded: write it out
				liveVersion.Version = schemaVersion;
				Context.Write("Schema", liveVersion);
			}
		}
    }
}
