using Devart.Data.MySql;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;
using Farmbook.Data;

namespace Farmbook.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            try
            {
                register model = new register();
                List<register> registerList = new List<register>();
                /*for(int i = 1; i <= registerList.Count(); i++)
                {

                }*/
                using (farmdb farmdb = new farmdb())
                {
                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

                    registerList = farmdb.registers.ToList<register>();
                    /*ViewBag.TotalRegister = registerList.Count();*/
                    List<ViewModel> ViewModeltList = new List<ViewModel>();
                    var data = from r in farmdb.registers
                               join p in farmdb.provinces on r.province equals p.provinceID into plist
                               from p in plist.DefaultIfEmpty()
                               join a in farmdb.amphers on r.ampher equals a.ampherID into alist
                               from a in alist.DefaultIfEmpty()
                               join d in farmdb.districts on r.district equals d.districtID into dlist
                               from d in dlist.DefaultIfEmpty()
                               join s in farmdb.status on r.status equals s.statusID into slist
                               from s in slist.DefaultIfEmpty()
                               join b in farmdb.bankusers on r.bank equals b.ID into blist
                               from b in blist.DefaultIfEmpty()
                               join l in farmdb.landplots on r.ID equals l.farmerName into llist 
                               from l in llist.DefaultIfEmpty()
                               select new
                               {
                                   r.ID, r.name, r.registerID, r.cardID, r.no, r.moo, r.road,
                                   p.provinceName,
                                   a.ampherName,
                                   d.districtName,s.statusName, r.dateUpdate, r.adminBy, r.active,
                                   TotalLandplot = llist.Where(a => a.active == null || a.active == 100).Count(),
                                   /*TotalLandplot = llist.Count(),*/
                                   Totalarea = llist.Where(a => a.active == null || a.active == 100).Sum(ll => ll.areaPlot),
                                   /*Totalarea = llist.Sum(ll => ll.areaPlot),*/
                                   Total = r.gender
                               };
                    foreach (var item in data.Distinct())
                    {
                        if (item.active == 100 || item.active == null)
                        {
                            ViewModel objcvm = new ViewModel();
                            objcvm.ID = item.ID;
                            objcvm.name = item.name;
                            objcvm.registerID = item.registerID;
                            objcvm.cardID = item.cardID;
                            objcvm.no = item.no;
                            objcvm.moo = item.moo;
                            objcvm.road = item.road;
                            objcvm.provinceName = item.provinceName;
                            objcvm.ampherName = item.ampherName;
                            objcvm.districtName = item.districtName;
                            objcvm.statusName = item.statusName;
                            objcvm.dateUpdate = item.dateUpdate;
                            objcvm.areaNumber = item.TotalLandplot;
                            objcvm.areaPlot = item.Totalarea;
                            objcvm.adminBy = item.adminBy;
                            ViewModeltList.Add(objcvm);
                        }
                        ViewBag.TotalRegister = ViewModeltList.Count();
                    }
                    return View(ViewModeltList);
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult IndexPlot(int id)
        {
            register registerModel = new register();
            using (farmdb farmdb = new farmdb())
            {
                registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();
                List<landplot> landplotModel = farmdb.landplots.Where(p => p.farmerName == registerModel.ID).ToList();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

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
                           join pro in farmdb.projects on l.projectName equals pro.ID into prolist
                           from pro in prolist.DefaultIfEmpty()
                           where l.farmerName == id
                           select new
                           {
                               l.ID,
                               p.provinceName,
                               a.ampherName,
                               d.districtName,l.plotName,pro.proName,l.active
                           };
                foreach (var item in data)
                {
                    if (item.active == 100 || item.active == null)
                    {
                        ViewModel objcvm = new ViewModel();
                        objcvm.ID = item.ID;
                        objcvm.provinceName = item.provinceName;
                        objcvm.ampherName = item.ampherName;
                        objcvm.districtName = item.districtName;
                        objcvm.plotName = item.plotName;
                        objcvm.projectName = item.proName;
                        ViewModeltList.Add(objcvm);
                    }
                }
                ViewBag.TotalLandplot = data.Count();
                return View(ViewModeltList);
            }
        }
        // GET: Register/Details/5
        public ActionResult Details(int id)
        {
            register registerModel = new register();
            using (farmdb farmdb = new farmdb())
            {
                registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();

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
                registerModel.ProvinceList = itemCountries;


                var countries2 = (from amp in farmdb.amphers select amp).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.ampherID.ToString(),
                    Text = x.ampherName
                });
                itemCountries2.AddRange(countries2);
                registerModel.AmpherList = itemCountries2;

                var countries3 = (from dis in farmdb.districts select dis).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.districtID.ToString(),
                    Text = x.districtName
                });
                itemCountries3.AddRange(countries3);
                registerModel.DistrictList = itemCountries3;
            }
            return View(registerModel);
        }
        public ActionResult GetProvince()
        {
            farmdb farmdb = new farmdb();
            List<province> list = farmdb.provinces.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAmpher(int proID)
        {
            farmdb farmdb = new farmdb();
            return Json(farmdb.amphers.Where(data => data.proID == proID).Select(x => new { value = x.ampherID, text = x.ampherName })
                , JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDistrict(int amID)
        {
            farmdb farmdb = new farmdb();
            return Json(farmdb.districts.Where(data => data.amID == amID).Select(x => new { value = x.districtID, text = x.districtName })
                , JsonRequestBehavior.AllowGet);
        }
        // GET: Register/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<SelectListItem> itemCountries = new List<SelectListItem>();
                register model = new register();
                var countries = (from pro in farmdb.provinces select pro).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.provinceID.ToString(),
                    Text = x.provinceName
                });
                itemCountries.AddRange(countries);
                model.ProvinceList = itemCountries;
                
                /*List<status> status = farmdb.status.ToList();
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
                                                                 Value = "100"
                                                             };
                    ViewBag.status = selstatuss;
                }*/

                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;
                return View(model);
            }
            
            return View(new register());
        }
        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(register registerModel, HttpPostedFileBase farmer_img, HttpPostedFileBase card_img)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    string folderPath = Server.MapPath("~/Content/img/upload/farmer/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    if (farmer_img != null && farmer_img.ContentLength > 0)
                    {
                        if (farmer_img.ContentType == "image/jpeg" || farmer_img.ContentType == "image/jpg" || farmer_img.ContentType == "image/png")
                        {
                            var fileName = Path.GetFileName(farmer_img.FileName);
                            var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/farmer/"), fileName);
                            var fullPath = Server.MapPath("~/Content/img/upload/farmer/") + farmer_img.FileName;
                            if (System.IO.File.Exists(fullPath))
                            {
                                ViewBag.ActionMessage = "Same File already Exists";
                            }
                            else
                            {
                                farmer_img.SaveAs(userfolderpath);
                                ViewBag.ActionMessage = "File has been uploaded successfully";
                                registerModel.farmer_img = farmer_img.FileName;
                            }
                            /*var file = Path.GetFileName(farmer_img.FileName);
                            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), file);
                            farmer_img.SaveAs(path);*/
                        }
                        else
                        {
                            ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                        }
                    }
                    string folderPathid = Server.MapPath("~/Content/img/upload/idcard/");
                    if (!Directory.Exists(folderPathid))
                    {
                        Directory.CreateDirectory(folderPathid);
                    }
                    if (card_img != null && card_img.ContentLength > 0)
                    {
                        if (card_img.ContentType == "image/jpeg" || card_img.ContentType == "image/jpg" || card_img.ContentType == "image/png")
                        {
                            var fileName = Path.GetFileName(card_img.FileName);
                            var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/idcard/"), fileName);
                            var fullPath = Server.MapPath("~/Content/img/upload/idcard/") + card_img.FileName;
                            if (System.IO.File.Exists(fullPath))
                            {
                                ViewBag.ActionMessage = "Same File already Exists";
                            }
                            else
                            {
                                card_img.SaveAs(userfolderpath);
                                ViewBag.ActionMessage = "File has been uploaded successfully";
                                registerModel.card_img = card_img.FileName;
                            }
                        }
                        else
                        {
                            ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                        }
                    }
                    /*bankuser bank = new bankuser();*/
                    /* bank.ID = (int)registerModel.bank;*/
                    farmdb.registers.Add(registerModel);
                    registerModel.adminBy = User.Identity.Name;
                    registerModel.dateUpdate = DateTime.Now;
                    registerModel.active = 100;
                    /*farmdb.bankusers.Add(bank);*/
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Create", "Plot", new { farmerName = registerModel.ID });
            }
            catch (Exception ex)
            {
               /* ViewBag.Message = ex.InnerException.InnerException.Message;*/
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Register/Edit/5
        public ActionResult Edit(int id)
        {
            register registerModel = new register();
            bankuser bankuserModel = new bankuser();
            using (farmdb farmdb = new farmdb())
            {
                registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();
                bankuserModel = farmdb.bankusers.Where(b => b.ID == registerModel.bank).FirstOrDefault();

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
                registerModel.ProvinceList = itemCountries;


                var countries2 = (from amp in farmdb.amphers select amp).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.ampherID.ToString(),
                    Text = x.ampherName
                });
                itemCountries2.AddRange(countries2);
                registerModel.AmpherList = itemCountries2;

                var countries3 = (from dis in farmdb.districts select dis).AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.districtID.ToString(),
                    Text = x.districtName
                });
                itemCountries3.AddRange(countries3);
                registerModel.DistrictList = itemCountries3;

                /*List<status> status = farmdb.status.ToList();
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
                                                                 Value = "100"
                                                             };
                    ViewBag.status = selstatuss;
                }*/


                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;
            }
            return View(registerModel);
        }
        // POST: Register/Edit/5
        [HttpPost]
        public ActionResult Edit(register registerModel, HttpPostedFileBase farmer_img, HttpPostedFileBase card_img)
        {
            try
            {
                /*register registerModel = new register();*/
                bankuser bankuserModel = new bankuser();
                using (farmdb farmdb = new farmdb())
                {
                    string folderPath = Server.MapPath("~/Content/img/upload/farmer/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    if (farmer_img != null && farmer_img.ContentLength > 0)
                    {
                        if (farmer_img.ContentType == "image/jpeg" || farmer_img.ContentType == "image/jpg" || farmer_img.ContentType == "image/png")
                        {
                            var fileName = Path.GetFileName(farmer_img.FileName);
                            var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/farmer/"), fileName);
                            farmer_img.SaveAs(userfolderpath);
                            registerModel.farmer_img = farmer_img.FileName;
                        }
                        else
                        {
                            ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                        }
                    }
                    string folderPathid = Server.MapPath("~/Content/img/upload/idcard/");
                    if (!Directory.Exists(folderPathid))
                    {
                        Directory.CreateDirectory(folderPathid);
                    }
                    if (card_img != null && card_img.ContentLength > 0)
                    {
                        if (card_img.ContentType == "image/jpeg" || card_img.ContentType == "image/jpg" || card_img.ContentType == "image/png")
                        {
                            var fileName = Path.GetFileName(card_img.FileName);
                            var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/idcard/"), fileName);
                            card_img.SaveAs(userfolderpath);
                            registerModel.card_img = card_img.FileName;
                            /*farmdb.registers.Include(registerModel.card_img);*/
                        }
                        else
                        {
                            ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                        }
                    }
                    farmdb.Entry(registerModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.Entry(registerModel.bankuser).State = System.Data.Entity.EntityState.Modified;
                    registerModel.adminBy = User.Identity.Name;
                    registerModel.dateUpdate = DateTime.Now;
                    registerModel.active = 100;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                /*ViewBag.Message = ex.InnerException.InnerException.Message;*/
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Register/Delete/5
        public ActionResult Delete(int id)
        {
            register registerModel = new register();
            bankuser bankuserModel = new bankuser();

            using (farmdb farmdb = new farmdb())
            {
                registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();
                bankuserModel = farmdb.bankusers.Where(b => b.ID == registerModel.bank).FirstOrDefault();

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

                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;

            }
            return View(registerModel);
        }
        // POST: Register/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    register registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();
                    bankuser bankuserModel = farmdb.bankusers.Where(b => b.ID == registerModel.bank).FirstOrDefault();
                    landplot landplotModel = farmdb.landplots.Where(p => p.farmerName == registerModel.ID).FirstOrDefault();
                    registerModel.bankuser = bankuserModel;
                    registerModel.active = 200;
                    if (registerModel.active == 100)
                    {
                        farmdb.registers.Remove(registerModel);
                        farmdb.bankusers.Remove(bankuserModel);
                        if (landplotModel != null)
                        {
                            farmdb.landplots.Remove(landplotModel);
                        }
                    }
                    registerModel.active = 200;
                    if (landplotModel != null)
                    {
                        landplotModel.active = 200;
                    }
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                /*ViewBag.Message = ex.InnerException.InnerException.Message;*/
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                    }
                    ViewBag.FileStatus = "File uploaded successfully.";
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
            }
            return View("Index");
        }
    }
}
