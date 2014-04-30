﻿//using System.Collections.Generic;
//using System.Linq;
//using Coevery.ContentManagement.Drivers;
//using Coevery.ContentManagement.Handlers;
//using Coevery.Core.Common.Extensions;
//using Coevery.Core.Projections.Descriptors.Filter;
//using Coevery.Core.Projections.FieldTypeEditors;
//using Coevery.Core.Projections.Services;
//using Coevery.Localization;
//using Coevery.Utility.Extensions;

//namespace Coevery.Core.Projections.Providers.Filters {
//    public class ContentFieldsFilter : IFilterProvider {
//        private readonly IContentDefinitionExtension _contentDefinitionExtension;
//        private readonly IEnumerable<IContentFieldDriver> _contentFieldDrivers;
//        private readonly IEnumerable<IConcreteFieldTypeEditor> _fieldTypeEditors;

//        public ContentFieldsFilter(
//            IContentDefinitionExtension contentDefinitionExtension,
//            IEnumerable<IContentFieldDriver> contentFieldDrivers,
//            IEnumerable<IConcreteFieldTypeEditor> fieldTypeEditors) {
//            _contentDefinitionExtension = contentDefinitionExtension;
//            _contentFieldDrivers = contentFieldDrivers;
//            _fieldTypeEditors = fieldTypeEditors;
//            T = NullLocalizer.Instance;
//        }

//        public Localizer T { get; set; }

//        public void Describe(DescribeFilterContext describe) {
//            foreach (var part in _contentDefinitionExtension.ListUserDefinedPartDefinitions()) {
//                if (!part.Fields.Any()) {
//                    continue;
//                }

//                var descriptor = describe.For(part.Name + "ContentFields", T("{0} Content Fields", part.Name.CamelFriendly()), T("Content Fields for {0}", part.Name.CamelFriendly()));

//                foreach (var field in part.Fields) {
//                    var localField = field;
//                    var localPart = part;
//                    var drivers = _contentFieldDrivers.Where(x => x.GetFieldInfo().Any(fi => fi.FieldTypeName == localField.FieldDefinition.Name)).ToList();

//                    var membersContext = new DescribeMembersContext(
//                        (storageName, storageType, displayName, description) => {
//                            // look for a compatible field type editor
//                            IConcreteFieldTypeEditor concreteFieldTypeEditor = _fieldTypeEditors.FirstOrDefault(x => x.CanHandle(localField.FieldDefinition.Name, storageType))
//                                                                               ?? _fieldTypeEditors.FirstOrDefault(x => x.CanHandle(storageType));
//                            IFieldTypeEditor fieldTypeEditor = concreteFieldTypeEditor;

//                            if (fieldTypeEditor == null) return;

//                            descriptor.Element(
//                                type: localPart.Name + "." + localField.Name + "." + storageName,
//                                name: new LocalizedString(localField.DisplayName + (displayName != null ? ":" + displayName.Text : "")),
//                                description: description ?? T("{0} property for {1}", storageName, localField.DisplayName),
//                                filter: context => concreteFieldTypeEditor.ApplyFilter(context, storageName, storageType, localPart, localField),
//                                display: context => fieldTypeEditor.DisplayFilter(localPart.Name.CamelFriendly() + "." + localField.DisplayName, storageName, context.State),
//                                form: fieldTypeEditor.FormName);
//                        });

//                    foreach (var driver in drivers) {
//                        driver.Describe(membersContext);
//                    }
//                }
//            }
//        }
//    }
//}