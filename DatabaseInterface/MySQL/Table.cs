using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace DatabaseInterface.MySQL {
	class Table : DatabaseInterface.Table {

		public Table(DatabaseInterface.Database database, string name, List<DatabaseInterface.TableColumn> columns) : base(database, name, columns) { }


		public override void delete() {
			string sql = $"DROP TABLE IF EXISTS `{database.name}`.`{name}`";
			MySqlCommand command = new MySqlCommand(sql, ((Server)database.server).connection);
			int result = command.ExecuteNonQuery();
		}

		public override int saveEntity<T>(int id, Dictionary<string, string> row) {
			throw new NotImplementedException();
		}


		public override string formatColumnValue(object value) {
			return TableColumn.getFormatted(value);
		}
	}
}
