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

			if (dataType == typeof(bool)) {
				formatted += "BIT(1) ";
			} else if (dataType == typeof(byte) || dataType == typeof(Enum)) {
				formatted += "TINYINT ";
			} else if (dataType == typeof(Int16)) {
				formatted += "SMALLINT ";
			} else if (dataType == typeof(Int32)) {
				formatted += "INT ";
			} else if (dataType == typeof(Int64) || dataType == typeof(TimeSpan)) {
				formatted += "BIGINT ";
			} else if (dataType == typeof(decimal) || dataType == typeof(Single)){
				formatted += "FLOAT ";
			} else if (dataType == typeof(double)){
				formatted += "DOUBLE ";
			} else if (dataType == typeof(char)) {
				formatted += "CHAR(1) ";
			} else if (dataType == typeof(string) || dataType == typeof(char[])) {
				formatted += "LONGTEXT ";
			} else if (dataType == typeof(DateTime)) {
				formatted += "DATETIME ";
			} else if (dataType == typeof(byte[])) {
				formatted += "LONGBLOB ";
			} else {
				return base.getCreateFormat();
			}

			if (!nullable) {
				formatted += "NOT NULL ";
			}

			return formatted;
		}
	}
}
