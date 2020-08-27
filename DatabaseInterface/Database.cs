using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

		public Table createTable<T>() where T : Entity<T>, new() {
			List<TableColumn> columns = new List<TableColumn>();
			foreach (PropertyInfo property in typeof(T).GetProperties()) {
				if (Attribute.IsDefined(property, typeof(DBColumn))) {
					columns.Add(new TableColumn() {
						columnName = property.Name,
						dataType = property.PropertyType
					});
				}
			}

			if (columns.Count == 0) {
				throw new Exception($"{typeof(T).AssemblyQualifiedName} has no DBColumn fields");
			}

			return createTable(new T().tableName, columns);
		}

		public abstract T getEntityById<T>(int id) where T : Entity<T>, new();
		public abstract List<T> getEntities<T>(int? top = null) where T : Entity<T>, new() ;

		public T saveEntity<T>(T entity) where T : Entity<T>, new() {
			Dictionary<string, string> row = new Dictionary<string, string>();
			Table table = tables.FirstOrDefault(t => t.name == entity.tableName) ?? createTable<T>();

			foreach (PropertyInfo property in typeof(T).GetProperties()) {

				object value = property.GetValue(entity);
				//If the field has the DBColumn attribute then...
				if (Attribute.IsDefined(property, typeof(DBColumn))) {
					if(value == null) continue;
					row[property.Name] = table.formatColumnValue(value);
				}
			}

			if(row.Count > 0) {
				entity.id = table.saveEntity<T>(entity.id, row);
			} else {
				throw new Exception($"{typeof(T).AssemblyQualifiedName} has no DBColumn fields");
			}

			return entity;
		}


	}

}
