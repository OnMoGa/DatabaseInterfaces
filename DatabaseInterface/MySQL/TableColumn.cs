using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseInterface.MySQL {
	class TableColumn : DatabaseInterface.TableColumn {
		public new static string getFormatted(object value) {
			return DatabaseInterface.TableColumn.getFormatted(value); 
		}
		public new string getCreateFormat() {
			
			string formatted = $"`{columnName}` ";
			string equivalent = Server.typeEquivalents[dataType] ?? base.getCreateFormat();

			formatted += $"{equivalent} ";

			if (!nullable) {
				formatted += "NOT NULL";
			}

			return formatted;
		}
	}
}
