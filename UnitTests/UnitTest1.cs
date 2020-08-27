using System.Collections.Generic;
using System.Linq;
using DatabaseInterface;
using DatabaseInterface.SQLServer;
using DatabaseInterface.MySQL;
using Xunit;

namespace UnitTests {
	public class UnitTest1 {

		[Fact]
		public void MSConnect() {
			Server server = new MSSQLServer("localhost", true);
			server.connect();
		}

		[Fact]
		public void MyConnect() {
			Server server = new MySQLServer("hostname","username", "password");
			server.connect();
		}

		[Fact]
		public void MSManageDBs() {
			Server server = new MSSQLServer("localhost", true);
			server.connect();
			Database db = server.createDB("test");
			Assert.Equal("test", db.name);

			List<Database> dbs = server.databases;
			Assert.Contains(dbs, db => db.name == "test");

			Assert.True(server.deleteDB(db));
		}

		[Fact]
		public void MyManageDBs() {
			Server server = new MySQLServer("192.168.0.175","root", "Mickyabc123");
			server.connect();
			Database db = server.createDB("test");
			Assert.Equal("test", db.name);
			
			List<Database> dbs = server.databases;
			Assert.Contains(dbs, db => db.name == "test");

			Assert.True(server.deleteDB(db));
		}



	}
}
