namespace Coevery.DeveloperTools.Projections.Services {
    public interface ISortService : IDependency {
        void MoveUp(int sortId);
        void MoveDown(int sortId);
    }
}
