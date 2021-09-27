using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProjectController : Controller
    {
        #region Index
        // GET: Project
        public ActionResult Index()
        {
            try
            {
                List<project> ProjectList = new List<project>();
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    ProjectList = farmdb.projects.ToList<project>();
                    ViewBag.TotalProjectand = ProjectList.Count();

                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

                    List<ViewModel> ViewModeltList = new List<ViewModel>();
                    var data = from p in farmdb.projects
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
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion
        #region IndexStandard
        public ActionResult IndexStandard(int id)
        {
            project projectModel = new project();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                projectModel = farmdb.projects.Where(x => x.ID == id).FirstOrDefault();
                List<standardlist> standardlistModel = farmdb.standardlists.Where(s => s.IDpro == projectModel.ID).ToList();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

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
                    objcvm.fillin = (bool)item.fillin;
                    objcvm.img = (bool)item.img;
                    objcvm.IDpro = item.IDpro;
                    ViewModeltList.Add(objcvm);
                }

                ViewBag.TotalStandard = data.Count();

                return View(ViewModeltList);
            }
        }
        #endregion
        #region Details
        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            project ProjectList = new project();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                ProjectList = farmdb.projects.Where(x => x.ID == id).FirstOrDefault();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

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
            return View(ProjectList);
        }
        #endregion
        #region Create
        // GET: Project/Create
        public ActionResult Create()
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
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
                return View(new project());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(project projectModel)
        {
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                farmdb.projects.Add(projectModel);
                projectModel.dataNow = DateTime.Now;
                farmdb.SaveChanges();
            }
            return RedirectToAction("Create", "Standardlist", new { IDpro = projectModel.ID });
        }
        #endregion
        #region Edit
        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            project projectModel = new project();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                projectModel = farmdb.projects.Where(x => x.ID == id).FirstOrDefault();
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
            return View(projectModel);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(project projectModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.Entry(projectModel).State = System.Data.Entity.EntityState.Modified;
                    projectModel.dataNow = DateTime.Now;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Project");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Delete
        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            project projectModel = new project();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                projectModel = farmdb.projects.Where(x => x.ID == id).FirstOrDefault();
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
            return View(projectModel);
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    project projectModel = farmdb.projects.Where(x => x.ID == id).FirstOrDefault();
                    standardlist StandardlistModel = farmdb.standardlists.Where(s => s.IDpro == projectModel.ID).FirstOrDefault();
                    farmdb.projects.Remove(projectModel);
                    if (StandardlistModel != null)
                    {
                        farmdb.standardlists.Remove(StandardlistModel);
                    }
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}