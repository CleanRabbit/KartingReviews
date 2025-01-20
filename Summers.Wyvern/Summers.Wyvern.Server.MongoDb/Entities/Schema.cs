using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson.Serialization.Attributes;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class Schema: EntityBase
	{
        public string SchemaVersion
		{
			get;set;
		}

		[BsonIgnore]
		[ExcludeFromCodeCoverage]
		public Version Version
		{
			get { return Version.Parse(SchemaVersion); }
			set { SchemaVersion = value.ToString(); }
		}

		public List<string> SeededComponents
		{
			get;set;
		}
	}
}
