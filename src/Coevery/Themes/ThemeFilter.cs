using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Coevery.Mvc.Filters;

namespace Coevery.Themes {
    public class ThemeFilter : FilterProvider, IActionFilter, IResultFilter {
        public void OnActionExecuting(ActionExecutingContext filterContext) {
            var attribute = GetThemedAttribute(filterContext.ActionDescriptor);
            if (attribute != null && attribute.Enabled) {
                Apply(filterContext.RequestContext);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) {}

        public void OnResultExecuting(ResultExecutingContext filterContext) {}

        public void OnResultExecuted(ResultExecutedContext filterContext) {}

        public static void Apply(RequestContext context) {
            // the value isn't important
            context.HttpContext.Items[typeof (ThemeFilter)] = null;
        }

        public static void Disable(RequestContext context) {
            context.HttpContext.Items.Remove(typeof (ThemeFilter));
        }

        public static bool IsApplied(RequestContext context) {
            return context.HttpContext.Items.Contains(typeof (ThemeFilter));
        }

        private static ThemedAttribute GetThemedAttribute(ActionDescriptor descriptor) {
            return descriptor.GetCustomAttributes(typeof (ThemedAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof (ThemedAttribute), true))
                .OfType<ThemedAttribute>()
                .FirstOrDefault();
        }
    }
}