using System.Collections.Generic;
using System.IO;
using System.Linq;
using Coevery.ContentManagement;
using Coevery.ContentManagement.Drivers;
using Coevery.ContentManagement.Handlers;
using Coevery.DisplayManagement;
using Coevery.DisplayManagement.Descriptors;
using Coevery.Environment;
using Coevery.Logging;
using Coevery.UI.Zones;

namespace Coevery.Core.Fields {
    public class Shapes : IShapeTableProvider {
        private readonly IEnumerable<Work<IContentFieldDriver>> _contentFieldDrivers;
        private readonly Work<IShapeFactory> _shapeFactory;

        public Shapes(Work<IShapeFactory> shapeFactory, 
            IEnumerable<Work<IContentFieldDriver>> contentFieldDrivers) {
            _shapeFactory = shapeFactory;
            _contentFieldDrivers = contentFieldDrivers;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void Discover(ShapeTableBuilder builder) {}

        [Shape]
        public void DisplayField(ContentPart part, string fieldName, dynamic Display, TextWriter Output) {

            var shapeFactory = _shapeFactory.Value;

            dynamic itemShape = shapeFactory.Create("Content", Arguments.Empty(), () => new ZoneHolding(() => shapeFactory.Create("ContentZone", Arguments.Empty())));
            itemShape.ContentItem = part.ContentItem;

            var context = new BuildEditorContext(itemShape, part, "", string.Empty, shapeFactory);
            context.FindPlacement = (partShapeType, differentiator, defaultLocation) => new PlacementInfo {Location = "Content"};

            var field = part.Fields.FirstOrDefault(f => f.Name == fieldName);
            if (field != null) {
                var fieldTypeName = field.FieldDefinition.Name;
                var drivers = _contentFieldDrivers.Where(x => x.Value.GetFieldInfo().Any(fi => fi.FieldTypeName == fieldTypeName)).ToList();
                drivers.Invoke(driver => {
                    context.Logger = Logger;
                    var result = driver.Value.BuildEditorShape(context) as CombinedResult;
                    if (result != null) {
                        var driverResults = result.GetResults().Where(x => x.ContentField == field);
                        foreach (var driverResult in driverResults) {
                            driverResult.Apply(context);
                        }
                    }
                }, Logger);
                Output.Write(Display(context.Shape));
            }
        }
    }
}