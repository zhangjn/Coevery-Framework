using System.ComponentModel.DataAnnotations.Schema;

namespace Coevery.Data.Migration.Records {
    [Table("Framework_DataMigrationRecord")]
    public class DataMigrationRecord {
        public virtual int Id { get; set; }
        public virtual string DataMigrationClass { get; set; }
        public virtual int? Version { get; set; }
    }
}