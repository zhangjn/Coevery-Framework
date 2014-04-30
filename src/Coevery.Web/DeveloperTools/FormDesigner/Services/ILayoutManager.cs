using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.Common.Extensions;
using Coevery.DeveloperTools.EntityManagement.Services;
using Coevery.DeveloperTools.FormDesigner.Models;
using Coevery.Localization;
using Newtonsoft.Json;
using Column = Coevery.DeveloperTools.FormDesigner.Models.Column;

namespace Coevery.DeveloperTools.FormDesigner.Services {
    public interface ILayoutManager : IDependency {
        void DeleteField(string typeName, string fieldName);
        void AddField(string fieldName, SettingsDictionary settings);
        IEnumerable<Section> GetLayout(ContentTypeDefinition typeDefinition);
    }

    public class LayoutManager : ILayoutManager {
        private readonly IEntityMetadataService _entityMetadataService;

        public LayoutManager(IEntityMetadataService entityMetadataService) {
            _entityMetadataService = entityMetadataService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void DeleteField(string typeName, string fieldName) {
            var entity = _entityMetadataService.GetEntity(typeName);
            if (entity == null) {
                return;
            }

            var settings = entity.EntitySettings;
            var layout = JsonConvert.DeserializeObject<IList<Section>>(settings["Layout"]);
            foreach (var section in layout) {
                foreach (var row in section.Rows) {
                    var fieldColumn = row.Columns.FirstOrDefault(column => column.Field != null && column.Field.FieldName == fieldName);
                    if (fieldColumn != null) {
                        if (row.Columns.Count(x => x.Field != null) > 1) {
                            fieldColumn.Field = null;
                        }
                        else {
                            section.Rows.Remove(row);
                        }

                        settings["Layout"] = JsonConvert.SerializeObject(layout);
                        entity.EntitySettings = settings;
                        break;
                    }
                }
            }
        }

        public void AddField(string fieldName, SettingsDictionary settings) {
            string layoutAttribute;
            IList<Section> layout = new List<Section>(new[] {
                new Section {
                    SectionColumns = 1,
                    SectionColumnsWidth = "12",
                    SectionTitle = T("General Information").Text
                }
            });
            if (settings.TryGetValue("Layout", out layoutAttribute)) {
                layout = JsonConvert.DeserializeObject<IList<Section>>(layoutAttribute);
            }
            var emptyColumn = layout.SelectMany(section => section.Rows.SelectMany(row => row.Columns))
                .FirstOrDefault(column => column != null && column.Field == null);
            if (emptyColumn == null) {
                var section = layout.Last();
                var row = new Row {Columns = new Column[section.SectionColumns]};
                section.Rows.Add(row);
                emptyColumn = new Column();
                row.Columns[0] = emptyColumn;
            }
            emptyColumn.Field = new Field {FieldName = fieldName};
            settings["Layout"] = JsonConvert.SerializeObject(layout);
        }

        public IEnumerable<Section> GetLayout(ContentTypeDefinition typeDefinition) {
            if (typeDefinition.Settings.ContainsKey("Layout")) {
                return JsonConvert.DeserializeObject<List<Section>>(typeDefinition.Settings["Layout"]);
            }

            var section = new Section {
                SectionColumns = 1,
                SectionColumnsWidth = "12",
                SectionTitle = T("General Information").Text
            };
            string partName = typeDefinition.Name.ToPartName();
            var fields = typeDefinition.Parts.First(x => x.PartDefinition.Name == partName).PartDefinition.Fields;
            section.Rows = fields.Select(field => new Row {
                Columns = new[] {
                    new Column {Field = new Field {FieldName = field.Name}}
                }
            }).ToList();
            return new[] {section};
        }
    }
}