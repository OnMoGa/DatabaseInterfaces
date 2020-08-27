using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseInterface.SQLServer {
	class TableColumn : DatabaseInterface.TableColumn {
		public new static string getFormatted(object value) {
			if (value is TimeSpan timeSpan) {
				return timeSpan.Ticks.ToString();
			} else {
				return DatabaseInterface.TableColumn.getFormatted(value);
			}
		}

		public new string getCreateFormat() {
			string formatted = $"[{columnName}] ";
			string equivalent = Server.typeEquivalents[dataType];
			if (equivalent != null) {
				formatted += $"{equivalent} ";
			} else {
				return base.getCreateFormat();
			}

			if (!nullable) {
				formatted += "NOT NULL";
			}

			return formatted;
		}
	}
}