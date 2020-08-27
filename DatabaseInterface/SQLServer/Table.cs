using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace DatabaseInterface.SQLServer {
	class Table : DatabaseInterface.Table {
		
		public Table(DatabaseInterface.Database database, string name, List<DatabaseInterface.TableColumn> columns) : base(database, name, columns) { }


		public override void delete() {
			string sql = $"DROP TABLE [{database.name}].[dbo].[{name}]";
			int result = ((Server)database.server).connection.Execute(sql);
		}

		public override int saveEntity<T>(int id, Dictionary<string, string> row) {
			if (id == 0) {
				//New row
				string keys = String.Join(", ", row.Keys.ToArray());
				string values = String.Join(", ", row.Values.ToArray());
				string query = $"insert into {name} ({keys}) OUTPUT INSERTED.[Id] values ({values})";
				id = ((Server)database.server).connection.QuerySingle<int>(query);

			} else {
				//update row
				List<string> queryComponents = new List<string>();
				foreach (KeyValuePair<string, string> kvp in row) {
					string key = kvp.Key;
					string value = kvp.Value;
					queryComponents.Add($"{key} = {value}");
				}
				string query = String.Join(", ", queryComponents);
				string fullQuery = $"update {name} set {query} where id = {id}";
				int affectedRows = ((Server)database.server).connection.Execute(fullQuery);
			}

			return id;
		}

		public override string formatColumnValue(object value) {
			return TableColumn.getFormatted(value);
		}


	}
}
