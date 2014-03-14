using System.Web.Routing;
using Coevery.Themes;
using Coevery.UI.Admin;
using JetBrains.Annotations;

namespace Coevery.Core.Common.Services {
    [UsedImplicitly]
    public class SafeModeThemeSelector : IThemeSelector {
        public ThemeSelectorResult GetTheme(RequestContext context) {
            if (!AdminFilter.IsApplied(context))
            {
                return new ThemeSelectorResult { Priority = -100, ThemeName = "Mooncake" };
            }
            return null;
        }

    }
}