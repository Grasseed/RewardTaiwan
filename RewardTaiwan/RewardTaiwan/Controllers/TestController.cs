using Microsoft.AspNetCore.Mvc;
using RewardTaiwan.Services;
using RewardTaiwan.Services.Interface;

namespace RewardTaiwan.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly ITest _test;
		private readonly ILogger<TestController> _logger;
		private readonly IDapper _dapper;
		public TestController(ITest test, ILogger<TestController> logger, IDapper dapper)
		{
			_test = test;
			_logger = logger;
			_dapper = dapper;
		}

		[HttpGet("GetName")]
		public string Get(string id)
		{
			_logger.Log(LogLevel.Trace, $"Get {id}");
			return _test.GetName(id);
		}
	}
}
