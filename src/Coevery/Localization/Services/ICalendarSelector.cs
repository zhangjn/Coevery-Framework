using System.Web;

namespace Coevery.Localization.Services {
	public class CalendarSelectorResult {
		public int Priority {
			get;
			set;
		}
		public string CalendarName {
			get;
			set;
		}
	}

	public interface ICalendarSelector : IDependency {
		CalendarSelectorResult GetCalendar(HttpContextBase context);
	}
}
