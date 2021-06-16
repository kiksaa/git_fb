using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class StandardlistController : Controller
    {
        // GET: Standardlist
        public ActionResult Index()
        {
            List<standardlist> StandardlistModel = new List<standardlist>();
            using (farmdb farmdb = new farmdb())
            {
                StandardlistModel = farmdb.standardlists.ToList<standardlist>();
            }
            return View(StandardlistModel);
        }

        // GET: Standardlist/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Standardlist/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<projectand> projectands = farmdb.projectands.ToList();
                IEnumerable<SelectListItem> selprojectands = from p in projectands
                                                            select new SelectListItem
                                                            {
                                                                Text = p.proName,
                                                                Value = p.ID.ToString()
                                                            };
                ViewBag.projectands = selprojectands;
            }
            return View(new standardlist());
        }

        // POST: Standardlist/Create
        [HttpPost]
        public ActionResult Create(standardlist StandardlistModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.standardlists.Add(StandardlistModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Project");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Standardlist/Edit/5
        public ActionResult Edit(int id)
        {
            standardlist StandardlistModel = new standardlist();
            using (farmdb farmdb = new farmdb())
            {
                StandardlistModel = farmdb.standardlists.Where(x => x.ID == id).FirstOrDefault();
                List<projectand> projectands = farmdb.projectands.ToList();
                IEnumerable<SelectListItem> selprojectands = from p in projectands
                                                             select new SelectListItem
                                                             {
                                                                 Text = p.proName,
                                                                 Value = p.ID.ToString()
                                                             };
                ViewBag.projectands = selprojectands;

            }
            return View(StandardlistModel);
        }

        // POST: Standardlist/Edit/5
        [HttpPost]
        public ActionResult Edit(standardlist StandardlistModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(StandardlistModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Project");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: Standardlist/Delete/5
        public ActionResult Delete(int id)
        {
            standardlist StandardlistModel = new standardlist();
            using (farmdb farmdb = new farmdb())
            {
                StandardlistModel = farmdb.standardlists.Where(x => x.ID == id).FirstOrDefault();
                List<projectand> projectands = farmdb.projectands.ToList();
                IEnumerable<SelectListItem> selprojectands = from p in projectands
                                                             select new SelectListItem
                                                             {
                                                                 Text = p.proName,
                                                                 Value = p.ID.ToString()
                                                             };
                ViewBag.projectands = selprojectands;
            }
            return View(StandardlistModel);
        }

        // POST: Standardlist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    standardlist StandardlistModel = farmdb.standardlists.Where(x => x.ID == id).FirstOrDefault();
                    farmdb.standardlists.Remove(StandardlistModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Project");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
           
        }
    }
}
