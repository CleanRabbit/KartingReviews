using System;
using System.Collections.Generic;
using System.Linq;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class VenueReview : EntityBase
	{
		public string ReviewTitle { get; set; }
		public string ReviewCaption { get; set; }

		public string VenueName { get; set; }
		public double StaffAverageScore { get { return (VenueFacilitiesReview.AverageScore + VenueStaffReview.AverageScore + VenueRentalKartReview.AverageScore + VenueTrackReview.AverageScore) / 4; } }
		public double UserAverageScore { get { return UserReviews.Average(r => r.AverageScore); } }
		public bool HasRentalRacing { get; set; }
		public bool HasOwnerDriverRacing { get; set; }
		public string MapsLocationURL { get; set; }
		public DateTime ReviewWrittenDate { get; set; }
		public DateTime LastUpdateDate { get; set; }
		public IEnumerable<string> ImageURLs { get; set; }
		public UserEntity StaffReviewer { get; set; }

		public VenueFacilitiesReview VenueFacilitiesReview { get; set; }
		public VenueTrackReview VenueTrackReview { get; set; }
		public VenueRentalKartReview VenueRentalKartReview { get; set; }
		public VenueStaffReview VenueStaffReview { get; set; }

		public IEnumerable<UserReview> UserReviews { get; set; }
	}


}