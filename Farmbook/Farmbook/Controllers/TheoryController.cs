﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class TheoryController : Controller
    {
        // GET: Theory
        public ActionResult Index()
        {
            List<theory> theoryList = new List<theory>();
            using (farmdb farmdb = new farmdb())
            {
                theoryList = farmdb.theories.ToList<theory>();
                ViewBag.TotalTheory = theoryList.Count();
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
                               t.product,
                               t.workName,
                               a.accessName,
                               t.reference,
                               tt.theoryName
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.product = item.product;
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
        public ActionResult IndexActivity(int id)
        {
            theory theoryModel = new theory();
            using (farmdb farmdb = new farmdb())
            {
                theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();
                List<activity> activityModel = farmdb.activities.Where(a => a.plan == theoryModel.ID).ToList();
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

        // GET: Theory/Details/5
        public ActionResult Details(int id)
        {
            theory theoryModel = new theory();
            using (farmdb farmdb = new farmdb())
            {
                theoryModel = farmdb.theories.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(theoryModel);
        }

        // GET: Theory/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
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
                                                              Text = p.projectName,
                                                              Value = p.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;
            }
            return View(new theory());
        }

        // POST: Theory/Create
        [HttpPost]
        public ActionResult Create(theory theoryModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
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

        // GET: Theory/Edit/5
        public ActionResult Edit(int id)
        {
            theory theoryModel = new theory();
            using (farmdb farmdb = new farmdb())
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
                                                              Text = p.projectName,
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
                using (farmdb farmdb = new farmdb())
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

        // GET: Theory/Delete/5
        public ActionResult Delete(int id)
        {
            theory theoryModel = new theory();
            using (farmdb farmdb = new farmdb())
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
                                                              Text = p.projectName,
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
                using (farmdb farmdb = new farmdb())
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
    }
}
