using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public sealed class DepartmentListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
       public string Description { get; set; }
    }

    public class DepartmentFilterOptions
    {
        public string Search { get; set; }
        public string Contactor { get; set; }
    }
}