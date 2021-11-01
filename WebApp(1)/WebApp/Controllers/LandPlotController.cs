using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LandPlotController : Controller
    {
        #region Index 
        // GET: LandPlot
        public ActionResult Index()
        {
            try
            {
                List<landplot> landplotrList = new List<landplot>();
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    landplotrList = farmdb.landplots.ToList<landplot>();

                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

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
                               join th in farmdb.theories on l.theoryName equals th.ID into thlist
                               from th in thlist.DefaultIfEmpty()
                               select new
                               {
                                   l.ID,
                                   l.plotName,
                                   p.provinceName,
                                   a.ampherName,
                                   d.districtName,
                                   /*l.provinceStr, l.ampherStr, l.districtStr,*/
                                   r.name,
                                   t.ownership,
                                   li.licenseName,
                                   pro.proName,
                                   s.statusName,
                                   l.areaPlot,
                                   l.areaPlotS,
                                   l.active,
                                   l.lease_img,
                                   l.license_img,
                                   th.product,
                                   th.workName,
                                   Totalarea = th.product * l.areaPlot,
                               };
                    foreach (var item in data)
                    {
                        if (item.active == 100 || item.active == null)
                        {
                            ViewLandPlot objcvm = new ViewLandPlot();
                            objcvm.ID = item.ID;
                            objcvm.plotName = item.plotName;
                            objcvm.provinceName = item.provinceName;
                            objcvm.ampherName = item.ampherName;
                            objcvm.districtName = item.districtName;
                            objcvm.name = item.name;
                            objcvm.ownership = item.ownership;
                            objcvm.licenseName = item.licenseName;
                            objcvm.lease_img = item.lease_img;
                            objcvm.license_img = item.license_img;
                            objcvm.projectName = item.proName;
                            objcvm.plotStatus = item.statusName;
                            objcvm.areaPlot = item.areaPlot;
                            objcvm.product = (float)item.Totalarea;
                            objcvm.workName = item.workName;
                            ViewModeltList.Add(objcvm);
                        }
                        ViewBag.TotalLandPlot = ViewModeltList.Count();
                        ViewBag.TotalPlot = ViewModeltList.Sum(a => a.areaPlot);
                        ViewBag.TotalTheory = ViewModeltList.Sum(p => p.product);
                    }
                    return View(ViewModeltList);
                    /*return View(landplotList);*/
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region detail 
        // GET: LandPlot/Details/5
        public ActionResult Details(int id)
        {
            landplot plotModel = new landplot();
            using (farmdbEntities farmdb = new farmdbEntities())
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

                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                            {
                                                                Text = b.nameBuy,
                                                                Value = b.ID.ToString()
                                                            };
                ViewBag.buymethods = selbuymethods;
                List<theory> theories = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from th in theories
                                                          select new SelectListItem
                                                          {
                                                              Text = th.workName,
                                                              Value = th.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;
            }
            return View(plotModel);
        }
        #endregion
        public ActionResult GetProvince()
        {
            farmdbEntities farmdb = new farmdbEntities();
            List<province> list = farmdb.provinces.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAmpher(int proID)
        {
            farmdbEntities farmdb = new farmdbEntities();
            return Json(farmdb.amphers.Where(data => data.proID == proID).Select(x => new { value = x.ampherID, text = x.ampherName })
                , JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDistrict(int amID)
        {
            farmdbEntities farmdb = new farmdbEntities();
            return Json(farmdb.districts.Where(data => data.amID == amID).Select(x => new { value = x.districtID, text = x.districtName })
                , JsonRequestBehavior.AllowGet);
        }
        #region create 
        // GET: LandPlot/Create
        public ActionResult Create()
        {
            using (farmdbEntities farmdb = new farmdbEntities())
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
                /*List<projectand> projectands = farmdb.projectands.ToList();
                IEnumerable<SelectListItem> selprojectands = from p in projectands
                                                          select new SelectListItem
                                                          {
                                                              Text = p.proName,
                                                              Value = p.ID.ToString()
                                                          };
                ViewBag.projects = selprojectands;*/
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

                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                            {
                                                                Text = b.nameBuy,
                                                                Value = b.ID.ToString()
                                                            };
                ViewBag.buymethods = selbuymethods;
                List<theory> theories = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from th in theories
                                                          select new SelectListItem
                                                          {
                                                              Text = th.workName,
                                                              Value = th.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;
                return View(model);
            }
            return View(new landplot());
        }

        // POST: LandPlot/Create
        [HttpPost]
        public ActionResult Create(landplot plotModel, filedetail filesModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
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
                    if (plotModel.file_lease != null)
                    {
                        String FileExt2 = Path.GetExtension(plotModel.file_lease.FileName).ToUpper();
                        if (FileExt2 != null)
                        {
                            if (FileExt2 == ".PDF")
                            {
                                Byte[] data = new byte[plotModel.file_lease.ContentLength];
                                plotModel.file_lease.InputStream.Read(data, 0, plotModel.file_lease.ContentLength);
                                filesModel.fileName = "02_" + plotModel.farmerName + plotModel.file_lease.FileName;
                                filesModel.fileData = data;
                                plotModel.lease_img = filesModel.fileName;
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
        #endregion

        #region IndexEdit
        public ActionResult IndexEdit(int id)
        {
            landplot plotModel = new landplot();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
                List<landplot> landplotModel = farmdb.landplots.Where(p => p.farmerName == plotModel.farmerName).ToList();

                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;

                List<ViewLandPlot> ViewModeltList = new List<ViewLandPlot>();
                var data = from l in farmdb.landplots
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
                           join b in farmdb.buymethods on l.buyMethod equals b.ID into blist
                           from b in blist.DefaultIfEmpty()
                           where l.farmerName == plotModel.farmerName
                           select new
                           {
                               l.ID,
                               r.name,
                               l.plotName, /*p.provinceName, a.ampherName, d.districtName,*/
                               l.administrator,
                               l.areaPlot,
                               li.licenseName,
                               l.titleDeed,
                               l.landNumber,
                               l.lease_img,
                               l.license_img,
                               pro.proName,
                               l.plotDetails,
                               b.nameBuy,
                               t.ownership,
                               s.statusName,
                               l.active
                           };
                foreach (var item in data)
                {
                    if (item.active == 100 || item.active == null)
                    {
                        ViewLandPlot objcvm = new ViewLandPlot();
                        objcvm.ID = item.ID;
                        objcvm.name = item.name;
                        objcvm.plotName = item.plotName;
                        objcvm.administrator = item.administrator;
                        objcvm.areaPlot = item.areaPlot;
                        objcvm.licenseName = item.licenseName;
                        objcvm.titleDeed = item.titleDeed;
                        objcvm.landNumber = item.landNumber;
                        objcvm.lease_img = item.lease_img;
                        objcvm.license_img = item.license_img;
                        objcvm.projectName = item.proName;
                        objcvm.plotDetails = item.plotDetails;
                        objcvm.nameBuy = item.nameBuy;
                        objcvm.ownership = item.ownership;
                        objcvm.plotStatus = item.statusName;
                        ViewModeltList.Add(objcvm);
                    }
                    ViewBag.LandPlot = ViewModeltList.Count();
                }
                return View(ViewModeltList);
            }
        }
        #endregion

        #region edit 
        // GET: LandPlot/Edit/5
        public ActionResult Edit(int id)
        {
            landplot plotModel = new landplot();
            using (farmdbEntities farmdb = new farmdbEntities())
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

                List<buymethod> buymethods = farmdb.buymethods.ToList();
                IEnumerable<SelectListItem> selbuymethods = from b in buymethods
                                                            select new SelectListItem
                                                            {
                                                                Text = b.nameBuy,
                                                                Value = b.ID.ToString()
                                                            };
                ViewBag.buymethods = selbuymethods;
                List<theory> theories = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from th in theories
                                                          select new SelectListItem
                                                          {
                                                              Text = th.workName,
                                                              Value = th.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;
            }
            return View(plotModel);
        }

        // POST: LandPlot/Edit/5
        [HttpPost]
        public ActionResult Edit(landplot plotModel)
        {
            try
            {
                filedetail filesModel = new filedetail();
                /*register registerModel = new register();*/
                using (farmdbEntities farmdb = new farmdbEntities())
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
                        farmdb.Entry(filesModel).State = System.Data.Entity.EntityState.Modified;
                        farmdb.filedetails.Add(filesModel);
                    }
                    if (plotModel.file_lease != null)
                    {
                        String FileExt2 = Path.GetExtension(plotModel.file_lease.FileName).ToUpper();
                        if (FileExt2 != null)
                        {
                            if (FileExt2 == ".PDF")
                            {
                                Byte[] data = new byte[plotModel.file_lease.ContentLength];
                                plotModel.file_lease.InputStream.Read(data, 0, plotModel.file_lease.ContentLength);
                                filesModel.fileName = "02_" + plotModel.farmerName + plotModel.file_lease.FileName;
                                filesModel.fileData = data;
                                plotModel.lease_img = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.Entry(filesModel).State = System.Data.Entity.EntityState.Modified;
                        farmdb.filedetails.Add(filesModel);
                    }
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
        #endregion

        #region IndexDelete
        public ActionResult IndexDelete(int id)
        {
            landplot plotModel = new landplot();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
                List<landplot> landplotModel = farmdb.landplots.Where(p => p.farmerName == plotModel.farmerName).ToList();

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
                List<theory> theories = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from th in theories
                                                          select new SelectListItem
                                                          {
                                                              Text = th.workName,
                                                              Value = th.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;

                List<ViewLandPlot> ViewModeltList = new List<ViewLandPlot>();
                var data = from l in farmdb.landplots
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
                           join b in farmdb.buymethods on l.buyMethod equals b.ID into blist
                           from b in blist.DefaultIfEmpty()
                           where l.farmerName == plotModel.farmerName
                           select new
                           {
                               l.ID,
                               r.name,
                               l.plotName, /*p.provinceName, a.ampherName, d.districtName,*/
                               l.administrator,
                               l.areaPlot,
                               li.licenseName,
                               l.titleDeed,
                               l.landNumber,
                               l.lease_img,
                               l.license_img,
                               pro.proName,
                               l.plotDetails,
                               b.nameBuy,
                               t.ownership,
                               s.statusName,

                               l.active
                           };
                foreach (var item in data)
                {
                    if (item.active == 100 || item.active == null)
                    {
                        ViewLandPlot objcvm = new ViewLandPlot();
                        objcvm.ID = item.ID;
                        objcvm.name = item.name;
                        objcvm.plotName = item.plotName;
                        objcvm.administrator = item.administrator;
                        objcvm.areaPlot = item.areaPlot;
                        objcvm.licenseName = item.licenseName;
                        objcvm.titleDeed = item.titleDeed;
                        objcvm.landNumber = item.landNumber;
                        objcvm.lease_img = item.lease_img;
                        objcvm.license_img = item.license_img;
                        objcvm.projectName = item.proName;
                        objcvm.plotDetails = item.plotDetails;
                        objcvm.nameBuy = item.nameBuy;
                        objcvm.ownership = item.ownership;
                        objcvm.plotStatus = item.statusName;
                        ViewModeltList.Add(objcvm);
                    }
                    ViewBag.LandPlot = ViewModeltList.Count();
                }
                return View(ViewModeltList);
            }
        }
        #endregion

        #region delete
        // GET: LandPlot/Delete/5
        public ActionResult Delete(int id)
        {
            landplot plotModel = new landplot();
            using (farmdbEntities farmdb = new farmdbEntities())
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
                List<theory> theories = farmdb.theories.ToList();
                IEnumerable<SelectListItem> seltheories = from th in theories
                                                          select new SelectListItem
                                                          {
                                                              Text = th.workName,
                                                              Value = th.ID.ToString()
                                                          };
                ViewBag.theories = seltheories;
            }
            return View(plotModel);
        }

        // POST: LandPlot/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    landplot plotModel = farmdb.landplots.Where(x => x.ID == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == plotModel.license_img || f.fileName == plotModel.lease_img).FirstOrDefault();
                    plotModel.active = 200;
                    if (plotModel.active == 100)
                    {
                        farmdb.landplots.Remove(plotModel);
                        farmdb.filedetails.Remove(filesModel);
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
        #endregion

        #region DownLoadFile 
        [HttpGet]
        public FileResult DownLoadFile(string name)
        {
            filedetail filesModel = new filedetail();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                filesModel = farmdb.filedetails.Where(x => x.fileName == name).FirstOrDefault();
            }
            return File(filesModel.fileData, "application/pdf", filesModel.fileName);
        }
        #endregion
    }
}