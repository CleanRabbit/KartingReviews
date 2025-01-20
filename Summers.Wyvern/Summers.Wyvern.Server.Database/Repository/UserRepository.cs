using Summers.Aelin.Server.Database.Entities;
using Summers.Aelin.Server.Database.Interfaces;

namespace Summers.Aelin.Server.Database.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {
        }
    }
}
