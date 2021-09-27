using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ActivityController : Controller
    {
        #region Index
        // GET: Standardlist
        public ActionResult Index()
        {
            List<activity> ActivitytModel = new List<activity>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                ActivitytModel = farmdb.activities.ToList<activity>();
            }
            return View(ActivitytModel);
        }
        #endregion
        #region Details
        // GET: Standardlist/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion
        #region Create
        // GET: Standardlist/Create
        public ActionResult Create()
        {
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                List<theory> projectands = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from t in projectands
                                                          select new SelectListItem
                                                          {
                                                              Text = t.workName,
                                                              Value = t.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;

            }
            return View(new activity());
        }
        // POST: Standardlist/Create
        [HttpPost]
        public ActionResult Create(activity ActivitytModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.activities.Add(ActivitytModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Theory");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Edit
        // GET: Standardlist/Edit/5
        public ActionResult Edit(int id)
        {
            activity ActivitytModel = new activity();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                ActivitytModel = farmdb.activities.Where(x => x.ID == id).FirstOrDefault();
                List<theory> projectands = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from t in projectands
                                                          select new SelectListItem
                                                          {
                                                              Text = t.workName,
                                                              Value = t.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;

            }
            return View(ActivitytModel);
        }

        // POST: Standardlist/Edit/5
        [HttpPost]
        public ActionResult Edit(activity ActivitytModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.Entry(ActivitytModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Theory");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Delete

        // GET: Standardlist/Delete/5
        public ActionResult Delete(int id)
        {
            activity ActivitytModel = new activity();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                ActivitytModel = farmdb.activities.Where(x => x.ID == id).FirstOrDefault();
                List<theory> projectands = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from t in projectands
                                                          select new SelectListItem
                                                          {
                                                              Text = t.workName,
                                                              Value = t.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;
            }
            return View(ActivitytModel);
        }

        // POST: Standardlist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    activity ActivitytModel = farmdb.activities.Where(x => x.ID == id).FirstOrDefault();
                    farmdb.activities.Remove(ActivitytModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Theory");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}