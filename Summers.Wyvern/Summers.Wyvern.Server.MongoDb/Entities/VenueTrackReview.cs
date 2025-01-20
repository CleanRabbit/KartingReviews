using System;

namespace Summers.Wyvern.Server.MongoDb.Entities
{
	public class VenueTrackReview
	{
		public Guid VenueId { get; set; }
		public UserEntity Author { get; set; }

		public string ReviewTitle { get; set; }
		public string DetailedReview { get; set; }

		public string TrackSurfaceReview { get; set; }
		public int TrackSurfaceScore { get; set; }

		public string TrackComplexityReview { get; set; }
		public int TrackComplexityScore { get; set; }

		public string TrackOvertakePotentialReview { get; set; }
		public int TrackOvertakePotentialScore { get; set; }

		//NOTE: If the shape of this entity changes, make sure you update the average score calculation!
		public double AverageScore { get { return (TrackComplexityScore + TrackOvertakePotentialScore + TrackSurfaceScore) / 3; } }
	}


}