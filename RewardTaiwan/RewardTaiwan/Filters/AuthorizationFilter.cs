using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace RewardTaiwan.Filters
{
	public class AuthorizationFilter : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
			{
				context.Result = new UnauthorizedResult();
			}

			var sessionId = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			var session = context.HttpContext.Session.GetString("sessionId");
			if (string.IsNullOrEmpty(session))
			{
				context.Result = new UnauthorizedResult();
			}
			else if (session != sessionId)
			{
				context.Result = new UnauthorizedResult();
			}

		}
	}
}
