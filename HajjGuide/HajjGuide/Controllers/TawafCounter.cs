using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace HajjGuide.Controllers
{
    public class TawafCounterController : Controller
    {
        private const int MaxCount = 7;
        private const int MinCount = 0;

        public IActionResult Index()
        {
            var tawafCounterModel = new TawafCounterModel { Count = 0 };
            return View(tawafCounterModel);
        }

        [HttpPost]
        public ActionResult Increment(TawafCounterModel model)
        {
            if (model.Count < MaxCount)
            {
                model.Count++;
            }

            return View("Index", model);
        }
        [HttpPost]
        public ActionResult Reset()
        {
            var tawafCounterModel = new TawafCounterModel { Count = 0 };
            return View("Index", tawafCounterModel);
        }
        [HttpPost]
        public ActionResult Decrement(TawafCounterModel model)
        {
            if (model.Count > 0)
            {
                model.Count--;
            }

            return View("Index", model);
        }
    }
}
