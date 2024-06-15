﻿using Dapper;
using Microsoft.Data.Sqlite;
using RewardTaiwan.Services.Interface;
using System.Data;
using System.Data.SqlClient;

namespace RewardTaiwan.Services
{
	public class DapperService : IDapper
	{
		private readonly string _connectionString;

		public DapperService(string connectionString)
		{
			_connectionString = connectionString;
		}

        // 通用查詢
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<T>(sql, param);
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



		//讀取資料庫的銀行名稱
        public async Task<IEnumerable<FinancialInfo>> GetAllBankNamesAsync()
        {
            string query = "SELECT bank_name_zh_tw AS BankNameZh, bank_name AS BankNameEn FROM Bank";
            return await QueryAsync<FinancialInfo>(query);
        }
    }
}
