using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Coevery.Data.Conventions {
    public class RecordTableNameAlteration : IAutoMappingAlteration {
        public void Alter(AutoPersistenceModel model) {
            model.OverrideAll(mapping => {
                var recordType = mapping.GetType().GetGenericArguments().Single();
                var tableAttribute = Attribute.GetCustomAttribute(recordType, typeof (TableAttribute)) as TableAttribute;
                if (tableAttribute != null) {
                    var type = typeof (TableNameAlterationInternal<>).MakeGenericType(recordType);
                    var alteration = (IAlteration) Activator.CreateInstance(type);
                    alteration.Override(mapping);
                }
            });
        }

        private interface IAlteration {
            void Override(object mapping);
        }

        private class TableNameAlterationInternal<T> : IAlteration {
            public void Override(object mappingObj) {
                var mapping = (AutoMapping<T>) mappingObj;
                var tableAttribute = (TableAttribute) Attribute.GetCustomAttribute(typeof (T), typeof (TableAttribute));
                mapping.Table(tableAttribute.Name);
            }
        }
    }
}
