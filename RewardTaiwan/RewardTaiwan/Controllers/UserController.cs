using Microsoft.AspNetCore.Mvc;
using RewardTaiwan.Services.Interface;
using RewardTaiwan.Filters;
using System.Web.Http.Filters;

namespace RewardTaiwan.Controllers
{
	public class UserController : AbstractController
	{
		private readonly ILogger<UserController> _logger;

		public UserController(ILogger<UserController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// 檢查站台是否正常運作
		/// </summary>
		/// <returns></returns>
		[HttpGet("")]
		public ActionResult IsRunning()
		{
			return Ok("RewardTaiwan is running");
		}

		/// <summary>
		/// 檢查是否登入
		/// </summary>
		/// <returns></returns>
		[HttpGet("IsLoggin")]
		[AuthorizationFilter]
		public ActionResult IsLoggin()
		{
			return Ok("success");
		}

		/// <summary>
		/// 登入
		/// </summary>
		/// <returns></returns>
		[HttpPost("Login")]
		public string Login()
		{
			_logger.LogInformation("login");
			var context = HttpContext.Response.HttpContext;
			var session = context.Session;
			var sessionId = Guid.NewGuid().ToString();
			session.SetString("sessionId", sessionId);
			return $"session id: {sessionId}";
		}

		/// <summary>
		/// 登出
		/// </summary>
		/// <returns></returns>
		[HttpPost("Logout")]
		public string Logout()
		{
			_logger.LogInformation("logout");
			// 清除 session
			HttpContext.Session.Clear();
			return "success";
		}
	}
}
