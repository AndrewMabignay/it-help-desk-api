using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers
{
    public class PrioritiesController : Controller
    {
        // GET: PrioritiesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PrioritiesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PrioritiesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrioritiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PrioritiesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PrioritiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PrioritiesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PrioritiesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
