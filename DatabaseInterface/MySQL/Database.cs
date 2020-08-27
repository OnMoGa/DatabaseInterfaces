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

		private class ColumnData {
			public string Field { get; set; }
			public string Type { get; set; }
			public string Null { get; set; }
		}

		public override List<DatabaseInterface.Table> tables {
			get {
				string sql = $"USE {name};SHOW TABLES;";
				List<string> tableNames = ((Server)server).connection.Query<string>(sql).ToList();
				List<DatabaseInterface.Table> tables = new List<DatabaseInterface.Table>();
				foreach (string tableName in tableNames) {
					sql = $"DESCRIBE {tableName}";
					List<ColumnData> columnData = ((Server)server).connection.Query<ColumnData>(sql).ToList();
					List<DatabaseInterface.TableColumn> columns = new List<DatabaseInterface.TableColumn>();

					foreach (ColumnData column in columnData) {
						columns.Add(new DatabaseInterface.TableColumn() {
							columnName = column.Field,
							dataType = Server.typeEquivalents.FirstOrDefault(e => column.Type.Contains(e.Value)).Key
									   ?? throw new NotImplementedException($"Couldn't map SQL Type: {column.Type} to type"),
							nullable = column.Null == "YES",
							size = 0
						});
					}

					tables.Add(new Table(this, tableName, columns));

				}
				return tables;
			}
		}

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
			string sql = $"select * from `{name}`.`{new T().tableName}` where id = {id};";
			T entity = ((Server)server).connection.Query<T>(sql).FirstOrDefault();
			return entity;
		}

		public override List<T> getEntities<T>(int? top) {
			string sql = $"select * from `{name}`.`{new T().tableName}`;";
			List<T> entities = ((Server)server).connection.Query<T>(sql).ToList();
			return entities;
		}
	}
}
