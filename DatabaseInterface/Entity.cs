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


		public T saveToDB<ENTITYTYPE>(Database db) where ENTITYTYPE : Entity<T>, new() {
			return db.saveEntity<T>((T)this);
		}

	}

	public class DBColumn : Attribute {
		public int size { get; set; }
	}
}
