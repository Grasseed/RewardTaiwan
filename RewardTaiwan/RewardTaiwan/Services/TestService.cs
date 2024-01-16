using RewardTaiwan.Services.Interface;

namespace RewardTaiwan.Services
{
	public class TestService : ITest
	{
		public string GetName(string id)
		{
			return $"{id}: Bill";
		}
	}
}
