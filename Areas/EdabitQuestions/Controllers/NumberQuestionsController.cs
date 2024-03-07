using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcPractice.Areas.EdabitQuestions.Models;
using MvcPractice.Helpers;
using System.Reflection;

namespace MvcPractice.Areas.EdabitQuestions.Controllers
{
    [Area("EdabitQuestions")]
    public class NumberQuestionsController : Controller
    {
        //Dependancy injection
        private readonly IViewRenderService _viewRenderService;
        public NumberQuestionsController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            NumberQuestionsModel model = new NumberQuestionsModel();           
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ShowQuestionModal(int questionId)
        {
            try
            {
                NumberQuestionsModel model = new NumberQuestionsModel();

                foreach (NumberQuestion item in model.QuestionList)
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
        public ActionResult PerformQuestionLogic(List<int> p, int questionId)
        {
            try
            {
                NumberQuestionsModel model = new NumberQuestionsModel();
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
