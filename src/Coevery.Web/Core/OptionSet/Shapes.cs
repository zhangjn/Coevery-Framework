using Coevery.DisplayManagement.Descriptors;

namespace Coevery.Core.OptionSet {
    public class Shapes : IShapeTableProvider {

        public void Discover(ShapeTableBuilder builder) {

            builder.Describe("OptionSet")
                .OnDisplaying(displaying => {
                    var shape = displaying.Shape;
                    shape.Classes.Add("option-set");
                });
        }
    }
}