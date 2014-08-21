using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coevery;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.Records;
using Coevery.Core.Relationships.Settings;
using Coevery.Data;
using Coevery.Environment.ShellBuilders.Models;
using Coevery.Localization;
using Coevery.UI.Notify;

namespace Coevery.Core.Common.Handlers {
    public class ContentDeletingHandler : ContentHandlerBase {
        private readonly IContentDefinitionQuery _contentDefinitionQuery;
        private readonly ICoeveryServices _coeveryServices;
        private readonly ShellBlueprint _shellBlueprint;

        public ContentDeletingHandler(
            IContentDefinitionQuery contentDefinitionQuery,
            ICoeveryServices coeveryServices,
            ShellBlueprint shellBlueprint) {
            _contentDefinitionQuery = contentDefinitionQuery;
            _coeveryServices = coeveryServices;
            _shellBlueprint = shellBlueprint;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override void Removing(RemoveContentContext context) {
            var contentType = context.ContentType;

            var partDefinitions = _contentDefinitionQuery.ListPartDefinitions()
                .Where(x => x.Fields.Any(f => f.FieldDefinition.Name == "ReferenceField" && f.Settings.GetModel<ReferenceFieldSettings>().ContentTypeName == contentType)).ToList();
            foreach (var partDefinition in partDefinitions) {
                var referenceFields = partDefinition.Fields.Where(f => f.FieldDefinition.Name == "ReferenceField" && f.Settings.GetModel<ReferenceFieldSettings>().ContentTypeName == contentType).ToList();
                foreach (var referenceField in referenceFields) {
                    var typeName = partDefinition.Name.Remove(partDefinition.Name.LastIndexOf("Part", StringComparison.Ordinal));
                    var settings = referenceField.Settings.GetModel<ReferenceFieldSettings>();
                    ContentPartDefinition definition = partDefinition;
                    var recordBluePrint = _shellBlueprint.Records.First(x => x.Type.Name == definition.Name + "Record");
                    if (!recordBluePrint.Type.IsSubclassOf(typeof (ContentPartRecord))) {
                        continue;
                    }
                    Action<IAliasFactory> alias = x => x.ContentPartRecord(recordBluePrint.Type);
                    ContentPartFieldDefinition field = referenceField;
                    var query = _coeveryServices.ContentManager.HqlQuery().ForType(typeName).Where(alias, x => x.Eq(field.Name, context.ContentItem.Id));
                    if (settings.DeleteAction == DeleteAction.NotAllowed) {
                        var referencedCount = query.Count();
                        if (referencedCount > 0)
                        {
                            _coeveryServices.Notifier.Error(T("Could not delete this object because it is referenced by other objects!"));
                            context.Cancel = true;
                            break;
                        }
                    }
                    else if (settings.DeleteAction == DeleteAction.Clear) {
                        var contentItems = query.List().ToList();
                        foreach (var item in contentItems) {
                            var part = item.Parts.First(x => x.PartDefinition.Name == definition.Name);
                            var partRecordProperty = part.GetType().GetProperty("Record");
                            if (partRecordProperty != null) {
                                var partRecord = partRecordProperty.GetValue(part);
                                var property = partRecord.GetType().GetProperty(field.Name);
                                var nullValue = GetDefault(property.PropertyType);
                                property.SetValue(partRecord, nullValue);
                            }
                        }
                    }
                    else {
                        var contentItems = query.List().ToList();
                        foreach (var item in contentItems) {
                            _coeveryServices.ContentManager.Remove(item);
                        }
                    }
                }
            }
        }

        private object GetDefault(Type t)
        {
            return GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
        }

        private T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
}