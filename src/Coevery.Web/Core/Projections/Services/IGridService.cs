namespace Coevery.Core.Projections.Services {
    public interface IGridService : IDependency {
        bool GenerateSortCriteria(string entityName, string sidx, string sord, int queryId);
    }
}