using System.ComponentModel.DataAnnotations.Schema;

namespace Coevery.ContentManagement.Records {
    [Table("Framework_ContentTypeRecord")]
    public class ContentTypeRecord {        
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
    
}
