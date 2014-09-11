using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coevery.Data.Conventions {
    [AttributeUsageAttribute(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : Attribute {
        public string Name { get; set; }
        public TableAttribute(string name) {
            Name = name;
        }
    }
}
