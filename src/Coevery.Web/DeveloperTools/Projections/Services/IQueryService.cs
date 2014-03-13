using Coevery.DeveloperTools.Projections.Models;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface IQueryService : IDependency {
        QueryPart GetQuery(int id);

        QueryPart CreateQuery(string name);
        void DeleteQuery(int id);
    }
}