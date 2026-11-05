using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        // GET: TicketAttachmentsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TicketAttachmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TicketAttachmentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketAttachmentsController/Create
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

        // GET: TicketAttachmentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TicketAttachmentsController/Edit/5
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

        // GET: TicketAttachmentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TicketAttachmentsController/Delete/5
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
