using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BankController : Controller
    {
        // GET: Bank
        public ActionResult Index()
        {
            List<bankuser> bankList = new List<bankuser>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                bankList = farmdb.bankusers.ToList<bankuser>();
                ViewBag.TotalSoftware = bankList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from b in farmdb.bankusers
                           join bt in farmdb.banks on b.bankID equals bt.ID into blist
                           from bt in blist.DefaultIfEmpty()
                           select new
                           {
                               b.ID,
                               b.bankID,
                               b.bankName,
                               b.bankNo,
                               bt.bankType
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    /*objcvm.bankID = item.bankID;*/
                    objcvm.bankName = item.bankName;
                    objcvm.bankNo = item.bankNo;
                    objcvm.bankType = item.bankType;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }

        // GET: Bank/Details/5
        public ActionResult Details(int id)
        {
            bankuser bankModel = new bankuser();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                bankModel = farmdb.bankusers.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(bankModel);
        }
        public ActionResult CreateBank(int id)
        {
            register registerModel = new register();
            List<bankuser> bankList = new List<bankuser>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                registerModel = farmdb.registers.Where(x => x.ID == id).FirstOrDefault();
                List<bankuser> bankuserModel = farmdb.bankusers.Where(b => b.regisName == registerModel.ID).ToList();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from b in farmdb.bankusers
                           join bt in farmdb.banks on b.bankID equals bt.ID into blist
                           from bt in blist.DefaultIfEmpty()
                           select new
                           {
                               b.ID,
                               b.bankID,
                               b.bankName,
                               b.bankNo,
                               bt.bankType
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    /*objcvm.bankID = item.bankID;*/
                    objcvm.bankName = item.bankName;
                    objcvm.bankNo = item.bankNo;
                    objcvm.bankType = item.bankType;
                    ViewModeltList.Add(objcvm);
                }
                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;
                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;

                return View(ViewModeltList);
            }
        }
        // GET: Bank/Create
        public ActionResult Create(int id)
        {
            bankuser bankModel = new bankuser();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                bankModel = farmdb.bankusers.Where(x => x.ID == id).FirstOrDefault();
                bankModel.regisName = id;
                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;
                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;
            }
            /*return View(bankModel);*/
            return View(new bankuser());
        }
        // POST: Bank/Create
        [HttpPost]
        public ActionResult Create(bankuser bankModel, register registerModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    bankModel.regisName = registerModel.ID;
                    farmdb.bankusers.Add(bankModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Register");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Bank/Edit/5
        public ActionResult Edit(int id)    
        {
            bankuser bankModel = new bankuser();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                bankModel = farmdb.bankusers.Where(x => x.ID == id).FirstOrDefault();

                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;
                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;
            }
            return View(bankModel);
        }

        // POST: Bank/Edit/5
        [HttpPost]
        public ActionResult Edit(bankuser bankModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    farmdb.Entry(bankModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Register");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Bank/Delete/5
        public ActionResult Delete(int id)
        {
            bankuser bankModel = new bankuser();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                bankModel = farmdb.bankusers.Where(x => x.ID == id).FirstOrDefault();

                List<bank> banks = farmdb.banks.ToList();
                IEnumerable<SelectListItem> selbanks = from b in banks
                                                       select new SelectListItem
                                                       {
                                                           Text = b.bankType,
                                                           Value = b.ID.ToString()
                                                       };
                ViewBag.banks = selbanks;
                List<register> registers = farmdb.registers.ToList();
                IEnumerable<SelectListItem> selregisters = from r in registers
                                                           select new SelectListItem
                                                           {
                                                               Text = r.name,
                                                               Value = r.ID.ToString()
                                                           };
                ViewBag.registers = selregisters;
            }
            return View(bankModel);
        }

        // POST: Bank/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    bankuser bankModel = farmdb.bankusers.Where(x => x.ID == id).FirstOrDefault();
                    farmdb.bankusers.Remove(bankModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Register");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
