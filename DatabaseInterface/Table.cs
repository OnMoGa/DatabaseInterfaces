using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseInterface {
	public abstract class Table {
		public string name { get; set; }
		public Dictionary<string, List<object>> data { get; set; }
	}
}
