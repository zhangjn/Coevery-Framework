using System.Collections.Generic;
using System.Linq;
using Coevery.ContentManagement.MetaData;
using Coevery.Core.Common.Extensions;
using Coevery.DeveloperTools.FormDesigner.Models;
using Coevery.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coevery.DeveloperTools.FormDesigner.Services {
    public interface ILayoutManager : IDependency {
        void DeleteField(string typeName, string fieldName);
        void AddField(string typeName, string fieldName);
        void GenerateDefaultLayout(string typeName);
    }

    public class LayoutManager : ILayoutManager {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public LayoutManager(IContentDefinitionManager contentDefinitionManager) {
            _contentDefinitionManager = contentDefinitionManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void DeleteField(string typeName, string fieldName) {
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(typeName);
            if (typeDefinition == null || !typeDefinition.Settings.ContainsKey("Layout")) {
                return;
            }

            var layout = JsonConvert.DeserializeObject<IList<Section>>(typeDefinition.Settings["Layout"]);
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

                        typeDefinition.Settings["Layout"] = JsonConvert.SerializeObject(layout);
                        _contentDefinitionManager.StoreTypeDefinition(typeDefinition);
                        break;
                    }
                }
            }
        }

        public void AddField(string typeName, string fieldName) {
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(typeName);
            if (typeDefinition == null) {
                return;
            }

            var layout = JsonConvert.DeserializeObject<IList<Section>>(typeDefinition.Settings["Layout"]);
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
            typeDefinition.Settings["Layout"] = JsonConvert.SerializeObject(layout);
            _contentDefinitionManager.StoreTypeDefinition(typeDefinition);
        }

        public void GenerateDefaultLayout(string typeName) {
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(typeName);
            if (typeDefinition == null) {
                return;
            }

            var layout = new JArray();
            var section = new JObject();
            layout.Add(section);
            section["SectionColumns"] = 1;
            section["SectionColumnsWidth"] = "12";
            section["SectionTitle"] = T("General Information").Text;
            var rows = new JArray();
            section["Rows"] = rows;
            string partName = typeName.ToPartName();
            var fields = typeDefinition.Parts.First(x => x.PartDefinition.Name == partName).PartDefinition.Fields;
            foreach (var field in fields) {
                var row = new JObject();
                rows.Add(row);
                var columns = new JArray();
                row["Columns"] = columns;
                var column = new JObject();
                columns.Add(column);
                var fieldObject = new JObject();
                column["Field"] = fieldObject;
                fieldObject["FieldName"] = field.Name;
            }
            typeDefinition.Settings["Layout"] = JsonConvert.SerializeObject(layout);
            _contentDefinitionManager.StoreTypeDefinition(typeDefinition);
        }
    }
}