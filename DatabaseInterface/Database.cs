using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseInterface {
	public abstract class Database {

		public Server server { get; }
		public string name { get; }
		public abstract List<Table> tables { get; }

		public Database(Server server, string name) {
			this.server = server;
			this.name = name;
		}

		public abstract String generateConnectionString();
		public abstract bool testConnection();
		public abstract bool connect();

	}

}
