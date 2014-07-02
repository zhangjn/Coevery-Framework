namespace Coevery.ContentManagement.Handlers {
    public class RemoveContentContext : ContentContextBase {
        public RemoveContentContext(ContentItem contentItem) : base(contentItem) {
        }

        public bool Cancel { get; set; }
    }
}