using Microsoft.AspNetCore.Mvc;

using Summers.Wyvern.Server.MongoDb.Database;

namespace Summers.Wyvern.DataAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class VenueReviewsListController : ControllerBase
	{
		private readonly IDataAccessController _dac;
		private readonly ILogger<HomePageController> _logger;

		public VenueReviewsListController(ILogger<HomePageController> logger, IDataAccessController dac)
		{
			_dac = dac;
			_logger = logger;
		}

		[HttpGet(Name = "Get")]
		public async Task<IEnumerable<ReviewPill>> Get(string OrderBy, int max, int index)
		{
			//TODO: Define the BSON Search parameter
			var bsonSearch = "";
			//TODO: Define the BSON Sort parameter (order by LastUpdateDate desc)
			var bsonOrder = "";

			var skipCount = index * max;

			return await Map.ToPill(_dac.VenueReviews.Read(bsonSearch, bsonOrder).Skip(skipCount).Take(max));
		}

		[HttpPost(Name = "PostSearchRequest")]
		public async Task Post(string query)
		{
			//TODO: redirect to search results page
			await Task.FromResult(true);
		}
	}
}
