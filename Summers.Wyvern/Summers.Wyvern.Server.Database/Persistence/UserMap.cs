using MongoDB.Bson.Serialization;
using Summers.Aelin.Server.Database.Entities;

namespace Summers.Aelin.Server.Database.Persistence
{
    public class UserMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.FirstName).SetIsRequired(true);
                map.MapMember(x => x.LastName);
                map.MapMember(x => x.UserName);
                map.MapMember(x => x.Salutation);
                map.MapMember(x => x.VoicePrint);
            });
        }
    }
}