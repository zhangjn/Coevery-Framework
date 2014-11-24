using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Coevery.DisplayManagement;
using Coevery.Environment.Extensions;
using Coevery.Localization;
using Coevery.Mvc.Filters;
using Coevery.UI.Notify;

namespace Coevery.PropertyManagement.Services
{
    [CoeverySuppressDependency("Coevery.UI.Notify.NotifyFilter")]
    public class NotifyFilter : FilterProvider, IActionFilter, IResultFilter
    {
        private const string TempDataMessages = "messages";
        private readonly INotifier _notifier;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly dynamic _shapeFactory;

        public NotifyFilter(
            INotifier notifier,
            IWorkContextAccessor workContextAccessor,
            IShapeFactory shapeFactory)
        {
            _notifier = notifier;
            _workContextAccessor = workContextAccessor;
            _shapeFactory = shapeFactory;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResultBase;

            // if it's not a view result, a redirect for example
            if (viewResult == null)
                return;

            if (!_notifier.List().Any())
                return;
            var messageEntries = _notifier.List().ToList();
            if (!messageEntries.Any())
                return;

            var messagesZone = _workContextAccessor.GetContext(filterContext).Layout.Zones["Messages"];
            foreach (var messageEntry in messageEntries)
                messagesZone = messagesZone.Add(_shapeFactory.Message(messageEntry));
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) { }
    }
}