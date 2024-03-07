using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using MvcPractice.Areas.FormData.Models;
using static MvcPractice.Areas.FormData.Models.FormDataModel;

namespace MvcPractice.Areas.FormData.Controllers
{
    [Area("FormData")]

    public class FormDataController : Controller
    {
        public IActionResult Index()
        {
            FormDataModel model = new FormDataModel();

            IList<Student> studentList = new List<Student>()
            {
                new Student() { StudentID = 1, StudentName = "John", Age = 18 } ,
                new Student() { StudentID = 2, StudentName = "Steve",  Age = 21 } ,
                new Student() { StudentID = 3, StudentName = "Bill",  Age = 18 } ,
                new Student() { StudentID = 4, StudentName = "Ram" , Age = 20 } ,
                new Student() { StudentID = 5, StudentName = "Abram" , Age = 21 }
            };
            var grouped = studentList.GroupBy(s => s.Age);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormData1(List<int> listInts)
        {
            try
            {
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        #region Testing AntiForgery tokens with request http types
        //GET
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult TestGETRequestWithTokenInHEADER()
        {
            return Json(new { success = true });
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult TestGETRequestWithTokenInBODY()
        {
            return Json(new { success = true });
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TestPOSTRequestWithTokenInHEADER()
        {
            return Json(new { success = true });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TestPOSTRequestWithTokenInBODY()
        {
            var body = Request;
            return Json(new { success = true });
        }
        //Put
        [HttpPut]
        [ValidateAntiForgeryToken]
        public IActionResult TestPUTRequestWithTokenInHEADER()
        {
            return Json(new { success = true });
        }
        [HttpPut]
        [ValidateAntiForgeryToken]
        public IActionResult TestPUTRequestWithTokenInBODY()
        {
            return Json(new { success = true });
        }
        //Delete
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult TestDELETERequestWithTokenInHEADER()
        {
            return Json(new { success = true });
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult TestDELETERequestWithTokenInBODY()
        {
            return Json(new { success = true });
        }
        //Head
        [HttpHead]
        [ValidateAntiForgeryToken]
        public IActionResult TestHEADRequestWithTokenInHEADER()
        {
            var headers = Request.Headers;
            return Json(new { success = true });
        }
        [HttpHead]
        [ValidateAntiForgeryToken]
        public IActionResult TestHEADRequestWithTokenInBODY()
        {
            return Json(new { success = true });
        }
        //Options
        [HttpOptions]
        [ValidateAntiForgeryToken]
        public IActionResult TestOPTIONSRequestWithTokenInHEADER()
        {
            return Json(new { success = true });
        }
        [HttpOptions]
        [ValidateAntiForgeryToken]
        public IActionResult TestOPTIONSRequestWithTokenInBODY()
        {
            return Json(new { success = true });
        }
        //Patch
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public IActionResult TestPatchRequestWithTokenInHEADER()
        {
            return Json(new { success = true });
        }
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public IActionResult TestPatchRequestWithTokenInBODY()
        {
            return Json(new { success = true });
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AjaxWithAllSettings()
        {
            var req = Request;
            return Json(new { success = true });
        }
    }
}