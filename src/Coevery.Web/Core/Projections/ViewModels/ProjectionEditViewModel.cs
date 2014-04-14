﻿using System.Collections.Generic;
using Coevery.Core.Projections.Descriptors.Layout;

namespace Coevery.Core.Projections.ViewModels {
    public class ProjectionEditViewModel {
        public ProjectionEditViewModel() {
            Fields = new List<PicklistItemViewModel>();
            State = new Dictionary<string, string>();
            PickedFields = new List<PropertyDescriptorViewModel>();
        }

        public int Id { get; set; }
        public string ItemContentType { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<PicklistItemViewModel> Fields { get; set; }
        public IEnumerable<PropertyDescriptorViewModel> PickedFields { get; set; }
        public bool IsDefault { get; set; }

        public int LayoutId { get; set; }
        public LayoutDescriptor Layout { get; set; }
        public dynamic Form { get; set; }
        public IDictionary<string, string> State { get; set; }
    }
}