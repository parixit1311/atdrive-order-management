using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Orders");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }

    public IActionResult StatusCodeHandler(int code)
    {
        if (code == StatusCodes.Status404NotFound)
        {
            return View("PageNotFound");
        }

        return View("Error");
    }
}
