using Microsoft.AspNetCore.Mvc;
using MvcPractice.Areas.Test.Models;
using System.Reflection;

namespace MvcPractice.Areas.Test.Controllers
{
    //[Authorize(Roles = "System Access")]
    [Area("Test")]
    public class TestController : Controller
    {
        TestModel IndexModel = new TestModel();

        public IActionResult Index()
        {            
            return View(IndexModel);
        }        
    }
}