﻿using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Fields.Settings;
using Coevery.Events;

namespace Coevery.Core.Fields.EntityRecords {
    public interface IEntityRecordEditorEvents : IEventHandler {
        IEnumerable<EntityRecordViewModel> FieldSettingsEditor();
        void FieldSettingsEditorUpdate(string fieldType, string fieldName, SettingsDictionary settings, IUpdateModel updateModel);
    }

    public abstract class EntityRecordEditorEventsBase : IEntityRecordEditorEvents {
        public virtual IEnumerable<EntityRecordViewModel> FieldSettingsEditor() {
            return Enumerable.Empty<EntityRecordViewModel>();
        }

        public virtual void FieldSettingsEditorUpdate(string fieldType, string fieldName, SettingsDictionary settings, IUpdateModel updateModel) { }
    }
}