using System;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class VenueStaffReview
	{
		public Guid VenueId { get; set; }
		public UserEntity Author { get; set; }

		public string ReviewTitle { get; set; }
		public string DetailedReview { get; set; }

		public int StaffScore { get; set; }

		//NOTE: If the shape of this entity changes, make sure you update the average score calculation!
		public double AverageScore { get { return StaffScore; } }
	}


}