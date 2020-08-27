using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AutoMapper;
using Dapper;

namespace DatabaseInterface.SQLServer {
	class Database : DatabaseInterface.Database {

		public Database(Server server, string name) : base(server, name) { }


		private class ColumnData {
			public string columnName { get; set; }
			public string typeName { get; set; }
			public int length { get; set; }
			public string nullable { get; set; }
		}
		public override List<DatabaseInterface.Table> tables {
			get {
				string sql = $"SELECT TABLE_NAME FROM {name}.INFORMATION_SCHEMA.TABLES;";
				List<string> tableNames = ((Server)server).connection.Query<string>(sql).ToList();
				List<DatabaseInterface.Table> tables = new List<DatabaseInterface.Table>();
				foreach (string tableName in tableNames) {
					sql = $@"
							use {name}
							declare @tableData as TABLE (
							    TABLE_QUALIFIER  sysname null,
							    TABLE_OWNER      sysname null,
							    TABLE_NAME       sysname null,
							    COLUMN_NAME      sysname null,
							    DATA_TYPE        sysname null,
							    TYPE_NAME        sysname null,
							    PRECISION        int null,
							    LENGTH           int null,
							    SCALE            int null,
							    RADIX            int null,
							    NULLABLE         bit null,
							    REMARKS          nvarchar(4000) Null,                                                                                                                                                                                                                                             
							    COLUMN_DEF       sysname null,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
							    SQL_DATA_TYPE    int null,
							    SQL_DATETIME_SUB int null,
							    CHAR_OCTET_LENGTH int null,
							    ORDINAL_POSITION  int null,
							    IS_NULLABLE       char(3) null,                                                                                                                                                                                                                                        
							    SS_DATA_TYPE      int null
							)
							Insert @tableData Exec sp_columns {tableName}
							Select 
								tb.COLUMN_NAME AS columnName,
								tb.TYPE_NAME AS typeName,
								tb.LENGTH AS length,
								tb.IS_NULLABLE AS nullable
							FROM @tableData tb
							";
					List<ColumnData> columnData = ((Server)server).connection.Query<ColumnData>(sql).ToList();
					List<DatabaseInterface.TableColumn> columns = new List<DatabaseInterface.TableColumn>();

					foreach (ColumnData column in columnData) {
						columns.Add(new DatabaseInterface.TableColumn() {
							columnName = column.columnName,
							dataType = Server.typeEquivalents.FirstOrDefault(e => e.Value == column.typeName.Split(' ')[0]).Key ?? throw new NotImplementedException($"Couldn't map SQL Type: {column.typeName} to type"),
							nullable = column.nullable == "YES",
							size = column.length
						});
					}

					tables.Add(new Table(this, tableName, columns));

				}
				return tables;
			}
		}

		public override void delete() {
			string sql = $@"USE master;
						 DROP DATABASE {name}";
			int result = ((Server)server).connection.Execute(sql);
		}

		public override DatabaseInterface.Table createTable(string name, List<DatabaseInterface.TableColumn> columns) {
			string sql = $@"USE {this.name}
if not exists (select * from sysobjects where name='{name}' and xtype='U')
BEGIN
CREATE TABLE [{this.name}].[dbo].[{name}](";

			sql += "[id] [INT] IDENTITY(1,1) NOT NULL,";
			IMapper mapper = (new MapperConfiguration(cfg => cfg.CreateMap<DatabaseInterface.TableColumn, TableColumn>())).CreateMapper();

			sql += string.Join(", ", columns.Select(row => mapper.Map<TableColumn>(row).getCreateFormat()));
			sql += ")END";
			int result = ((Server)server).connection.Execute(sql);

			return new Table(this, name, columns);
		}


		public override T getEntityById<T>(int id) {
			string sql = $"SELECT * FROM [{name}].[dbo].[{new T().tableName}] where id = {id}";
			T entity = ((Server)server).connection.Query<T>(sql).FirstOrDefault();
			return entity;
		}

		public override List<T> getEntities<T>(int? top = null) {
			string sql = $"SELECT * FROM [{name}].[dbo].[{new T().tableName}]";
			List<T> entities = ((Server)server).connection.Query<T>(sql).ToList();
			return entities;
		}
	}
}
