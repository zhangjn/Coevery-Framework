using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coevery;
using Coevery.Localization;
using Coevery.Mvc;
using Coevery.Themes;
using Sample.Lead.Models;

namespace Sample.Lead.Controllers
{
    [Themed]
    public class LeadController : Controller
    {
        public LeadController(ICoeveryServices services) {
            Services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public ICoeveryServices Services { get; set; }
        //
        // GET: /Lead/
        public ActionResult Index() {
            var contentItem = Services.ContentManager.New("Lead");
            contentItem.Weld(new LeadPart());
            var model = Services.ContentManager.BuildDisplay(contentItem, "List");
            return View(model);
        }

        //
        // GET: /Lead/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Lead/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Lead/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Lead/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Lead/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Lead/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Lead/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
