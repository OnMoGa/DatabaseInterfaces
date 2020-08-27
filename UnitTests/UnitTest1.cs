using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseInterface;
using DatabaseInterface.SQLServer;
using DatabaseInterface.MySQL;
using Xunit;

namespace UnitTests {
	public class UnitTest1 {


		[Fact]
		public void MSRunTests() {
			DatabaseInterface.Server server = new DatabaseInterface.SQLServer.Server("localhost", true);
			runTests(server);
		}

		[Fact]
		public void MyRunTests() {
			DatabaseInterface.Server server = new MySQLServer("hostname","username", "password");
			runTests(server);
		}

		public void runTests(DatabaseInterface.Server server) {
			connect(server);
			string dbName = "testDB";
			string tableName = "users";
			
			Database db = createDatabase(server, dbName);
			getDatabases(server, db);

			Table table = createTable(db, tableName);


			//cleanup
			table.delete();
			deleteDatabase(db);
		}


		public void connect(DatabaseInterface.Server server) {
			server.connect();
		}

		public Database createDatabase(DatabaseInterface.Server server, string name) {
			Database db = server.createDB(name);
			Assert.Equal(name, db.name);
			return db;
		}

		public void getDatabases(DatabaseInterface.Server server, Database expectedDatabase) {
			List<Database> dbs = server.databases;
			Assert.Contains(dbs, db => db.name.ToLower() == expectedDatabase.name.ToLower());
		}

		public void deleteDatabase(Database database) {
			database.delete();
		}

		public Table createTable(Database db, string name) {
			Table table = db.createTable(name, new List<TableColumn> {
				new TableColumn(){ columnName = "name", dataType = typeof(string) },
				new TableColumn(){ columnName = "address", dataType = typeof(string) },
				new TableColumn(){ columnName = "lastLogin", dataType = typeof(DateTime) },
				new TableColumn(){ columnName = "bankBalance", dataType = typeof(double) },
				new TableColumn(){ columnName = "sex", dataType = typeof(char) },
				new TableColumn(){ columnName = "loginCount", dataType = typeof(int) }
			});
			Assert.Equal(name, table.name);
			return table;
		}



	}
}
