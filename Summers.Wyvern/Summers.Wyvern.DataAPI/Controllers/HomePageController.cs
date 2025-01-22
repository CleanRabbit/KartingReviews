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
			var bsonSearch = "";
			var bsonOrder = "";
			return await Map.ToPill(_dac.VenueReviews.Read(bsonSearch, bsonOrder));
		}

		[HttpPost(Name = "PostSearchRequest")]
		public async Task Post(string query)
		{
			await Task.FromResult(true);
		}
	}
}
