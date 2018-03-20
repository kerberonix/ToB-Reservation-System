using System.Web;
using System.Web.Mvc;

namespace TobReservationSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); // redirects user to error page when action throws an error
            filters.Add(new AuthorizeAttribute());
        }
    }
}
