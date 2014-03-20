namespace Coevery.Core.Projections.ViewModels {
    public class FilterData {
        public string Category { get; set; }
        public string Type { get; set; }
        public Data[] FormData { get; set; }
    }

    public class Data {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}