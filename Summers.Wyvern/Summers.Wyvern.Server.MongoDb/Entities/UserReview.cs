using System;
using System.Collections.Generic;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class UserReview
	{
		public Guid VenueId { get; set; }
		public double AverageScore { get; set; }

		public VenueFacilitiesReview VenueFacilitiesReview { get; set; }
		public VenueTrackReview VenueTrackReview { get; set; }
		public VenueRentalKartReview VenueRentalKartReview { get; set; }
		public VenueStaffReview VenueStaffReview { get; set; }

		public IEnumerable<string> ImageURLs { get; set; }

	}
}