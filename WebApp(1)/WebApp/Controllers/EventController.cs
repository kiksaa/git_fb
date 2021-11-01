using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EventController : Controller
    {

        #region Index
        // GET: Event
        public ActionResult Index()
        {
            try
            {
                List<@event> EventList = new List<@event>();
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

                    EventList = farmdb.events.ToList<@event>();
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
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion
        #region Details
        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            @event eventModel = new @event();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                eventModel = farmdb.events.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(eventModel);
        }
        #endregion
        #region Create
        // GET: Event/Create
        public ActionResult Create()
        {
            return View(new @event());
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(@event eventModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.events.Add(eventModel);
                    /*eventModel.themeColor = User.Identity.Name;*/
                    //themeColor -> responsible AND change length 10 -> 100
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Event");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Edit
        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            @event eventModel = new @event();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                eventModel = farmdb.events.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(eventModel);
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(@event eventModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.Entry(eventModel).State = System.Data.Entity.EntityState.Modified;
                    /*eventModel.themeColor = User.Identity.Name;*/
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Event");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
        #region Delete
        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            @event eventModel = new @event();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                eventModel = farmdb.events.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(eventModel);
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
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
        #endregion
    }
}