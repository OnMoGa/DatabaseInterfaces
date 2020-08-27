using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseInterface {
	public abstract class Table {
		public Database database { get; }
		public string name { get; }
		public List<DatabaseInterface.TableColumn> columns { get; set; }


		public Table(Database database, string name, List<DatabaseInterface.TableColumn> columns) {
			this.database = database;
			this.name = name;
			this.columns = columns;
		}


		public abstract void delete();
		public abstract int saveEntity<T>(int id, Dictionary<string, string> row) where T : Entity<T>, new();
		public abstract string formatColumnValue(object value);
	}
}
