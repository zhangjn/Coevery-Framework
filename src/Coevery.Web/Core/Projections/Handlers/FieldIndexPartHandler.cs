﻿//using System.Collections.Generic;
//using System.Linq;
//using Coevery.ContentManagement;
//using Coevery.ContentManagement.Drivers;
//using Coevery.ContentManagement.FieldStorage;
//using Coevery.ContentManagement.Handlers;
//using Coevery.ContentManagement.MetaData;
//using Coevery.ContentManagement.MetaData.Models;
//using Coevery.Core.Projections.Models;
//using Coevery.Core.Projections.Services;
//using Coevery.Data;

//namespace Coevery.Core.Projections.Handlers {
//    public class FieldIndexPartHandler : ContentHandler {
//        private readonly IContentDefinitionManager _contentDefinitionManager;
//        private readonly IFieldIndexService _fieldIndexService;
//        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
//        private readonly IFieldStorageProviderSelector _fieldStorageProviderSelector;

//        public FieldIndexPartHandler(
//            IContentDefinitionManager contentDefinitionManager,
//            IRepository<FieldIndexPartRecord> repository,
//            IFieldIndexService fieldIndexService,
//            IEnumerable<IContentFieldDriver> contentFieldDrivers,
//            IFieldStorageProviderSelector fieldStorageProviderSelector) {
//            Filters.Add(StorageFilter.For(repository));
//            _contentDefinitionManager = contentDefinitionManager;
//            _fieldIndexService = fieldIndexService;
//            _contentFieldDrivers = contentFieldDrivers;
//            _fieldStorageProviderSelector = fieldStorageProviderSelector;

//            OnPublishing<FieldIndexPart>(Publishing);
//        }

//        protected override void Activating(ActivatingContentContext context) {
//            base.Activating(context);

//            // weld the FieldIndexPart dynamically, if a field has been assigned to one of its parts
//            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(context.ContentType);
//            if (contentTypeDefinition == null)
//                return;
//            if (contentTypeDefinition.Parts.Any(p => p.PartDefinition.Fields.Any())) {
//                context.Builder.Weld<FieldIndexPart>();
//            }
//        }

//        public void Publishing(PublishContentContext context, FieldIndexPart fieldIndexPart) {
//            foreach (var part in fieldIndexPart.ContentItem.Parts) {
//                foreach (var field in part.PartDefinition.Fields) {
//                    // get the correct field storage provider
//                    var fieldStorageProvider = _fieldStorageProviderSelector.GetProvider(field);

//                    // get all drivers for the current field type
//                    // the driver will describe what values of the field should be indexed
//                    var drivers = _contentFieldDrivers.Where(x => x.GetFieldInfo().Any(fi => fi.FieldTypeName == field.FieldDefinition.Name)).ToList();

//                    ContentPart localPart = part;
//                    ContentPartFieldDefinition localField = field;
//                    var membersContext = new DescribeMembersContext(
//                        (storageName, storageType, displayName, description) => {
//                            var fieldStorage = fieldStorageProvider.BindStorage(localPart, localField);

//                            // fieldStorage.Get<T>(storageName)
//                            var getter = typeof(IFieldStorage).GetMethod("Get").MakeGenericMethod(storageType);
//                            var fieldValue = getter.Invoke(fieldStorage, new[] { storageName });

//                            _fieldIndexService.Set(fieldIndexPart,
//                                localPart.PartDefinition.Name,
//                                localField.Name,
//                                storageName, fieldValue, storageType);
//                        });

//                    foreach (var driver in drivers) {
//                        driver.Describe(membersContext);
//                    }
//                }
//            }
//        }
//    }
//}