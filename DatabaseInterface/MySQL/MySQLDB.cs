using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseInterface.MySQL {
	class MySQLDB : Database {

		public MySQLDB(Server server, string name) : base(server, name) { }

		public override List<Table> tables {
			get {
				return null;
			}
		}

		public override string generateConnectionString() {
			throw new NotImplementedException();
		}

		public override bool testConnection() {
			throw new NotImplementedException();
		}

		public override bool connect() {
			throw new NotImplementedException();
		}

	}
}
