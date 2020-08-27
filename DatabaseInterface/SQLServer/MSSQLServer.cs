using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using DatabaseInterface.MySQL;

namespace DatabaseInterface.SQLServer {
	public class MSSQLServer : Server {

		public bool integratedSecurity { get; set; } = false;

		private SqlConnection connection;

		public override List<Database> databases {
			get {
				string sql = $"SELECT * FROM sys.databases";
				List<string> dbNames = connection.Query<string>(sql).ToList();
				List<Database> dbs = new List<Database>(dbNames.Select(s => new MSSQLServerDB(this, s)));
				return dbs;
			}
		}


		public MSSQLServer(string hostname, bool integratedSecurity) {
			this.hostname = hostname;
			this.integratedSecurity = integratedSecurity;
		}

		public MSSQLServer(string hostname, string username, string password) : base(hostname, username, password) { }
		
		~MSSQLServer() {
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


		public override Database createDB(string name) {
			string sql = $"CREATE DATABASE {name}";
			int result = connection.Execute(sql);
			return new MSSQLServerDB(this, name);
		}

		public override bool deleteDB(Database database) {
			string sql = $"DROP DATABASE {database.name}";
			int result = connection.Execute(sql);
			return true;
		}
	}
}
