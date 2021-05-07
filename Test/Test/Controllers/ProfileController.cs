using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            List<profile> profileList = new List<profile>();
            using (farmdb farmdb = new farmdb())
            {
                profileList = farmdb.profiles.ToList<profile>();
            }
            return View(profileList);
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
            return View(new profile());
        }

        // POST: Profile/Create
        [HttpPost]
        public ActionResult Create(profile profileModel)
        {
            using (farmdb farmdb = new farmdb())
            {
                farmdb.profiles.Add(profileModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Profile/Edit/5
        /*public ActionResult Edit(int id)
        {
            profile profileModel = new profile();
            using (farmdb farmdb = new farmdb())
            {
                profileModel = farmdb.profiles.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(profileModel);
        }*/

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
            using (farmdb farmdb = new farmdb())
            {
                farmdb.Entry(profileModel).State = System.Data.Entity.EntityState.Modified;
                /*farmdb.Entry(profileModel).State = EntityState.Modified;*/
                /*farmdb.Entry(loginModel).State = EntityState.Added;*/
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index","Account");
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            profile profileModel = new profile();
            using (farmdb farmdb = new farmdb())
            {
                profileModel = farmdb.profiles.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(profileModel);
        }

        // POST: Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (farmdb farmdb = new farmdb())
            {
                profile profileModel = farmdb.profiles.Where(x => x.ID == id).FirstOrDefault();
                farmdb.profiles.Remove(profileModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
