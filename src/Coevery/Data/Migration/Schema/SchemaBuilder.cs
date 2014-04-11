﻿using System;
using Coevery.Data.Migration.Interpreters;

namespace Coevery.Data.Migration.Schema {
    public class SchemaBuilder {
        private readonly IDataMigrationInterpreter _interpreter;
        private readonly string _tablePrefix;
        private readonly Func<string, string> _formatPrefix;

        public SchemaBuilder(IDataMigrationInterpreter interpreter, string tablePrefix = null, Func<string, string> formatPrefix = null) {
            _interpreter = interpreter;
            _tablePrefix = tablePrefix ?? String.Empty;
            _formatPrefix = formatPrefix ?? (s => s ?? String.Empty);
        }

        public IDataMigrationInterpreter Interpreter {
            get { return _interpreter; }
        }
      
        public string TablePrefix {
            get { return _tablePrefix; }
        }
      
        public Func<string, string> FormatPrefix {
            get { return _formatPrefix; }
        }
      
        public SchemaBuilder CreateTable(string name, Action<CreateTableCommand> table) {
            var createTable = new CreateTableCommand(String.Concat(_formatPrefix(_tablePrefix), name));
            table(createTable);
            Run(createTable);
            return this;
        }

        public SchemaBuilder AlterTable(string name, Action<AlterTableCommand> table) {
            var alterTable = new AlterTableCommand(String.Concat(_formatPrefix(_tablePrefix), name));
            table(alterTable);
            Run(alterTable);
            return this;
        }

        public SchemaBuilder DropTable(string name) {
            var deleteTable = new DropTableCommand(String.Concat(_formatPrefix(_tablePrefix), name));
            Run(deleteTable);
            return this;
        }

        public SchemaBuilder ExecuteSql(string sql, Action<SqlStatementCommand> statement = null) {
            var sqlStatmentCommand = new SqlStatementCommand(sql);
            if ( statement != null ) {
                statement(sqlStatmentCommand);
            }
            Run(sqlStatmentCommand);
            return this;
        }

        private void Run(ISchemaBuilderCommand command) {
            _interpreter.Visit(command);
        }

        public SchemaBuilder CreateForeignKey(string name, string srcTable, string[] srcColumns, string destTable, string[] destColumns) {
            var command = new CreateForeignKeyCommand(name, String.Concat(_formatPrefix(_tablePrefix), srcTable), srcColumns, String.Concat(_formatPrefix(_tablePrefix), destTable), destColumns);
            Run(command);
            return this;
        }

        public SchemaBuilder CreateForeignKey(string name, string srcModule, string srcTable, string[] srcColumns, string destTable, string[] destColumns) {
            var command = new CreateForeignKeyCommand(name, String.Concat(_formatPrefix(srcModule), srcTable), srcColumns, String.Concat(_formatPrefix(_tablePrefix), destTable), destColumns);
            Run(command);
            return this;
        }

        public SchemaBuilder CreateForeignKey(string name, string srcTable, string[] srcColumns, string destModule, string destTable, string[] destColumns) {
            var command = new CreateForeignKeyCommand(name, String.Concat(_formatPrefix(_tablePrefix), srcTable), srcColumns, String.Concat(_formatPrefix(destModule), destTable), destColumns);
            Run(command);
            return this;
        }

        public SchemaBuilder CreateForeignKey(string name, string srcModule, string srcTable, string[] srcColumns, string destModule, string destTable, string[] destColumns) {
            var command = new CreateForeignKeyCommand(name, String.Concat(_formatPrefix(srcModule), srcTable), srcColumns, String.Concat(_formatPrefix(destModule), destTable), destColumns);
            Run(command);
            return this;
        }

        public SchemaBuilder DropForeignKey(string srcTable, string name) {
            var command = new DropForeignKeyCommand(String.Concat(_formatPrefix(_tablePrefix), srcTable), name);
            Run(command);
            return this;
        }

        public SchemaBuilder DropForeignKey(string srcModule, string srcTable, string name) {
            var command = new DropForeignKeyCommand(String.Concat(_formatPrefix(srcModule), srcTable), name);
            Run(command);
            return this;
        }

    }
}
