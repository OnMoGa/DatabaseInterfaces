using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DatabaseInterface.MySQL {
	public class Server : DatabaseInterface.Server {

		public int port { get; set; }



		public Server(string hostname, string username, string password, int port = 3306) : base(hostname, username, password) {
			this.port = port;
		}

		~Server() {
			close();
		}

		public MySqlConnection connection;
		public override ConnectionState connectionState => connection?.State ?? ConnectionState.Closed;

		public override List<DatabaseInterface.Database> databases {
			get {
				string sql = "SHOW DATABASES;";
				MySqlCommand command = new MySqlCommand(sql, connection);
				MySqlDataReader reader = command.ExecuteReader();
				List<DatabaseInterface.Database> dbs = new List<DatabaseInterface.Database>();
				while(reader.Read()) {
					dbs.Add(new Database(this, reader.GetString(reader.GetName(0))));
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


		public override DatabaseInterface.Database createDB(string name) {
			string sql = $"CREATE DATABASE IF NOT EXISTS {name}";
			MySqlCommand command = new MySqlCommand(sql, connection);
			int result = command.ExecuteNonQuery();
			return new Database(this, name);
		}


		public static Dictionary<Type, string> typeEquivalents { get; } = new Dictionary<Type, string>() {
			{typeof(bool), "TINYINT(4)".ToLower()},
			{typeof(byte), "TINYINT(8)".ToLower()},
			{typeof(Enum), "TINYINT(8)".ToLower()},
			{typeof(Int16), "SMALLINT".ToLower()},
			{typeof(Int32), "INT(11)".ToLower()},
			{typeof(Int64), "BIGINT".ToLower()},
			{typeof(float), "FLOAT".ToLower()},
			{typeof(decimal), "FLOAT".ToLower()},
			{typeof(double), "DOUBLE".ToLower()},
			{typeof(char), "CHAR(1)".ToLower()},
			{typeof(string), "LONGTEXT".ToLower()},
			{typeof(DateTime), "DATETIME".ToLower()},
			{typeof(byte[]), "LONGBLOB".ToLower()},
		};

	}
}
