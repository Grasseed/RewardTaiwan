using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using RewardTaiwan.Filters;
using RewardTaiwan.Models;
using System.Text;

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

		/// <summary>
		/// Google登入
		/// </summary>
		/// <param name="idToken"></param>
		/// <returns></returns>
		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin(string idToken)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
				// 驗證成功，返回使用者資訊
				return Ok(new
				{
					Email = payload.Email,
					Name = payload.Name,
					Picture = payload.Picture
				});
			}
			catch (InvalidJwtException)
			{
				// 驗證失敗
				return BadRequest("Google 登入驗證失敗");
			}
		}

		/// <summary>
		/// 驗證 Google 登入授權
		/// </summary>
		/// <returns></returns>
		[HttpPost("ValidGoogleLogin")]
		public IActionResult ValidGoogleLogin()
		{
			string formCredential = Request.Form["credential"]; // 回傳憑證
			string formToken = Request.Form["g_csrf_token"]; // 回傳令牌
			string cookiesToken = Request.Cookies["g_csrf_token"]; // Cookie 令牌

			// 驗證 Google Token
			GoogleJsonWebSignature.Payload payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
			if (payload == null)
			{
				// 驗證失敗
				return BadRequest("驗證 Google 授權失敗");
			}
			else
			{
				// 驗證成功，重定向回來源頁面
				return Redirect(Configs.SitePages.HomePage);
			}
		}

		/// <summary>
		/// 驗證 Google Token
		/// </summary>
		/// <param name="formCredential"></param>
		/// <param name="formToken"></param>
		/// <param name="cookiesToken"></param>
		/// <returns></returns>
		public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
		{
			// 檢查空值
			if (formCredential == null || formToken == null && cookiesToken == null)
			{
				return null;
			}

			GoogleJsonWebSignature.Payload? payload;
			try
			{
				// 驗證 token
				if (formToken != cookiesToken)
				{
					return null;
				}

				// 驗證憑證
				IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
				string GoogleApiClientId = Configs.UseGoogleAuthentication.ClientId;
				var settings = new GoogleJsonWebSignature.ValidationSettings()
				{
					Audience = new List<string>() { GoogleApiClientId }
				};
				payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
				if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
				{
					return null;
				}
				if (payload.ExpirationTimeSeconds == null)
				{
					return null;
				}
				else
				{
					DateTime now = DateTime.Now.ToUniversalTime();
					DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
					if (now > expiration)
					{
						return null;
					}
				}
			}
			catch
			{
				return null;
			}
			return payload;
		}

		/// <summary>
		/// 將參數後方自動補上&
		/// </summary>
		/// <param name="url"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string AddParamsToUrl(string url, List<Parameter> parameters)
		{
			StringBuilder urlBuilder = new StringBuilder(url);
			if (parameters.Count > 0)
			{
				urlBuilder.Append("?");
				foreach (var param in parameters)
				{
					urlBuilder.Append($"{param.Key}={param.Value}&");
				}
				// 移除最後一個 "&"
				urlBuilder.Remove(urlBuilder.Length - 1, 1);
			}
			return urlBuilder.ToString();
		}
	}
}
