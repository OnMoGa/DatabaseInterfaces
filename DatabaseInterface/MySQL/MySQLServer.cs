using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseInterface.MySQL {
	public class MySQLServer : Server {

		public int port { get; set; }

		public MySQLServer(string hostname, string username, string password, int port = 3306) : base(hostname, username, password) {
			this.port = port;
		}

		~MySQLServer() {
			close();
		}

		private MySqlConnection connection;


		public override List<Database> databases {
			get {
				string sql = "SHOW DATABASES;";
				MySqlCommand command = new MySqlCommand(sql, connection);
				MySqlDataReader reader = command.ExecuteReader();
				List<Database> dbs = new List<Database>();
				while(reader.Read()) {
					dbs.Add(new MySQLDB(this, reader.GetString(reader.GetName(0))));
				}
				reader.Close();
				return dbs;
			}
		}

		public override string generateConnectionString() {
			string connString = "";
			connString += $"datasource={hostname};";
			connString += $"port={port};";
			connString += $"username={username};";
			connString += $"password={password};";
			return connString;
		}

		public override void connect() {
			connection = new MySqlConnection(generateConnectionString());
			connection.Open();
		}

		public override void close() {
			connection.Close();
		}


		public override Database createDB(string name) {
			string sql = $"CREATE DATABASE IF NOT EXISTS {name}";
			MySqlCommand command = new MySqlCommand(sql, connection);
			int result = command.ExecuteNonQuery();
			return new MySQLDB(this, name);
		}

		public override bool deleteDB(Database database) {
			string sql = $"DROP DATABASE {database.name}";
			MySqlCommand command = new MySqlCommand(sql, connection);
			int result = command.ExecuteNonQuery();
			return true;
		}
	}
}
