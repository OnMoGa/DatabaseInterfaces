using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Dapper;

namespace DatabaseInterface.SQLServer {
	class Database : DatabaseInterface.Database {

		public Database(Server server, string name) : base(server, name) { }
		public override List<DatabaseInterface.Table> tables { get; }

		public override void delete() {
			string sql = $"DROP DATABASE {name}";
			int result = ((Server)server).connection.Execute(sql);
		}

		public override DatabaseInterface.Table createTable(string name, List<DatabaseInterface.TableColumn> columns) {
			string sql = $"CREATE TABLE [{this.name}].[dbo].[{name}](";
			sql += "[id] [INT] IDENTITY(1,1) NOT NULL,";
			IMapper mapper = (new MapperConfiguration(cfg => cfg.CreateMap<DatabaseInterface.TableColumn, TableColumn>())).CreateMapper();

			sql += string.Join(", ", columns.Select(row => mapper.Map<TableColumn>(row).getCreateFormat()));
			sql += ")";
			int result = ((Server)server).connection.Execute(sql);

			return new Table(this, name, columns);
		}


		public override T getEntityById<T>(int id) {
			throw new NotImplementedException();
		}

		public override List<T> getEntities<T>(int? top) {
			throw new NotImplementedException();
		}
	}
}
