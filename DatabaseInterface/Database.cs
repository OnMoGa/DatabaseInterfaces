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
		public abstract void delete();
		public abstract Table createTable(string name, List<TableColumn> columns);
		public abstract T getEntityById<T>(int id);
		public abstract List<T> getEntities<T>(int? top);


	}

}
