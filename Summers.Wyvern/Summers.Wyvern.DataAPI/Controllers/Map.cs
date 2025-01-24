using Summers.Wyvern.Server.MongoDb.Entities;

namespace Summers.Wyvern.DataAPI.Controllers
{
	internal class Map
	{
		internal static async Task<IEnumerable<ReviewPill>> ToPill(IEnumerable<VenueReview> venues)
		{
			return await Task.WhenAll(venues.Select(x => Task.FromResult(new ReviewPill
			{
				PillAuthor = $"{x.StaffReviewer.Salutation} {x.StaffReviewer.FirstName} {x.StaffReviewer.LastName}",
				PillCaption = x.ReviewCaption,
				PillDisplayDate = x.LastUpdateDate,
				PillImageURL = x.ImageURLs.FirstOrDefault(),
				PillTitle = x.ReviewTitle
			})));
		}

		internal static async Task<Review> ToReview(VenueReview venueReview)
		{
			var reviewer = venueReview.StaffReviewer;
			var reviewerName = $"{reviewer.Salutation} {reviewer.FirstName} {reviewer.LastName}";

			var review = new Review()
			{
				ReviewTitle = venueReview.ReviewTitle,
				ReviewCaption = venueReview.ReviewCaption,
				VenueName = venueReview.VenueName,
				StaffAverageScore = venueReview.StaffAverageScore,
				UserAverageScore = venueReview.UserAverageScore,
				HasOwnerDriverRacing = venueReview.HasOwnerDriverRacing,
				HasRentalRacing = venueReview.HasRentalRacing,
				MapsLocationURL = venueReview.MapsLocationURL,
				ReviewWrittenDate = venueReview.ReviewWrittenDate,
				LastUpdateDate = venueReview.LastUpdateDate,
				BathroomReview = venueReview.VenueFacilitiesReview.BathroomReview,
				BathroomScore = venueReview.VenueFacilitiesReview.BathroomScore,
				CleanlinessReview = venueReview.VenueFacilitiesReview.CleanlinessReview,
				CleanlinessScore = venueReview.VenueFacilitiesReview.CleanlinessScore,
				ComfortReview = venueReview.VenueFacilitiesReview.ComfortReview,
				ComfortScore = venueReview.VenueFacilitiesReview.ComfortScore,
				FoodReview = venueReview.VenueFacilitiesReview.FoodReview,
				FoodScore = venueReview.VenueFacilitiesReview.FoodScore,
				KartComfortReview = venueReview.VenueRentalKartReview.KartComfortReview,
				KartComfortScore = venueReview.VenueRentalKartReview.KartComfortScore,
				KartEqualityReview = venueReview.VenueRentalKartReview.KartEqualityReview,
				KartEqualityScore = venueReview.VenueRentalKartReview.KartEqualityScore,
				KartPerformanceReview = venueReview.VenueRentalKartReview.KartPerformanceReview,
				KartPerformanceScore = venueReview.VenueRentalKartReview.KartPerformanceScore,
				ImageURLs = venueReview.ImageURLs,
				StaffReview = venueReview.VenueStaffReview.StaffReview,
				StaffReviewerId = venueReview.StaffReviewer.Id.ToString(),
				StaffReviewerTag = reviewerName,
				StaffScore = venueReview.VenueStaffReview.StaffScore,
				TemperatureReview = venueReview.VenueFacilitiesReview.TemperatureReview,
				TemperatureScore = venueReview.VenueFacilitiesReview.TemperatureScore,
				TrackComplexityReview = venueReview.VenueTrackReview.TrackComplexityReview,
				TrackComplexityScore = venueReview.VenueTrackReview.TrackComplexityScore,
				TrackOvertakePotentialReview = venueReview.VenueTrackReview.TrackOvertakePotentialReview,
				TrackOvertakePotentialScore = venueReview.VenueTrackReview.TrackOvertakePotentialScore,
				TrackSurfaceReview = venueReview.VenueTrackReview.TrackSurfaceReview,
				TrackSurfaceScore = venueReview.VenueTrackReview.TrackSurfaceScore
			};
			return await Task.FromResult(review);
		}
	}
}