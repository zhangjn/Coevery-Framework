using System;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Handlers;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.ContentManagement.Records;
using Coevery.Core.Relationships.Settings;
using Coevery.Data;
using Coevery.Environment.Extensions;
using Coevery.Environment.ShellBuilders.Models;
using Coevery.Localization;
using Coevery.UI.Notify;
using NHibernate.Persister.Entity;

namespace Coevery.PropertyManagement.Handlers {
    [CoeverySuppressDependency("Coevery.Core.Common.Handlers.ContentDeletingHandler")]
    public class ContentDeletingHandler : ContentHandlerBase {
        private readonly IContentDefinitionQuery _contentDefinitionQuery;
        private readonly ICoeveryServices _coeveryServices;
        private readonly ShellBlueprint _shellBlueprint;
        private readonly Lazy<ISessionLocator> _sessionLocator; 

        public ContentDeletingHandler(
            IContentDefinitionQuery contentDefinitionQuery,
            ICoeveryServices coeveryServices,
            ShellBlueprint shellBlueprint, 
            Lazy<ISessionLocator> sessionLocator) {
            _contentDefinitionQuery = contentDefinitionQuery;
            _coeveryServices = coeveryServices;
            _shellBlueprint = shellBlueprint;
            _sessionLocator = sessionLocator;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override void Removing(RemoveContentContext context) {
            var contentType = context.ContentType;
            if (context.Cancel) return;
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

                    ContentPartFieldDefinition field = referenceField;
                    var session = _sessionLocator.Value.For(typeof (ContentItemRecord));
                    var contentItemMetadata = session.SessionFactory.GetClassMetadata(recordBluePrint.Type);
                    var entityPersister = (AbstractEntityPersister) contentItemMetadata;
                    var propertyNameList = entityPersister.PropertyNames.ToDictionary(x => x, entityPersister.GetPropertyColumnNames);
                    var propertyName = propertyNameList.Where(kvp => kvp.Value.Contains(field.Name)).Select(kvp => kvp.Key).FirstOrDefault();
                    if (propertyName == null) {
                        continue;
                    }
                    Action<IAliasFactory> alias = x => x.ContentPartRecord(recordBluePrint.Type);
                    var query = _coeveryServices.ContentManager.HqlQuery().ForType(typeName).Where(alias, x => x.Eq(propertyName, context.ContentItem.Id));
                    if (settings.DeleteAction == DeleteAction.NotAllowed) {
                        var referencedCount = query.Count();
                        if (referencedCount > 0) {
                            _coeveryServices.Notifier.Error(T("无法删除这条数据因为它已经被其他的数据使用！"));
                            context.Cancel = true;
                            return;
                        }
                    }
                    else if (settings.DeleteAction == DeleteAction.Clear) {
                        var contentItems = query.List().ToList();
                        foreach (var item in contentItems) {
                            var part = item.Parts.First(x => x.PartDefinition.Name == definition.Name);
                            var partRecordProperty = part.GetType().GetProperty("Record");
                            if (partRecordProperty != null) {
                                var partRecord = partRecordProperty.GetValue(part,null);
                                var property = partRecord.GetType().GetProperty(field.Name);
                                var nullValue = GetDefault(property.PropertyType);
                                property.SetValue(partRecord, nullValue, null);
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

        public override void Removed(RemoveContentContext context) {
            base.Removed(context);
            // Delete the ContentItemRecord
            var session = _sessionLocator.Value.For(typeof (ContentItemRecord));

            foreach (var version in context.ContentItemRecord.Versions) {
                var sql = string.Format("delete from {0} where ContentItemRecord_id = :id", typeof (ContentItemVersionRecord).FullName);
                session.CreateQuery(sql).SetParameter("id", version.Id).ExecuteUpdate();
            }

            var hsql = string.Format("delete from {0} where id = :id", typeof (ContentItemRecord).FullName);
            session.CreateQuery(hsql).SetParameter("id", context.Id).ExecuteUpdate();
        }
    }
}