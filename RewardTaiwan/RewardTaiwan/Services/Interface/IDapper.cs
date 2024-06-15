namespace RewardTaiwan.Services.Interface
{
	public interface IDapper
	{
        Task<IEnumerable<FinancialInfo>> GetAllBankNamesAsync();
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);
		IEnumerable<T> Query<T>(string sql, object param = null);
		int Execute(string sql, object param = null);
	}
}
