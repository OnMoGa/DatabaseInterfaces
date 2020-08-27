using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Dapper;
using MySql.Data.MySqlClient;

namespace DatabaseInterface.MySQL {
	class Database : DatabaseInterface.Database {

		public Database(DatabaseInterface.Server server, string name) : base(server, name) { }


		public override List<DatabaseInterface.Table> tables { get; }

		public override void delete() {
			string sql = $"DROP DATABASE `{name}`";
			MySqlCommand command = new MySqlCommand(sql, ((Server)server).connection);
			int result = command.ExecuteNonQuery();
		}

		public override DatabaseInterface.Table createTable(string name, List<DatabaseInterface.TableColumn> columns) {
			string sql = $"CREATE TABLE IF NOT EXISTS `{this.name}`.`{name}`(";
			sql += "`id` int(11) NOT NULL AUTO_INCREMENT,";
			IMapper mapper = (new MapperConfiguration(cfg => cfg.CreateMap<DatabaseInterface.TableColumn, TableColumn>())).CreateMapper();

			sql += string.Join(", ", columns.Select(row => mapper.Map<TableColumn>(row).getCreateFormat()));
			sql += ", PRIMARY KEY (`id`))";
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
