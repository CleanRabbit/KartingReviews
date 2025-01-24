using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.DataAPI
{
	public class Review
	{
		public string ReviewTitle { get; set; }
		public string ReviewCaption { get; set; }

		public string VenueName { get; set; }
		public double StaffAverageScore { get; set; }
		public double UserAverageScore { get; set; }
		public bool HasRentalRacing { get; set; }
		public bool HasOwnerDriverRacing { get; set; }
		public string MapsLocationURL { get; set; }
		public DateTime ReviewWrittenDate { get; set; }
		public DateTime LastUpdateDate { get; set; }
		public IEnumerable<string> ImageURLs { get; set; }

		public string StaffReviewerTag { get; set; }
		public string StaffReviewerId { get; set; }

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

		public string TrackSurfaceReview { get; set; }
		public int TrackSurfaceScore { get; set; }
		public string TrackComplexityReview { get; set; }
		public int TrackComplexityScore { get; set; }
		public string TrackOvertakePotentialReview { get; set; }
		public int TrackOvertakePotentialScore { get; set; }

		public string KartPerformanceReview { get; set; }
		public int KartPerformanceScore { get; set; }
		public string KartEqualityReview { get; set; }
		public int KartEqualityScore { get; set; }
		public string KartComfortReview { get; set; }
		public int KartComfortScore { get; set; }

		public int StaffScore { get; set; }
		public string StaffReview { get; set; }

		public double FacilitiesAverageScore { get { return (CleanlinessScore + FoodScore + ComfortScore + TemperatureScore + BathroomScore) / 5; } }
		public double TrackAverageScore { get { return (TrackComplexityScore + TrackOvertakePotentialScore + TrackSurfaceScore) / 3; } }
		public double RentalKartAverageScore { get { return (KartComfortScore + KartEqualityScore + KartPerformanceScore) / 3; } }
		public double FinalScore { get { return (FacilitiesAverageScore + TrackAverageScore + RentalKartAverageScore + StaffScore) /4; } }
	}
}