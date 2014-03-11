namespace Coevery.Core.Entities.Services {
    public interface IModelInitializer {
        void InitializeModel<TModel>(TModel model) where TModel : class;
    }
}