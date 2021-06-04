using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            try
            {
                /*if (User.Identity.Name == "admin")
                {*/
                    List<profile> profileList = new List<profile>();
                    using (farmdb farmdb = new farmdb())
                    {
                        profileList = farmdb.profiles.ToList<profile>();
                        ViewBag.TotalProfile = profileList.Count();
                        List<ViewModel> ViewModeltList = new List<ViewModel>();
                        var data = from p in farmdb.profiles
                                   join r in farmdb.registertypes on p.registerType equals r.typeID into rlist
                                   from r in rlist.DefaultIfEmpty()
                                   join g in farmdb.genders on p.gender equals g.genderID into glist
                                   from g in glist.DefaultIfEmpty()
                                   select new
                                   {
                                       p.ID,
                                       p.name,
                                       p.cradID,
                                       g.genderName,
                                       p.birthday,
                                       p.email,
                                       p.tel,
                                       r.typeName
                                   };
                        foreach (var item in data)
                        {
                            ViewModel objcvm = new ViewModel();
                            objcvm.ID = item.ID;
                            objcvm.name = item.name;
                            objcvm.cardID = item.cradID;
                            objcvm.genderName = item.genderName;
                            objcvm.birthday = (DateTime)item.birthday;
                            objcvm.email = item.email;
                            objcvm.tel = item.tel;
                            objcvm.typeName = item.typeName;
                            ViewModeltList.Add(objcvm);
                        }
                        return View(ViewModeltList);
                    }
                /*}
                else
                {
                    return RedirectToAction("Index", "Account");
                }*/
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            profile profileModel = new profile();
            using (farmdb farmdb = new farmdb())
            {
                profileModel = farmdb.profiles.Where(x => x.ID == id).FirstOrDefault();
            }
                return View(profileModel);
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
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


                List<registertype> registertypes = farmdb.registertypes.ToList();
                IEnumerable<SelectListItem> selretypes = from rt in registertypes
                                                         select new SelectListItem
                                                         {
                                                             Text = rt.typeName,
                                                             Value = rt.typeID.ToString()
                                                         };
                ViewBag.registertypes = selretypes;
            }
            return View(new profile());
        }

        // POST: Profile/Create
        [HttpPost]
        public ActionResult Create(profile profileModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.profiles.Add(profileModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home");
            }
            
        }
        public ActionResult Edit(string email)
        {
            profile profileModel = new profile();
            using (farmdb farmdb = new farmdb())
            {
                profileModel = farmdb.profiles.Where(x => x.email == email).FirstOrDefault();

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


                List<registertype> registertypes = farmdb.registertypes.ToList();
                IEnumerable<SelectListItem> selretypes = from rt in registertypes
                                                         select new SelectListItem
                                                         {
                                                             Text = rt.typeName,
                                                             Value = rt.typeID.ToString()
                                                         };
                ViewBag.registertypes = selretypes;
            }
            return View(profileModel);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(profile profileModel, login loginModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(profileModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.Entry(loginModel).State = System.Data.Entity.EntityState.Added;
                    /*if (loginModel == new login())
                    {
                        farmdb.Entry(loginModel).State = System.Data.Entity.EntityState.Deleted;
                    }*/
                    /*farmdb.Entry(profileModel).State = EntityState.Modified;*/
                    /*farmdb.Entry(loginModel).State = EntityState.Added;*/
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index", "Profile");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            profile profileModel = new profile();
            using (farmdb farmdb = new farmdb())
            {
                profileModel = farmdb.profiles.Where(x => x.ID == id).FirstOrDefault();

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

                List<registertype> registertypes = farmdb.registertypes.ToList();
                IEnumerable<SelectListItem> selretypes = from rt in registertypes
                                                         select new SelectListItem
                                                         {
                                                             Text = rt.typeName,
                                                             Value = rt.typeID.ToString()
                                                         };
                ViewBag.registertypes = selretypes;
            }
            return View(profileModel);
        }

        // POST: Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    profile profileModel = farmdb.profiles.Where(x => x.ID == id).FirstOrDefault();
                    login loginModel = farmdb.logins.Where(l => l.email == profileModel.email).FirstOrDefault();
                    farmdb.profiles.Remove(profileModel);
                    farmdb.logins.Remove(loginModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
