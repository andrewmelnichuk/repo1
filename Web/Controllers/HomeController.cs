using System.Web.Mvc;

namespace Yuka.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewModel.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
