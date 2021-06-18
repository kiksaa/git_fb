using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class LandPlotController : Controller
    {
        // GET: LandPlot
        public ActionResult Index()
        {
            List<landplot> landplotrList = new List<landplot>();
            using (farmdb farmdb = new farmdb())
            {
                landplotrList = farmdb.landplots.ToList<landplot>();
               /* ViewBag.TotalLandPlot = landplotrList.Count();
                ViewBag.TotalPlot = landplotrList.Sum(a => a.areaPlot);*/

                List<ViewLandPlot> ViewModeltList = new List<ViewLandPlot>();
                var data = from l in farmdb.landplots
                           join p in farmdb.provinces on l.province equals p.provinceID into plist
                           from p in plist.DefaultIfEmpty()
                           join a in farmdb.amphers on l.ampher equals a.ampherID into alist
                           from a in alist.DefaultIfEmpty()
                           join d in farmdb.districts on l.district equals d.districtID into dlist
                           from d in dlist.DefaultIfEmpty()
                           join r in farmdb.registers on l.farmerName equals r.ID into rlist
                           from r in rlist.DefaultIfEmpty()
                           join t in farmdb.typeownerships on l.typeOwnership equals t.ID into tlist
                           from t in tlist.DefaultIfEmpty()
                           join li in farmdb.licenses on l.license equals li.ID into lilist
                           from li in lilist.DefaultIfEmpty()
                           join pro in farmdb.projects on l.projectName equals pro.ID into prolist
                           from pro in prolist.DefaultIfEmpty()
                           join s in farmdb.status on l.plotStatus equals s.statusID into slist
                           from s in slist.DefaultIfEmpty()
                           select new
                           {
                               l.ID, l.plotName, /*p.provinceName, a.ampherName, d.districtName,*/
                               l.provinceStr,l.ampherStr,l.districtStr,r.name,
                               t.ownership, li.licenseName, pro.projectName, s.statusName, l.areaPlot, l.active
                           };
                foreach (var item in data)
                {
                    if (item.active == 100 || item.active == null)
                    {
                        ViewLandPlot objcvm = new ViewLandPlot();
                        objcvm.ID = item.ID;
                        objcvm.plotName = item.plotName;
                        objcvm.provinceName = item.provinceStr;
                        objcvm.ampherName = item.ampherStr;
                        objcvm.districtName = item.districtStr;
                        objcvm.name = item.name;
                        objcvm.ownership = item.ownership;
                        objcvm.licenseName = item.licenseName;
                        objcvm.projectName = item.projectName;
                        objcvm.plotStatus = item.statusName;
                        objcvm.areaPlot = item.areaPlot;
                        ViewModeltList.Add(objcvm);
                    }
                    ViewBag.TotalLandPlot = ViewModeltList.Count();
                    ViewBag.TotalPlot = ViewModeltList.Sum(a => a.areaPlot);

                }
                return View(ViewModeltList);
                /*return View(landplotList);*/
            }
        }

        // GET: LandPlot/Details/5
        public ActionResult Details(int id)
        {
            landplot plotModel = new landplot();
            using (farmdb farmdb = new farmdb())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(plotModel);
        }

        // GET: LandPlot/Create
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
                /*if (User.Identity.Name == "admin")
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
                }*/
                IEnumerable<SelectListItem> selstatuss = from s in status
                                                         select new SelectListItem
                                                         {
                                                             Text = s.statusName,
                                                             Value = s.statusID.ToString()
                                                         };
                ViewBag.status = selstatuss;
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

                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                           {
                                                               Text = b.nameBuy,
                                                               Value = b.ID.ToString()
                                                           };
                ViewBag.buymethods = selbuymethods;

            }
            return View(new landplot());
        }

        // POST: LandPlot/Create
        [HttpPost]
        public ActionResult Create(landplot plotModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    /*landplot land = new landplot();
                    register re = new register();*/

                    /*re.ID = (int)plotModel.farmerName;*/

                    farmdb.landplots.Add(plotModel);
                    /*farmdb.landplots.Add(land);
                    farmdb.registers.Add(re);*/
                    plotModel.active = 100;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: LandPlot/Edit/5
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
                /* if (User.Identity.Name == "admin")
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
                 }*/
                IEnumerable<SelectListItem> selstatuss = from s in status
                                                         select new SelectListItem
                                                         {
                                                             Text = s.statusName,
                                                             Value = s.statusID.ToString()
                                                         };
                ViewBag.status = selstatuss;

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

                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                            {
                                                                Text = b.nameBuy,
                                                                Value = b.ID.ToString()
                                                            };
                ViewBag.buymethods = selbuymethods;
            }
            return View(plotModel);
        }

        // POST: LandPlot/Edit/5
        [HttpPost]
        public ActionResult Edit(landplot plotModel)
        {
            try
            {
                /*register registerModel = new register();*/
                using (farmdb farmdb = new farmdb())
                {
                    /*plotModel.farmerName = registerModel.ID;*/
                    farmdb.Entry(plotModel).State = System.Data.Entity.EntityState.Modified;
                    /*plotModel.farmerName = plotModel.farmerName;*/
                    plotModel.active = 100;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: LandPlot/Delete/5
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
            }
            return View(plotModel);
        }

        // POST: LandPlot/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    landplot plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
                    plotModel.active = 200;
                    if (plotModel.active == 100)
                    {
                        farmdb.landplots.Remove(plotModel);
                    }
                    plotModel.active = 200;
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
