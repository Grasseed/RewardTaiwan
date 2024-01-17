namespace RewardTaiwan.Services.Interface
{
	public interface IDapper
	{
		T QuerySingle<T>(string sql, object param = null);
		IEnumerable<T> Query<T>(string sql, object param = null);
		int Execute(string sql, object param = null);
	}
}
