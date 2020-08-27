using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DatabaseInterface {
	public abstract class Entity<T> where T: Entity<T>, new() {

		public abstract string tableName { get; }

		public int id { get; set; }

		public static T getById(int id, Database db) {
			return db.getEntityById<T>(id);
		}


		public static List<T> get(Database db, int? top = null) {
			return db.getEntities<T>(top);
		}


		public T saveToDB(Table table) {
			Dictionary<string, string> row = new Dictionary<string, string>();

			foreach (PropertyInfo property in typeof(T).GetProperties()) {

				object value = property.GetValue(this);
				//If the field has the DBColumn attribute then...
				if (Attribute.IsDefined(property, typeof(DBColumn))) {
					if(value == null) continue;
					row[property.Name] = table.formatColumnValue(value);
				}
			}

			if(row.Count > 0) {
				id = table.saveEntity<T>(id, row);
			} else {
				throw new Exception($"{typeof(T).AssemblyQualifiedName} has no DBColumn fields");
			}

			return (T)this;
		}

	}

	public class DBColumn : Attribute {}
}
