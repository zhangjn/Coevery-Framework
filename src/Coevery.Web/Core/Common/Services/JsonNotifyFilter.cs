using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Coevery;
using Coevery.DisplayManagement;
using Coevery.Localization;
using Coevery.Mvc.Filters;
using Coevery.UI.Notify;
using Newtonsoft.Json;

namespace Coevery.Core.Common.Services {
    public class JsonNotifyFilter : FilterProvider, IResultFilter {
        private const string TempDataMessages = "messages";
        private readonly INotifier _notifier;

        public JsonNotifyFilter(
            INotifier notifier) {
            _notifier = notifier;
        }


        public void OnResultExecuted(ResultExecutedContext filterContext) {

            HttpResponseBase response = filterContext.HttpContext.Response;

            var viewResult = filterContext.Result as HttpStatusCodeResult;
            if (viewResult == null)
                return;

            if (!_notifier.List().Any())
                return;

            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;

            var messageEntries = _notifier.List();

            var json = JsonConvert.SerializeObject(new { __Messages = messageEntries.Select(x => new { x.Type, Message = x.Message.Text }) });
            response.Write(json);
        }


        public void OnResultExecuting(ResultExecutingContext filterContext) {}
    }
}