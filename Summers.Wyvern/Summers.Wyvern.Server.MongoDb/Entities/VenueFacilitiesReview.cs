using System;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class VenueFacilitiesReview
	{
		public Guid VenueId { get; set; }
		public UserEntity Author { get; set; }

		public string ReviewTitle { get; set; }
		public string DetailedReview { get; set; }

		public string CleanlinessReview { get; set; }
		public int CleanlinessScore { get; set; }

		public string FoodReview { get; set; }
		public int FoodScore { get; set; }

		public string ComfortReview { get; set; }
		public int ComfortScore { get; set; }

		public string TemperatureReview { get; set; }
		public int TemperatureScore { get; set; }

		public string BathroomReview { get; set; }
		public int BathroomScore { get; set; }

		//NOTE: If the shape of this entity changes, make sure you update the average score calculation!
		public double AverageScore { get { return (CleanlinessScore + FoodScore + ComfortScore + TemperatureScore + BathroomScore) / 5; } }
	}
}