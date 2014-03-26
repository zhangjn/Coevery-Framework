﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Mvc.Filters;

namespace Coevery.Core.Common.Services
{
    public class LayoutFilter : FilterProvider, IResultFilter
    {
        private readonly IWorkContextAccessor _wca;

        public LayoutFilter(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var workContext = _wca.GetContext();
            var routeValues = filterContext.RouteData.Values;
            workContext.Layout.Metadata.Alternates.Add(BuildShapeName(routeValues, "area"));
            workContext.Layout.Metadata.Alternates.Add(BuildShapeName(routeValues, "area", "controller"));
            workContext.Layout.Metadata.Alternates.Add(BuildShapeName(routeValues, "area", "controller", "action"));
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        private static string BuildShapeName(RouteValueDictionary values, params string[] names)
        {
            return "Layout__" +
                string.Join("__",
                names.Select(s => ((string)values[s] ?? "").Replace(".", "_")));
        }
    }
}