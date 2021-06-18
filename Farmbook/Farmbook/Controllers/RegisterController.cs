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

namespace Farmbook.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            register model = new register();
            List<register> registerList = new List<register>();
            /*for(int i = 1; i <= registerList.Count(); i++)
            {

            }*/
            using (farmdb farmdb = new farmdb())
            {
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
                               r.ID,r.name,r.registerID,r.cardID,r.no,r.moo,r.road,
                               /*p.provinceName,a.ampherName,d.districtName,*/
                               r.provinceStr,r.ampherStr,r.districtStr,s.statusName,r.dateUpdate,r.adminBy,r.active,
                               TotalLandplot = llist.Count(),
                               Totalarea = llist.Sum(ll => ll.areaPlot),
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
                        objcvm.provinceName = item.provinceStr;
                        objcvm.ampherName = item.ampherStr;
                        objcvm.districtName = item.districtStr;
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
        public ActionResult IndexPlot(int id)
        {
            register registerModel = new register();
            using (farmdb farmdb = new farmdb())
            {
                registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();
                List<landplot> landplotModel = farmdb.landplots.Where(p => p.farmerName == registerModel.ID).ToList();

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
                               l.ID,/*p.provinceName,a.ampherName,d.districtName,*/
                               l.provinceStr,l.ampherStr,l.districtStr,l.plotName,pro.projectName,
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    objcvm.provinceName = item.provinceStr;
                    objcvm.ampherName = item.ampherStr;
                    objcvm.districtName = item.districtStr;
                    objcvm.plotName = item.plotName;
                    objcvm.projectName = item.projectName;
                    ViewModeltList.Add(objcvm);
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
            }
            return View(registerModel);
        }
        // GET: Register/Create
        public ActionResult Create(int? provinceID, decimal? NRB, int? ampherID)
        {
            /*using (farmdb db = new farmdb())
            {
                var currFrom = db.amphers.Where(u => u.proID == eventIdB).First();
                Session["NewBTrate"] = currFrom.ampherID;
                NRB = currFrom.ampherID;
                var result = currFrom.ampherID;
                return Json(result, JsonRequestBehavior.AllowGet);
            }*/
            /* var data = GetTransactionBuyingRate();*/
            using (farmdb farmdb = new farmdb())
            {
                List<province> provinces = farmdb.provinces.ToList();
                IEnumerable<SelectListItem> selprovinces = from p in provinces
                                                           select new SelectListItem
                                                           {
                                                               Text = p.provinceName,
                                                               Value = p.provinceID.ToString(),

                                                           };
                var selectList = new SelectList(selprovinces, "provinceID", "provinceName");
                ViewBag.provinces = selprovinces;

                /*if (selectList.SelectedValue != null)
                {*/
                var ID = Convert.ToInt32(provinceID);
                List<ampher> amphers = farmdb.amphers.Where(a => a.proID == ID).ToList();
                /*List<ampher> amphers = farmdb.amphers.ToList();*/
                /*if(amphers.Count == 0)
                {*/
                IEnumerable<SelectListItem> selamphers = from a in amphers
                                                             /*where a.proID == ID*/
                                                         select new SelectListItem
                                                         {
                                                             Text = a.ampherName,
                                                             Value = a.ampherID.ToString()
                                                         };
                ViewBag.amphers = selamphers;
                /*}*/

                List<district> districts = farmdb.districts.ToList();
                IEnumerable<SelectListItem> seldistricts = from d in districts
                                                           select new SelectListItem
                                                           {
                                                               Text = d.districtName,
                                                               Value = d.districtID.ToString()
                                                           };
                ViewBag.districts = seldistricts;

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
            return View(new register());
        }
        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(register registerModel, HttpPostedFileBase farmer_img, HttpPostedFileBase card_img)
        {
            /*try
            {*/
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
            /*}
            catch (Exception ex)
            {
                *//*ViewBag.Message = ex.InnerException.InnerException.Message;*//*
                return RedirectToAction("Index", "Home");
            }*/
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
                        }
                        else
                        {
                            ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                        }
                    }
                    /*bankuserModel = farmdb.bankusers.Where(b => b.ID == registerModel.bank).FirstOrDefault();
                    registerModel.bankuser = bankuserModel;*/
                    registerModel.bank = bankuserModel.ID;

                    /*registerModel.bankuser = farmdb.bankusers.Where(b => b.ID == registerModel.bank).FirstOrDefault();*/
                    farmdb.Entry(registerModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.Entry(registerModel.bankuser).State = System.Data.Entity.EntityState.Added;
                    registerModel.adminBy = User.Identity.Name;
                    registerModel.dateUpdate = DateTime.Now;
                    registerModel.active = 100;
                    /*UpdateModel(bankuserModel);*/
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
