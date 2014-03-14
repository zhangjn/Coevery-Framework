using Autofac;

namespace Coevery.Core.Projections.FieldTypeEditors {
    public class FieldTypeEditorModule: Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAdapter<IFieldTypeEditor, IConcreteFieldTypeEditor>(editor => new FieldTypeEditorAdapterr(editor));
        }
    }
}