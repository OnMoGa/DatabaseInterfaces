using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseInterface {
	public class TableColumn {
		public string columnName { get; set; }
		public Type dataType { get; set; }
		public int size { get; set; }
		public bool nullable { get; set; }

		public static string getFormatted(object value) {
			string formatted = "";
			if (value is byte[] bytes) {
				formatted = Tools.ByteArrayToString(bytes);

			} else if (value is bool b) {
				formatted = b ? "1":"0";

			} else if (value is int || value is float || value is double) {
				formatted = Convert.ToDouble(value).ToString();

			} else if (value is string){
				formatted = $"'{((string)value).Replace("'", "\\'")}'";

			} else if (value is char c) {
				formatted = $"'{c}'";

			} else if (value is DateTime dateTime) {
				formatted = $"'{dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}'";

			} else if (value is List<string> list) {
				formatted = $"'{string.Join(",", list.Select(v => v.Replace("'", "\\'")))}'";

			} else {
				throw new NotImplementedException($"There is no way implemented to save fields of type: {value.GetType().AssemblyQualifiedName}");
			}

			return formatted;
		}

		public string getCreateFormat() {
			throw new NotImplementedException($"Cant create column in db for type: {dataType.AssemblyQualifiedName}");
		}

	}
}
