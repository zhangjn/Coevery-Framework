﻿using System.ComponentModel.DataAnnotations;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Utilities;
using Coevery.Data.Conventions;

namespace Coevery.Core.Navigation.Models {
    public class MenuPart : ContentPart<MenuPartRecord> {

        private readonly LazyField<IContent> _menu = new LazyField<IContent>();
        public LazyField<IContent> MenuField { get { return _menu; } }

        public string Name
        {
            get { return Record.Name; }
            set { Record.Name=value; }
        }

        public IContent Menu {
            get { return _menu.Value; }
            set { _menu.Value = value; }
        }

        [StringLength(MenuPartRecord.DefaultMenuTextLength)]
        public string MenuText {
            get { return Record.MenuText; }
            set { Record.MenuText = value; }
        }

        public string MenuPosition {
            get { return Record.MenuPosition; }
            set { Record.MenuPosition = value; }
        }

        [StringLengthMax]
        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }
    }
}