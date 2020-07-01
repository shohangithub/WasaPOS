using App.Helper;
using System.Web.Mvc;
using Utility;

namespace App.Controllers
{

    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
           // string email = SessionManager.GetSession(GlobalConstant.LOGIN).ToString();
           
           
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}