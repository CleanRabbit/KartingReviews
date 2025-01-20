using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Summers.Aelin.Server.Database.Persistence
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            UserMap.Configure();
            
            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("Aelin.Database.Conventions", pack, t => true);
        }
    }
}
