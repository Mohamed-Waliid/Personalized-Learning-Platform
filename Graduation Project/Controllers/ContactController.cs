using Graduation_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace Graduation_Project.Controllers
{
    public class ContactController : Controller
    {
        public ContactController() { }

        public IActionResult Index()
        {
            return View("index");
        }

        [HttpPost]
        public IActionResult SendMessage(ContactViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", obj);
            }

            return View("Success");
        }
    }
}