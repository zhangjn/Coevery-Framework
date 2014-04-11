using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Coevery.DisplayManagement;
using Coevery.Mvc.Filters;
using Coevery.UI.Admin;
using Coevery.UI.Navigation;

namespace Coevery.Core.Common {
    public class FrontMenuFilter : FilterProvider, IResultFilter {
        private readonly INavigationManager _navigationManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly dynamic _shapeFactory;

        public FrontMenuFilter(IWorkContextAccessor workContextAccessor,
            IShapeFactory shapeFactory,
            INavigationManager navigationManager) {
            _workContextAccessor = workContextAccessor;
            _shapeFactory = shapeFactory;
            _navigationManager = navigationManager;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // should only run on a full view rendering result
            if (!(filterContext.Result is ViewResult)) {
                return;
            }

            WorkContext workContext = _workContextAccessor.GetContext(filterContext);

            const string menuName = "FrontMenu";
            if (AdminFilter.IsApplied(filterContext.RequestContext)) {
                return;
            }

            IEnumerable<MenuItem> menuItems = _navigationManager.BuildMenu(menuName).ToList();
            // adding query string parameters
            var routeData = new RouteValueDictionary(filterContext.RouteData.Values);
            var queryString = workContext.HttpContext.Request.QueryString;
            if (queryString != null) {
                foreach (var key in from string key in queryString.Keys where key != null && !routeData.ContainsKey(key) let value = queryString[key] select key) {
                    routeData[key] = queryString[key];
                }
            }

            // Set the currently selected path
            NavigationHelper.SetSelectedPath(menuItems, workContext.HttpContext.Request, routeData);

            // Populate main nav
            dynamic menuShape = _shapeFactory.Menu(MenuName: menuName, ItemList: menuItems);
            workContext.Layout.Navigation.Add(menuShape);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {}
    }
}