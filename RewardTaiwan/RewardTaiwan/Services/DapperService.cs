using Dapper;
using Microsoft.Data.Sqlite;
using RewardTaiwan.Services.Interface;
using System.Data;

namespace RewardTaiwan.Services
{
	public class DapperService : IDapper
	{
		private readonly string _connectionString;

		public DapperService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public T QuerySingle<T>(string sql, object param = null)
		{
			using (IDbConnection db = new SqliteConnection(_connectionString)) // 或 SQLiteConnection
			{
				return db.Query<T>(sql, param).FirstOrDefault();
			}
		}

		public IEnumerable<T> Query<T>(string sql, object param = null)
		{
			using (IDbConnection db = new SqliteConnection(_connectionString)) // 或 SQLiteConnection
			{
				return db.Query<T>(sql, param);
			}
		}

		public int Execute(string sql, object param = null)
		{
			using (IDbConnection db = new SqliteConnection(_connectionString)) // 或 SQLiteConnection
			{
				return db.Execute(sql, param);
			}
		}
	}
}
