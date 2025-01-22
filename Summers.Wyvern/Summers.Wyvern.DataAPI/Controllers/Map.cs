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
	}
}