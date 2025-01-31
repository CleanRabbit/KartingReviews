using Microsoft.AspNetCore.Mvc;

using Summers.Wyvern.Server.MongoDb.Database;

namespace Summers.Wyvern.DataAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HomePageController : ControllerBase
	{
		private readonly IDataAccessController _dac;
		private readonly ILogger<HomePageController> _logger;

		public HomePageController(ILogger<HomePageController> logger, IDataAccessController dac)
		{
			_dac = dac;
			_logger = logger;
		}

		[HttpGet(Name = "GetLetestPills")]
		public async Task<IEnumerable<ReviewPill>> Get()
		{
			//TODO: Define the BSON Search parameter
			var bsonSearch = "";
			//TODO: Define the BSON Sort parameter (order by LastUpdateDate desc)
			var bsonOrder = "";

			return await Map.ToPill(_dac.VenueReviews.Read(bsonSearch, bsonOrder).Take(3));
		}

		[HttpPost(Name = "PostSearchRequest")]
		public async Task Post(string query)
		{
			//TODO: redirect to search results page
			await Task.FromResult(true);
		}
	}
}
