﻿using Coevery.ContentManagement.MetaData.Models;

namespace Coevery.DeveloperTools.ViewModels {

    public class EditFieldViewModel {
        public EditFieldViewModel() { }

        public EditFieldViewModel(ContentFieldDefinition contentFieldDefinition) {
            Name = contentFieldDefinition.Name;
            _Definition = contentFieldDefinition;
        }

        public string Name { get; set; }
        public ContentFieldDefinition _Definition { get; private set; }
    }
}