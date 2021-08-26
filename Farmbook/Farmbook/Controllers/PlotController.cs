using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
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
                List<SelectListItem> itemCountries = new List<SelectListItem>();
                landplot model = new landplot();
                var countries = (from pro in farmdb.provinces select pro).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.provinceID.ToString(),
                    Text = x.provinceName
                });
                itemCountries.AddRange(countries);
                model.ProvinceList = itemCountries;

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
                                                            Text = s.proName,
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
                return View(model);
            }
            return View(new landplot());
        }
        // POST: Plot/Create
        [HttpPost]
        public ActionResult Create(landplot plotModel, filedetail filesModel)
        {
            /*try
            {*/
            using (farmdb farmdb = new farmdb())
            {
                if (plotModel.file_license != null)
                {
                    String FileExt = Path.GetExtension(plotModel.file_license.FileName).ToUpper();
                    if (FileExt != null)
                    {
                        if (FileExt == ".PDF")
                        {
                            Byte[] data = new byte[plotModel.file_license.ContentLength];
                            plotModel.file_license.InputStream.Read(data, 0, plotModel.file_license.ContentLength);
                            filesModel.fileName = "01_" + plotModel.farmerName + plotModel.file_license.FileName;
                            filesModel.fileData = data;
                            plotModel.license_img = filesModel.fileName;
                        }
                        else
                        {
                            ViewBag.FileStatus = "Invalid file format.";
                            return View();
                        }
                    }
                    farmdb.filedetails.Add(filesModel);
                }
                    
                /*landplot land = new landplot();
                register re = new register();*/
                /*re.ID = (int)plotModel.farmerName;*/
                farmdb.landplots.Add(plotModel);
                if (plotModel.areaPlot == null)
                {
                    plotModel.areaPlot = 0;
                }
                plotModel.theoryName = 1;
                /*farmdb.landplots.Add(land);
                farmdb.registers.Add(re);*/
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index", "Register");
            /*}
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }*/
        }

        // GET: Plot/Edit/5
        public ActionResult Edit(int id)
        {
            landplot plotModel = new landplot();
            using (farmdb farmdb = new farmdb())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();

                List<SelectListItem> itemCountries = new List<SelectListItem>();
                List<SelectListItem> itemCountries2 = new List<SelectListItem>();
                List<SelectListItem> itemCountries3 = new List<SelectListItem>();
                /*register model = new register();*/
                var countries = (from pro in farmdb.provinces select pro).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.provinceID.ToString(),
                    Text = x.provinceName
                });
                itemCountries.AddRange(countries);
                plotModel.ProvinceList = itemCountries;


                var countries2 = (from amp in farmdb.amphers select amp).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.ampherID.ToString(),
                    Text = x.ampherName
                });
                itemCountries2.AddRange(countries2);
                plotModel.AmpherList = itemCountries2;

                var countries3 = (from dis in farmdb.districts select dis).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.districtID.ToString(),
                    Text = x.districtName
                });
                itemCountries3.AddRange(countries3);
                plotModel.DistrictList = itemCountries3;

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
                                                              Text = s.proName,
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
            try
            {
                filedetail filesModel = new filedetail();
                /*register registerModel = new register();*/
                using (farmdb farmdb = new farmdb())
                {
                    if(plotModel.file_license != null)
                    {
                        String FileExt = Path.GetExtension(plotModel.file_license.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PDF")
                            {
                                Byte[] data = new byte[plotModel.file_license.ContentLength];
                                plotModel.file_license.InputStream.Read(data, 0, plotModel.file_license.ContentLength);
                                filesModel.fileName = "01_" + plotModel.farmerName + plotModel.file_license.FileName;
                                filesModel.fileData = data;
                                plotModel.license_img = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    /*plotModel.farmerName = registerModel.ID;*/
                    farmdb.Entry(plotModel).State = System.Data.Entity.EntityState.Modified;
                    /*plotModel.farmerName = plotModel.farmerName;*/
                    plotModel.theoryName = 1;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Register");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
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
                                                              Text = s.proName,
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
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    landplot plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == plotModel.license_img).FirstOrDefault();
                    farmdb.landplots.Remove(plotModel);
                    farmdb.filedetails.Remove(filesModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Register");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public FileResult DownLoadFile(string name)
        {
            filedetail filesModel = new filedetail();
            using (farmdb farmdb = new farmdb())
            {
                filesModel = farmdb.filedetails.Where(x => x.fileName == name).FirstOrDefault();
            }
            return File(filesModel.fileData, "application/pdf", filesModel.fileName);
        }
    }
}
