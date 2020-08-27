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
			DatabaseInterface.Server server = new DatabaseInterface.MySQL.Server("hostname","username", "password");
			runTests(server);
		}

		public void runTests(DatabaseInterface.Server server) {
			connect(server);
			string dbName = "testDB";
			string tableName = "users";
			
			Database db = createDatabase(server, dbName);
			getDatabases(server, db);

			Table table = createTable(db, tableName);
			getTables(db, table);

			saveEntity(db);
			
			//cleanup
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
			Table table = db.createTable<TestData_User>();

			Assert.Equal(name, table.name);
			return table;
		}

		public void getTables(Database db, Table expectedTable) {
			List<Table> tables = db.tables;
			Assert.Contains(tables, t => t.name == expectedTable.name);
		}

		public void saveEntity(Database db) {
			TestData_User user = (new TestData_User() {
				name = "Michael Hamilton",
				address = "180 Commercial Rd",
				lastLogin = DateTime.Now,
				bankBalance = 0.01,
				sex = 'M',
				loginCount = 5
			}).saveToDB<TestData_User>(db);

			TestData_User user2 = (new TestData_User() {
				name = "Chloe Hernandez",
				address = "180 Commercial Rd",
				lastLogin = DateTime.Now,
				bankBalance = 0.01,
				sex = 'F',
				loginCount = 3
			}).saveToDB<TestData_User>(db);


			Assert.NotEqual(0, user.id);

			TestData_User theSameUser = db.getEntityById<TestData_User>(user.id);
			Assert.Equal(user.name, theSameUser.name);

		}

		public void getEntities(Database db) {
			List<TestData_User> users = db.getEntities<TestData_User>();
			Assert.True(users.Count > 0);
		}


		public void deleteTable(Table table) {
			table.delete();
		}


	}
}
