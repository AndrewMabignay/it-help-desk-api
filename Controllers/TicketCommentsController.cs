using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers
{
    public class TicketCommentsController : Controller
    {
        // GET: TicketCommentsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TicketCommentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TicketCommentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketCommentsController/Create
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

        // GET: TicketCommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TicketCommentsController/Edit/5
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

        // GET: TicketCommentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TicketCommentsController/Delete/5
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
