using System.Collections.Generic;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class UserEntity:EntityBase
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public bool IsStaff { get; set; }
        public RoleEntity UserRole { get; set; }
		public virtual IEnumerable<UserReview> Reviews { get; set; }
        public virtual IEnumerable<VenueReview> VenueReviews { get; set; }
	}
}
