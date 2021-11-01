﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TheoryController : Controller
    {
        #region Index
        // GET: Theory
        public ActionResult Index()
        {
            try
            {
                List<theory> theoryList = new List<theory>();
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    theoryList = farmdb.theories.ToList<theory>();
                    ViewBag.TotalTheory = theoryList.Count();

                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

                    List<ViewModel> ViewModeltList = new List<ViewModel>();
                    var data = from t in farmdb.theories
                               join tt in farmdb.theorytypes on t.workProcedure equals tt.theoryID into tlist
                               from tt in tlist.DefaultIfEmpty()
                               join a in farmdb.accesses on t.access equals a.accessID into alist
                               from a in alist.DefaultIfEmpty()
                               select new
                               {
                                   t.ID,
                                   t.dateUpdate,
                                   t.productType,
                                   t.workName,
                                   a.accessName,
                                   t.reference,
                                   tt.theoryName
                               };
                    foreach (var item in data)
                    {
                        ViewModel objcvm = new ViewModel();
                        objcvm.product = item.productType;
                        objcvm.workName = item.workName;
                        objcvm.accessName = item.accessName;
                        objcvm.reference = item.reference;
                        objcvm.theoryName = item.theoryName;
                        objcvm.dateUpdate = item.dateUpdate;
                        objcvm.ID = item.ID;
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
        #region IndexActivity
        public ActionResult IndexActivity(int id)
        {
            try
            {
                theory theoryModel = new theory();
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();
                    List<activity> activityModel = farmdb.activities.Where(a => a.plan == theoryModel.ID).ToList();

                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

                    List<ViewModel> ViewModeltList = new List<ViewModel>();
                    var data = from a in farmdb.activities
                               where a.plan == id
                               select new
                               {
                                   a.ID,
                                   a.stepNum,
                                   a.stepName,
                                   a.age,
                                   a.time,
                                   a.activity1,
                                   a.notice
                               };

                    foreach (var item in data)
                    {
                        ViewModel objcvm = new ViewModel();
                        objcvm.ID = item.ID;
                        objcvm.stepNum = item.stepNum;
                        objcvm.stepName = item.stepName;
                        objcvm.age = item.age;
                        objcvm.time = item.time;
                        objcvm.activity1 = item.activity1;
                        objcvm.notice = item.notice;
                        ViewModeltList.Add(objcvm);
                    }
                    ViewBag.TotalActivity = data.Count();
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
        // GET: Theory/Details/5
        public ActionResult Details(int id)
        {
            theory theoryModel = new theory();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();
                List<theorytype> theorytypes = farmdb.theorytypes.ToList();
                IEnumerable<SelectListItem> seltheorytypes = from l in theorytypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = l.theoryName,
                                                                 Value = l.theoryID.ToString()
                                                             };
                ViewBag.theorytypes = seltheorytypes;

                List<access> accesses = farmdb.accesses.ToList();
                IEnumerable<SelectListItem> selaccesses = from p in accesses
                                                          select new SelectListItem
                                                          {
                                                              Text = p.accessName,
                                                              Value = p.accessID.ToString()
                                                          };
                ViewBag.accesses = selaccesses;
                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from p in projects
                                                          select new SelectListItem
                                                          {
                                                              Text = p.proName,
                                                              Value = p.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;
            }
            return View(theoryModel);
        }
        #endregion
        #region Create
        // GET: Theory/Create
        public ActionResult Create()
        {
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                List<theorytype> theorytypes = farmdb.theorytypes.ToList();
                IEnumerable<SelectListItem> seltheorytypes = from l in theorytypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = l.theoryName,
                                                                 Value = l.theoryID.ToString()
                                                             };
                ViewBag.theorytypes = seltheorytypes;

                List<access> accesses = farmdb.accesses.ToList();
                IEnumerable<SelectListItem> selaccesses = from p in accesses
                                                          select new SelectListItem
                                                          {
                                                              Text = p.accessName,
                                                              Value = p.accessID.ToString()
                                                          };
                ViewBag.accesses = selaccesses;
                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from p in projects
                                                          select new SelectListItem
                                                          {
                                                              Text = p.proName,
                                                              Value = p.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;
                /* List<projectand> projectands = farmdb.projectands.ToList();
                 IEnumerable<SelectListItem> selprojectands = from p in projectands
                                                              select new SelectListItem
                                                              {
                                                                  Text = p.proName,
                                                                  Value = p.ID.ToString()
                                                              };
                 ViewBag.projects = selprojectands;*/
            }
            return View(new theory());
        }

        // POST: Theory/Create
        [HttpPost]
        public ActionResult Create(theory theoryModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.theories.Add(theoryModel);
                    theoryModel.dateUpdate = DateTime.Now;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Create", "Activity", new { plan = theoryModel.ID });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Edit
        // GET: Theory/Edit/5
        public ActionResult Edit(int id)
        {
            theory theoryModel = new theory();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();

                List<theorytype> theorytypes = farmdb.theorytypes.ToList();
                IEnumerable<SelectListItem> seltheorytypes = from l in theorytypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = l.theoryName,
                                                                 Value = l.theoryID.ToString()
                                                             };
                ViewBag.theorytypes = seltheorytypes;

                List<access> accesses = farmdb.accesses.ToList();
                IEnumerable<SelectListItem> selaccesses = from p in accesses
                                                          select new SelectListItem
                                                          {
                                                              Text = p.accessName,
                                                              Value = p.accessID.ToString()
                                                          };
                ViewBag.accesses = selaccesses;
                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from p in projects
                                                          select new SelectListItem
                                                          {
                                                              Text = p.proName,
                                                              Value = p.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;
            }
            return View(theoryModel);
        }

        // POST: Theory/Edit/5
        [HttpPost]
        public ActionResult Edit(theory theoryModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.Entry(theoryModel).State = System.Data.Entity.EntityState.Modified;
                theoryModel.dateUpdate = DateTime.Now;
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
        #region Delete
        // GET: Theory/Delete/5
        public ActionResult Delete(int id)
        {
            theory theoryModel = new theory();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();

                List<theorytype> theorytypes = farmdb.theorytypes.ToList();
                IEnumerable<SelectListItem> seltheorytypes = from l in theorytypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = l.theoryName,
                                                                 Value = l.theoryID.ToString()
                                                             };
                ViewBag.theorytypes = seltheorytypes;

                List<access> accesses = farmdb.accesses.ToList();
                IEnumerable<SelectListItem> selaccesses = from p in accesses
                                                          select new SelectListItem
                                                          {
                                                              Text = p.accessName,
                                                              Value = p.accessID.ToString()
                                                          };
                ViewBag.accesses = selaccesses;
                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from p in projects
                                                          select new SelectListItem
                                                          {
                                                              Text = p.proName,
                                                              Value = p.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;
            }
            return View(theoryModel);
        }

        // POST: Theory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    theory theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();
                    activity activityModel = farmdb.activities.Where(a => a.plan == theoryModel.ID).FirstOrDefault();
                    farmdb.theories.Remove(theoryModel);
                    if (activityModel != null)
                    {
                        farmdb.activities.Remove(activityModel);
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