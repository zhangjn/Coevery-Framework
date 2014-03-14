using Coevery.Core.Projections.Models;

namespace Coevery.Core.Projections.Services {
    public interface IQueryService : IDependency {
        QueryPart GetQuery(int id);

        QueryPart CreateQuery(string name);
        void DeleteQuery(int id);
    }
}