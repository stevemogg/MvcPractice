using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcPractice.Areas.EdabitQuestions.Models;
using MvcPractice.Helpers;

namespace MvcPractice.Areas.EdabitQuestions.Controllers
{
    [Area("EdabitQuestions")]
    public class StringQuestionsController : Controller
    {
        //Dependancy injection
        private readonly IViewRenderService _viewRenderService;
        public StringQuestionsController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            StringQuestionsModel model = new StringQuestionsModel();
            return View(model);
        }

        [HttpPost]
        //Doesnt work for some reason with the headers
        //[ValidateAntiForgeryToken]
        public ActionResult ShowQuestionModal(int questionId)
        {
            try
            {
                StringQuestionsModel model = new StringQuestionsModel();

                foreach (StringQuestion item in model.QuestionList)
                {
                    if (item.QuestionId == questionId)
                    {
                        model.SelectedQuestion = item;
                        string html = _viewRenderService.RenderToStringAsync("_Modal", model).Result;
                        return Json(new { success = true, html });
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult PerformQuestionLogic(List<string> p, int questionId)
        {
            try
            {
                StringQuestionsModel model = new StringQuestionsModel();
                foreach (var item in model.QuestionList)
                {
                    if (questionId == item.QuestionId)
                    {
                        item.p = p;
                        dynamic? answer = item.PerformQuestionLogic();
                        string html = $"<p>The answer is {answer.ToString()}</p>";
                        return Json(new { success = true, html });
                    }
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}