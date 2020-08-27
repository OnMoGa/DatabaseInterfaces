using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using DatabaseInterface.MySQL;

namespace DatabaseInterface.SQLServer {
	public class Server : DatabaseInterface.Server {

		public bool integratedSecurity { get; set; } = false;

		public SqlConnection connection;

		public override ConnectionState connectionState => connection?.State ?? ConnectionState.Closed;

		public override List<DatabaseInterface.Database> databases {
			get {
				string sql = $"SELECT * FROM sys.databases";
				List<string> dbNames = connection.Query<string>(sql).ToList();
				List<DatabaseInterface.Database> dbs = new List<DatabaseInterface.Database>(dbNames.Select(s => new Database(this, s)));
				return dbs;
			}
		}


		public Server(string hostname, bool integratedSecurity) {
			this.hostname = hostname;
			this.integratedSecurity = integratedSecurity;
		}

		public Server(string hostname, string username, string password) : base(hostname, username, password) { }
		
		~Server() {
			close();
		}

		public override string generateConnectionString() {
			string connString = "";
			connString += $"Data Source={hostname};";

			if (integratedSecurity) {
				connString += $"Integrated Security=True;";
			} else {
				connString += $"User ID={hostname};";
				connString += $"Password={password};";
			}

			return connString;
		}

		public override void connect() {
			connection = new SqlConnection(generateConnectionString());
			connection.Open();
		}

		public override void close() {
			//connection.Close();
		}

		public override DatabaseInterface.Database createDB(string name) {
			if(connection == null) connect();
			string sql = $@"IF NOT EXISTS (
						        SELECT *
						        FROM sys.databases
						        WHERE name = '{name}'
					        )
						CREATE DATABASE {name}";
			int result = connection.Execute(sql);
			return new Database(this, name);
		}


		public static Dictionary<Type, string> typeEquivalents = new Dictionary<Type, string>() {
			{typeof(bool), "BIT".ToLower()},
			{typeof(byte), "TINYINT".ToLower()},
			{typeof(Enum), "TINYINT".ToLower()},
			{typeof(Int16), "SMALLINT".ToLower()},
			{typeof(Int32), "INT".ToLower()},
			{typeof(Int64), "BIGINT".ToLower()},
			{typeof(double), "FLOAT".ToLower()},
			{typeof(char), "CHAR".ToLower()},
			{typeof(string), "NTEXT".ToLower()},
			{typeof(DateTime), "DATETIME2".ToLower()},
			{typeof(TimeSpan), "TIME".ToLower()},
			{typeof(byte[]), "VARBINARY(MAX)".ToLower()},
			{typeof(Guid), "UNIQUEIDENTIFIER".ToLower()},
		};

		
	}
}
