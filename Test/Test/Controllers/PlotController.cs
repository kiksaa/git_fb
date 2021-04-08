using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class PlotController : Controller
    {
        // GET: Plot
        public ActionResult Index()
        {
            /*List<landplot> landplotList = new List<landplot>();*/
            using (farmdb farmdb = new farmdb())
            {
                /*landplotList = (from l in farmdb.landplots 
                                join r in farmdb.registers 
                                on l.plotName equals r.name 
                                where l.plotName == r.name
                                select l).ToList();*/

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from l in farmdb.landplots
                           join p in farmdb.provinces on l.province equals p.provinceID into plist
                           from p in plist.DefaultIfEmpty()
                           join a in farmdb.amphers on l.ampher equals a.ampherID into alist
                           from a in alist.DefaultIfEmpty()
                           join d in farmdb.districts on l.district equals d.districtID into dlist
                           from d in dlist.DefaultIfEmpty()
                           join r in farmdb.registers on l.farmerName equals r.ID into rlist
                           from r in rlist.DefaultIfEmpty()
                           select new
                           {
                               l.ID,
                               l.plotName,
                               p.provinceName,
                               a.ampherName,
                               d.districtName,
                               r.name
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    objcvm.plotName = item.plotName;
                    objcvm.provinceName = item.provinceName;
                    objcvm.ampherName = item.ampherName;
                    objcvm.districtName = item.districtName;
                    objcvm.name = item.name;
                    ViewModeltList.Add(objcvm);

                }
                return View(ViewModeltList);
                /*return View(landplotList);*/
            }
        }  

        // GET: Plot/Details/5
        public ActionResult Details(int id)
        {
            landplot plotModel = new landplot();
            using (farmdb farmdb = new farmdb())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(plotModel);
        }

        // GET: Plot/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<typeownership> typeownerships = farmdb.typeownerships.ToList();
                IEnumerable<SelectListItem> seltypeownerships = from t in typeownerships
                                                                select new SelectListItem
                                                           {
                                                               Text = t.ownership,
                                                               Value = t.ID.ToString()
                                                           };
                ViewBag.typeownerships = seltypeownerships;

                List<license> licenses = farmdb.licenses.ToList();
                IEnumerable<SelectListItem> sellicenses = from t in licenses
                                                          select new SelectListItem
                                                                {
                                                                    Text = t.licenseName,
                                                                    Value = t.ID.ToString()
                                                                };
                ViewBag.licenses = sellicenses;

                List<province> provinces = farmdb.provinces.ToList();
                IEnumerable<SelectListItem> selprovinces = from p in provinces
                                                           select new SelectListItem
                                                           {
                                                               Text = p.provinceName,
                                                               Value = p.provinceID.ToString()
                                                           };
                ViewBag.provinces = selprovinces;

                List<ampher> amphers = farmdb.amphers.ToList();
                IEnumerable<SelectListItem> selamphers = from a in amphers
                                                         select new SelectListItem
                                                         {
                                                             Text = a.ampherName,
                                                             Value = a.ampherID.ToString()
                                                         };
                ViewBag.amphers = selamphers;

                List<district> districts = farmdb.districts.ToList();
                IEnumerable<SelectListItem> seldistricts = from d in districts
                                                           select new SelectListItem
                                                           {
                                                               Text = d.districtName,
                                                               Value = d.districtID.ToString()
                                                           };
                ViewBag.districts = seldistricts;

                List<status> status = farmdb.status.ToList();
                if (User.Identity.Name == "admin")
                {
                    IEnumerable<SelectListItem> selstatus = from s in status
                                                            select new SelectListItem
                                                            {
                                                                Text = s.statusName,
                                                                Value = s.statusID.ToString()
                                                            };
                    ViewBag.status = selstatus;
                }
                else
                {
                    IEnumerable<SelectListItem> selstatuss = from s in status
                                                             select new SelectListItem
                                                             {
                                                                 Text = s.statusName,
                                                                 Value = "400"
                                                             };
                    ViewBag.status = selstatuss;
                }

                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from s in projects
                                                          select new SelectListItem
                                                        {
                                                            Text = s.projectName,
                                                            Value = s.ID.ToString()
                                                        };
                ViewBag.projects = selprojects;

                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;

            }
            return View(new landplot());
        }

        // POST: Plot/Create
        [HttpPost]
        public ActionResult Create(landplot plotModel)
        {
            using (farmdb farmdb = new farmdb())
            {
                /*landplot land = new landplot();
                register re = new register();*/

                /*re.ID = (int)plotModel.farmerName;*/

                farmdb.landplots.Add(plotModel);
                /*farmdb.landplots.Add(land);
                farmdb.registers.Add(re);*/
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index", "Register");
        }

        // GET: Plot/Edit/5
        public ActionResult Edit(int id)
        {
            landplot plotModel = new landplot();
            using (farmdb farmdb = new farmdb())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();

                List<typeownership> typeownerships = farmdb.typeownerships.ToList();
                IEnumerable<SelectListItem> seltypeownerships = from t in typeownerships
                                                                select new SelectListItem
                                                                {
                                                                    Text = t.ownership,
                                                                    Value = t.ID.ToString()
                                                                };
                ViewBag.typeownerships = seltypeownerships;

                List<license> licenses = farmdb.licenses.ToList();
                IEnumerable<SelectListItem> sellicenses = from t in licenses
                                                          select new SelectListItem
                                                          {
                                                              Text = t.licenseName,
                                                              Value = t.ID.ToString()
                                                          };
                ViewBag.licenses = sellicenses;

                List<province> provinces = farmdb.provinces.ToList();
                IEnumerable<SelectListItem> selprovinces = from p in provinces
                                                           select new SelectListItem
                                                           {
                                                               Text = p.provinceName,
                                                               Value = p.provinceID.ToString()
                                                           };
                ViewBag.provinces = selprovinces;

                List<ampher> amphers = farmdb.amphers.ToList();
                IEnumerable<SelectListItem> selamphers = from a in amphers
                                                         select new SelectListItem
                                                         {
                                                             Text = a.ampherName,
                                                             Value = a.ampherID.ToString()
                                                         };
                ViewBag.amphers = selamphers;

                List<district> districts = farmdb.districts.ToList();
                IEnumerable<SelectListItem> seldistricts = from d in districts
                                                           select new SelectListItem
                                                           {
                                                               Text = d.districtName,
                                                               Value = d.districtID.ToString()
                                                           };
                ViewBag.districts = seldistricts;

                List<status> status = farmdb.status.ToList();
                if (User.Identity.Name == "admin")
                {
                    IEnumerable<SelectListItem> selstatus = from s in status
                                                            select new SelectListItem
                                                            {
                                                                Text = s.statusName,
                                                                Value = s.statusID.ToString()
                                                            };
                    ViewBag.status = selstatus;
                }
                else
                {
                    IEnumerable<SelectListItem> selstatuss = from s in status
                                                             select new SelectListItem
                                                             {
                                                                 Text = s.statusName,
                                                                 Value = "400"
                                                             };
                    ViewBag.status = selstatuss;
                }

                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from s in projects
                                                          select new SelectListItem
                                                          {
                                                              Text = s.projectName,
                                                              Value = s.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;

                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                          {
                                                              Text = r.name,
                                                              Value = r.ID.ToString()
                                                          };
                ViewBag.registers = selregisters;
            }
            return View(plotModel);
        }

        // POST: Plot/Edit/5
        [HttpPost]
        public ActionResult Edit(landplot plotModel)
        {
            /*register registerModel = new register();*/
            using (farmdb farmdb = new farmdb())
            {
                /*plotModel.farmerName = registerModel.ID;*/
                farmdb.Entry(plotModel).State = System.Data.Entity.EntityState.Modified;
                /*plotModel.farmerName = plotModel.farmerName;*/
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index", "Register");
        }

        // GET: Plot/Delete/5
        public ActionResult Delete(int id)
        {
            landplot plotModel = new landplot();
            using (farmdb farmdb = new farmdb())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();

                List<typeownership> typeownerships = farmdb.typeownerships.ToList();
                IEnumerable<SelectListItem> seltypeownerships = from t in typeownerships
                                                                select new SelectListItem
                                                                {
                                                                    Text = t.ownership,
                                                                    Value = t.ID.ToString()
                                                                };
                ViewBag.typeownerships = seltypeownerships;

                List<license> licenses = farmdb.licenses.ToList();
                IEnumerable<SelectListItem> sellicenses = from t in licenses
                                                          select new SelectListItem
                                                          {
                                                              Text = t.licenseName,
                                                              Value = t.ID.ToString()
                                                          };
                ViewBag.licenses = sellicenses;

                List<province> provinces = farmdb.provinces.ToList();
                IEnumerable<SelectListItem> selprovinces = from p in provinces
                                                           select new SelectListItem
                                                           {
                                                               Text = p.provinceName,
                                                               Value = p.provinceID.ToString()
                                                           };
                ViewBag.provinces = selprovinces;

                List<ampher> amphers = farmdb.amphers.ToList();
                IEnumerable<SelectListItem> selamphers = from a in amphers
                                                         select new SelectListItem
                                                         {
                                                             Text = a.ampherName,
                                                             Value = a.ampherID.ToString()
                                                         };
                ViewBag.amphers = selamphers;

                List<district> districts = farmdb.districts.ToList();
                IEnumerable<SelectListItem> seldistricts = from d in districts
                                                           select new SelectListItem
                                                           {
                                                               Text = d.districtName,
                                                               Value = d.districtID.ToString()
                                                           };
                ViewBag.districts = seldistricts;

                List<status> status = farmdb.status.ToList();
                IEnumerable<SelectListItem> selstatus = from s in status
                                                        select new SelectListItem
                                                        {
                                                            Text = s.statusName,
                                                            Value = s.statusID.ToString()
                                                        };
                ViewBag.status = selstatus;

                List<project> projects = farmdb.projects.ToList();
                IEnumerable<SelectListItem> selprojects = from s in projects
                                                          select new SelectListItem
                                                          {
                                                              Text = s.projectName,
                                                              Value = s.ID.ToString()
                                                          };
                ViewBag.projects = selprojects;

                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;
            }
            return View(plotModel);
        }

        // POST: Plot/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (farmdb farmdb = new farmdb())
            {
                landplot plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
                farmdb.landplots.Remove(plotModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index", "Register");
        }
    }
}
