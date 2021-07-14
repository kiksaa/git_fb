using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class EventController : Controller
    {
        // GET: Labor
        public ActionResult Index()
        {
            List<@event> EventtList = new List<@event>();
            using (farmdb farmdb = new farmdb())
            {
                EventtList = farmdb.events.ToList<@event>();
                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from e in farmdb.events
                           select new
                           {
                               e.ID,
                               e.subject,
                               e.start,
                               e.end,
                               e.description,
                               e.isFullDay,
                               e.themeColor
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    objcvm.subject = item.subject;
                    objcvm.start = item.start;
                    objcvm.end = item.end;
                    objcvm.description = item.description;
                    objcvm.isFullDay = item.isFullDay;
                    objcvm.themeColor = item.themeColor;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }

        // GET: Labor/Details/5
        public ActionResult Details(int id)
        {
            @event eventModel = new @event();
            using (farmdb farmdb = new farmdb())
            {
                eventModel = farmdb.events.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(eventModel);
        }

        // GET: Labor/Create
        public ActionResult Create()
        {
            return View(new @event());
        }

        // POST: Labor/Create
        [HttpPost]
        public ActionResult Create(@event eventModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.events.Add(eventModel);
                    eventModel.themeColor = User.Identity.Name;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Event");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Labor/Edit/5
        public ActionResult Edit(int id)
        {
            @event eventModel = new @event();
            using (farmdb farmdb = new farmdb())
            {
                eventModel = farmdb.events.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(eventModel);
        }

        // POST: Labor/Edit/5
        [HttpPost]
        public ActionResult Edit(@event eventModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(eventModel).State = System.Data.Entity.EntityState.Modified;
                    eventModel.themeColor = User.Identity.Name;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Event");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Labor/Delete/5
        public ActionResult Delete(int id)
        {
            @event eventModel = new @event();
            using (farmdb farmdb = new farmdb())
            {
                eventModel = farmdb.events.Where(x => x.ID== id).FirstOrDefault();
            }
            return View(eventModel);
        }

        // POST: Labor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    @event eventModel = farmdb.events.Where(x => x.ID == id).FirstOrDefault();
                    farmdb.events.Remove(eventModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Event");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
