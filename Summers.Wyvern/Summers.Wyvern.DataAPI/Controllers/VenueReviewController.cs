using Microsoft.AspNetCore.Mvc;

using Summers.Wyvern.Server.MongoDb.Database;

namespace Summers.Wyvern.DataAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class VenueReviewController : ControllerBase
	{
		private readonly IDataAccessController _dac;
		private readonly ILogger<HomePageController> _logger;

		public VenueReviewController(ILogger<HomePageController> logger, IDataAccessController dac)
		{
			_dac = dac;
			_logger = logger;
		}

		[HttpGet(Name = "Get")]
		public async Task<Review> Get(string id)
		{
			return await Map.ToReview(_dac.VenueReviews.Read(id));
		}
	}
}
