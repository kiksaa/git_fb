using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            List<projectand> ProjectandList = new List<projectand>();
            using (farmdb farmdb = new farmdb())
            {
                ProjectandList = farmdb.projectands.ToList<projectand>();
                ViewBag.TotalProjectand = ProjectandList.Count();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from p in farmdb.projectands
                           join b in farmdb.buymethods on p.buyMethod equals b.ID into blist
                           from b in blist.DefaultIfEmpty()
                           join s in farmdb.standards on p.manuStandards equals s.ID into slist
                           from s in slist.DefaultIfEmpty()
                           select new
                           {
                               p.ID,
                               p.dataNow,
                               p.proName,
                               b.nameBuy,
                               s.standardName
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    objcvm.dateUpdate = item.dataNow;
                    objcvm.projectName = item.proName;
                    objcvm.nameBuy = item.nameBuy;
                    objcvm.standardName = item.standardName;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }
        public ActionResult IndexStandard(int id)
        {
            projectand projectandModel = new projectand();
            using (farmdb farmdb = new farmdb())
            {
                projectandModel = farmdb.projectands.Where(x => x.ID == id).FirstOrDefault();
                List<standardlist> standardlistModel = farmdb.standardlists.Where(s => s.IDpro == projectandModel.ID).ToList();
                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from s in farmdb.standardlists
                           where s.IDpro == id
                           select new
                           {
                               s.ID,
                               s.IDlist,
                               s.list,
                               s.fillin,
                               s.img,
                               s.IDpro
                           };

                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    objcvm.IDlist = item.IDlist;
                    objcvm.list = item.list;
                    objcvm.fillin = item.fillin;
                    objcvm.img = item.img;
                    objcvm.IDpro = item.IDpro;
                    ViewModeltList.Add(objcvm);
                }

                ViewBag.TotalStandard = data.Count();

                return View(ViewModeltList);
            }
        }
        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            projectand ProjectandList = new projectand();
            using (farmdb farmdb = new farmdb())
            {
                ProjectandList = farmdb.projectands.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(ProjectandList);
        }
        // GET: Project/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                                select new SelectListItem
                                                                {
                                                                    Text = b.nameBuy,
                                                                    Value = b.ID.ToString()
                                                                };
                ViewBag.buymethods = selbuymethods;

                List<standard> standards = farmdb.standards.ToList();
                IEnumerable<SelectListItem> selstandards = from s in standards
                                                       select new SelectListItem
                                                       {
                                                           Text = s.standardName,
                                                           Value = s.ID.ToString()
                                                       };
                ViewBag.standards = selstandards;

            }
            return View(new projectand());
        }
        
        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(projectand projectandModel)
        {
            using(farmdb farmdb = new farmdb())
            {
                farmdb.projectands.Add(projectandModel);
                projectandModel.dataNow = DateTime.Now;
                farmdb.SaveChanges();
            }
            return RedirectToAction("Create", "Standardlist", new { IDpro = projectandModel.ID });
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            projectand projectandModel = new projectand();
            using (farmdb farmdb = new farmdb())
            {
                projectandModel = farmdb.projectands.Where(x => x.ID == id).FirstOrDefault();
                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                            {
                                                                Text = b.nameBuy,
                                                                Value = b.ID.ToString()
                                                            };
                ViewBag.buymethods = selbuymethods;

                List<standard> standards = farmdb.standards.ToList();
                IEnumerable<SelectListItem> selstandards = from s in standards
                                                           select new SelectListItem
                                                           {
                                                               Text = s.standardName,
                                                               Value = s.ID.ToString()
                                                           };
                ViewBag.standards = selstandards;
            }
            return View(projectandModel);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(projectand projectandModel)
        {
            using (farmdb farmdb = new farmdb())
            {
                farmdb.Entry(projectandModel).State = System.Data.Entity.EntityState.Modified;
                projectandModel.dataNow = DateTime.Now;
                farmdb.SaveChanges();
            }
            return RedirectToAction("Edit", "Project");
        }
        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            projectand projectandModel = new projectand();
            using (farmdb farmdb = new farmdb())
            {
                projectandModel = farmdb.projectands.Where(x => x.ID == id).FirstOrDefault();
                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                            {
                                                                Text = b.nameBuy,
                                                                Value = b.ID.ToString()
                                                            };
                ViewBag.buymethods = selbuymethods;

                List<standard> standards = farmdb.standards.ToList();
                IEnumerable<SelectListItem> selstandards = from s in standards
                                                           select new SelectListItem
                                                           {
                                                               Text = s.standardName,
                                                               Value = s.ID.ToString()
                                                           };
                ViewBag.standards = selstandards;
            }
            return View(projectandModel);
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (farmdb farmdb = new farmdb())
            {
                projectand projectandModel = farmdb.projectands.Where(x => x.ID == id).FirstOrDefault();
                standardlist StandardlistModel = farmdb.standardlists.Where(s => s.IDpro == projectandModel.ID).FirstOrDefault();
                farmdb.projectands.Remove(projectandModel);
                if (StandardlistModel != null)
                {
                    farmdb.standardlists.Remove(StandardlistModel);
                }
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
