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
        private readonly IEnumerable<IContentFieldDriver> _drivers;
        private readonly Work<IShapeFactory> _shapeFactory;

        public Shapes(IEnumerable<IContentFieldDriver> drivers, Work<IShapeFactory> shapeFactory) {
            _drivers = drivers;
            _shapeFactory = shapeFactory;
            Logger = NullLogger.Instance;
        }

        public IShapeFactory ShapeFactory {
            get { return _shapeFactory.Value; }
        }

        public ILogger Logger { get; set; }

        public void Discover(ShapeTableBuilder builder) {}

        [Shape]
        public void DisplayField(ContentPart part, string fieldName, dynamic Display, TextWriter Output) {
            dynamic itemShape = ShapeFactory.Create("ViewModel", Arguments.Empty(), () => new ZoneHolding(() => ShapeFactory.Create("ContentZone", Arguments.Empty())));
            itemShape.ContentItem = part.ContentItem;

            var context = new BuildEditorContext(itemShape, part, string.Empty, ShapeFactory);
            context.FindPlacement = (partShapeType, differentiator, defaultLocation) => new PlacementInfo {Location = "Fields"};

            _drivers.Invoke(driver => {
                context.Logger = Logger;
                var result = driver.BuildEditorShape(context);
                if (result != null) {
                    result.Apply(context);
                }
            }, Logger);

            IEnumerable<dynamic> fields = context.Shape.Fields.Items;
            string partName = part.PartDefinition.Name;
            dynamic field = fields.FirstOrDefault(x => x.ContentPart.PartDefinition.Name == partName && x.ContentField.Name == fieldName);
            if (field != null) {
                Output.Write(Display(field));
            }
        }
    }
}