using System.Web.Mvc;

namespace ETicket.Areas.Mis
{
    public class MisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mis_default",
                "Mis/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "ETicket.Areas.Mis.Controllers" }
            );
        }
    }
}