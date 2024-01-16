using Microsoft.AspNetCore.Mvc;
using RewardTaiwan.Services.Interface;

namespace RewardTaiwan.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly ITest _test;
		public TestController(ITest test)
		{
			_test = test;
		}
		[HttpGet("GetName")]
		public string Get(string id)
		{
			return _test.GetName(id);
		}
	}
}
