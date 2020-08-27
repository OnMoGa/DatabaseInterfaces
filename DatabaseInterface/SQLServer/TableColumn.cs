using System;
using System.Collections.Generic;
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

			if (dataType == typeof(bool)) {
				formatted += "BIT ";
			} else if (dataType == typeof(byte) || dataType == typeof(Enum)) {
				formatted += "TINYINT ";
			} else if (dataType == typeof(Int16)) {
				formatted += "SMALLINT ";
			} else if (dataType == typeof(Int32)) {
				formatted += "INT ";
			} else if (dataType == typeof(Int64)) {
				formatted += "BIGINT ";
			} else if (dataType == typeof(decimal) || dataType == typeof(Single) || dataType == typeof(double)){
				formatted += "FLOAT(53) ";
			} else if (dataType == typeof(char)) {
				formatted += "CHAR ";
			} else if (dataType == typeof(string) || dataType == typeof(char[])) {
				formatted += "NVARCHAR(MAX) ";
			} else if (dataType == typeof(DateTime)) {
				formatted += "DATETIME2 ";
			} else if (dataType == typeof(TimeSpan)) {
				formatted += "TIME ";
			} else if (dataType == typeof(byte[])) {
				formatted += "VARBINARY(MAX) ";
			} else if (dataType == typeof(Guid)) {
				formatted += "UNIQUEIDENTIFIER ";
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