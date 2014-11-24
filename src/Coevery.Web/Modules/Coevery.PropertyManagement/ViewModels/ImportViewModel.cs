using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class ImportViewModel
    {
        public bool Success { get; set; }
        public List<ImportListModel> List { get; set; } 
    }
    public class ImportListModel
    {
        public int RowId { get; set; }
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
    }

}