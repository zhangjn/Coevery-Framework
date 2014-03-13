﻿using System.Collections.Generic;
using System.Xml.Serialization;
using Coevery.ContentManagement.Records;
using Coevery.Data.Conventions;

namespace Coevery.DeveloperTools.Projections.Models {
    public class QueryPartRecord : ContentPartRecord {
        public QueryPartRecord() {
            FilterGroups = new List<FilterGroupRecord>();
            SortCriteria = new List<SortCriterionRecord>();
            Layouts = new List<LayoutRecord>();
        }

        [CascadeAllDeleteOrphan, Aggregate]
        [XmlArray("FilterGroupRecords")]
        public virtual IList<FilterGroupRecord> FilterGroups { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        [XmlArray("SortCriteria")]
        public virtual IList<SortCriterionRecord> SortCriteria { get; set; }

        [CascadeAllDeleteOrphan, Aggregate]
        [XmlArray("Layouts")]
        public virtual IList<LayoutRecord> Layouts { get; set; }

    }
}