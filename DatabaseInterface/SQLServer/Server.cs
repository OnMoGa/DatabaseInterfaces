using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using DatabaseInterface.MySQL;

namespace DatabaseInterface.SQLServer {
	public class Server : DatabaseInterface.Server {

		public bool integratedSecurity { get; set; } = false;

		public SqlConnection connection;

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
			connection.Close();
		}

		public override DatabaseInterface.Database createDB(string name) {
			string sql = $"CREATE DATABASE {name}";
			int result = connection.Execute(sql);
			return new Database(this, name);
		}

		
	}
}
