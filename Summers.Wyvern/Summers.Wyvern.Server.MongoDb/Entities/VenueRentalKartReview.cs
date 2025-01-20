using System;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class VenueRentalKartReview
	{
		public Guid VenueId { get; set; }
		public UserEntity Author { get; set; }

		public string ReviewTitle { get; set; }
		public string DetailedReview { get; set; }

		public string KartPerformanceReview { get; set; }
		public int KartPerformanceScore { get; set; }

		public string KartEqualityReview { get; set; }
		public int KartEqualityScore { get; set; }

		public string KartComfortReview { get; set; }
		public int KartComfortScore { get; set; }

		//NOTE: If the shape of this entity changes, make sure you update the average score calculation!
		public double AverageScore { get { return (KartComfortScore + KartEqualityScore + KartPerformanceScore) / 3; } }
	}


}