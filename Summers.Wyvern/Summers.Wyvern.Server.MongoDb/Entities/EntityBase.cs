using System;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
    public class EntityBase
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object Id { get; set; }

    }
}