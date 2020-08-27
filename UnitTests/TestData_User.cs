using System;
using System.Collections.Generic;
using System.Text;
using DatabaseInterface;

namespace UnitTests {
	class TestData_User : Entity<TestData_User> {
		public override string tableName {
			get => "users";
		}

		[DBColumn] public string name { get; set; }
		[DBColumn] public string address { get; set; }
		[DBColumn] public DateTime lastLogin { get; set; }
		[DBColumn] public double bankBalance { get; set; }
		[DBColumn] public char sex { get; set; }
		[DBColumn] public int loginCount { get; set; }



	}
}
