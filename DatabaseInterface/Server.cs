using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DatabaseInterface {
	public abstract class Server {
		public string hostname { get; set; }
		public string username { get; set; }
		public string password { get; set; }


		public abstract List<Database> databases { get; }


		public Server(string hostname, string username, string password) {
			this.hostname = hostname;
			this.username = username;
			this.password = password;
		}


		public Server() {}

		public abstract String generateConnectionString();
		public abstract void connect();
		public abstract void close();
		public abstract Database createDB(string name);
		public abstract bool deleteDB(Database database);


	}
}
