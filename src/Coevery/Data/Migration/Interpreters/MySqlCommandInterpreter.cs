﻿using System;
using System.Data;
using System.Text;
using NHibernate.Dialect;
using Coevery.Data.Migration.Schema;
using Coevery.Environment.Configuration;
using Coevery.Localization;

namespace Coevery.Data.Migration.Interpreters {
    public class MySqlCommandInterpreter : ICommandInterpreter<AlterColumnCommand> {
        private readonly Lazy<Dialect> _dialectLazy;
        private readonly ShellSettings _shellSettings;

        public MySqlCommandInterpreter() {
            T = NullLocalizer.Instance;
        }

        public string DataProvider {
            get { return "MySql"; }
        }

        public Localizer T { get; set; }

        public MySqlCommandInterpreter(
            ShellSettings shellSettings,
            ISessionFactoryHolder sessionFactoryHolder) {
                _shellSettings = shellSettings;
                _dialectLazy = new Lazy<Dialect>(() => Dialect.GetDialect(sessionFactoryHolder.GetConfiguration().Properties));
        }

        public string[] CreateStatements(AlterColumnCommand command) {
            var builder = new StringBuilder();
            
            builder.AppendFormat("alter table {0} modify column {1} ",
                            _dialectLazy.Value.QuoteForTableName(PrefixTableName(command.TableName)),
                            _dialectLazy.Value.QuoteForColumnName(command.ColumnName));

            // type
            if (command.DbType != DbType.Object) {
                builder.Append(DefaultDataMigrationInterpreter.GetTypeName(_dialectLazy.Value, command.DbType, command.Length, command.Precision, command.Scale));
            }
            else {
                if (command.Length > 0 || command.Precision > 0 || command.Scale > 0) {
                    throw new CoeveryException(T("Error while executing data migration: you need to specify the field's type in order to change its properties"));
                }
            }

            // [default value]
            if (command.Default != null) {
                builder.Append(" set default ").Append(DefaultDataMigrationInterpreter.ConvertToSqlValue(command.Default)).Append(" ");
            }

            return new [] {
                builder.ToString()
            };
        }

        private string PrefixTableName(string tableName) {
            if (string.IsNullOrEmpty(_shellSettings.DataTablePrefix))
                return tableName;
            return _shellSettings.DataTablePrefix + "_" + tableName;
        }
    }
}
