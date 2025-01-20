using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Summers.Aelin.Server.Database.Entities
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public double[] VoicePrint { get; set; }
    }

}
