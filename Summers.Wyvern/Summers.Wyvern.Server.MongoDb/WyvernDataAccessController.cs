using System;
using System.Linq;

using Summers.Wyvern.Server.MongoDb.Database;
using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.Server.MongoDb
{
    public class WyvernDataAccessController : DataAccessController
    {
        protected override void SeedDatabase(Version schemaVersion, ref Schema liveVersion)
        {
            SeedUsers(ref liveVersion);
            base.SeedDatabase(schemaVersion, ref liveVersion);
        }

        private void SeedUsers(ref Schema liveVersion)
        {
            var user = Users.Read(x => x.FirstName.Equals("Danielle") && x.LastName.Equals("Summers")).FirstOrDefault();
            if (user == null)
            {
                Users.Create(new UserEntity
                {
                    FirstName = "Danielle",
                    LastName = "Summers",
                    Salutation = "Miss",
                    UserName = "Danielle",
                    UserRole = new RoleEntity{RoleName = "Owner"}
                });
            }
            else if (user.UserRole == null)
            {
                user.UserRole = new RoleEntity{RoleName = "Owner"};
                Users.Update(user);
            }
            user = Users.Read(x => x.FirstName.Equals("Anna") && x.LastName.Equals("Tierney")).FirstOrDefault();
            if (user == null)
            {
                Users.Create(new UserEntity
                {
                    FirstName = "Anna",
                    LastName = "Tierney",
                    Salutation = "Miss",
                    UserName = "Anna",
                    UserRole = new RoleEntity{RoleName = "PowerUser"}
                });
            }
            else if (user.UserRole == null)
            {
                user.UserRole = new RoleEntity{RoleName = "PowerUser"};
                Users.Update(user);
            }

            liveVersion.SeededComponents.Add(nameof(Users));
        }
    }
}