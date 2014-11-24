using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement.ViewModels
{
    public class EntitySelectorViewModel
    {
        public string Label { get; set; }
        public bool Required { get; set; }
        public string HelpText { get; set; }
        public string RelatedEntityName { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public IDictionary<string, object> GridOptions { get; set; }
        public object SelectedValue { get; set; }
        public string SelectedText { get; set; }
        public string DropdownUrl { get; set; }
        public string PopupWindowCaption { get; set; }
        public string DisplayFieldName { get; set; }
        public string RequiredMsg { get; set; }
        public string GridId { get; set; }
    }
}