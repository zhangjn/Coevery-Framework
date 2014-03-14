﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Coevery.Core.Projections.Descriptors.Layout;
using Coevery.Core.Projections.Models;

namespace Coevery.Core.Projections.ViewModels {

    public class LayoutEditViewModel {
        public LayoutEditViewModel() {
            Properties = new List<PropertyEntry>();
            Display = (int)LayoutRecord.Displays.Content;
            DisplayType = "Summary";
        }

        public int Id { get; set; }
        public int QueryId { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public LayoutDescriptor Layout { get; set; }
        public dynamic Form { get; set; }

        [Required]
        public int Display { get; set; }

        [Required, StringLength(64)]
        public string DisplayType { get; set; }

        public IEnumerable<PropertyEntry> Properties { get; set; }

        public IEnumerable<PropertyGroupEntry> Groups { get; set; }
    }
    
    public class PropertyEntry {
        public int PropertyRecordId { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string DisplayText { get; set; }
        public int Position { get; set; }
    }

    public class PropertyGroupEntry
    {
        public int LayoutGroupRecordId { get; set; }
        public int GroupPropertyId { get; set; }

        public string Sort { get; set; }
        public string DisplayText { get; set; }
        public int Position { get; set; }
    }
}
