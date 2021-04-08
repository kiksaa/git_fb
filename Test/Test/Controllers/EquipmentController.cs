using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class EquipmentController : Controller
    {
        // GET: Equipment
        public ActionResult Index()
        {
            List<equipment> EquipmentList = new List<equipment>();
            using (farmdb farmdb = new farmdb())
            {
                EquipmentList = farmdb.equipments.ToList<equipment>();
                ViewBag.TotalEquipment = EquipmentList.Count();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from e in farmdb.equipments
                           join v in farmdb.vehicles on e.vehicleType equals v.vehicleID into vlist
                           from v in vlist.DefaultIfEmpty()
                           select new
                           {
                               v.vehicleType,
                               e.vehicleName,
                               e.vehicleID,
                               e.detail,
                               e.price,
                               e.equipmentID
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.vehicleType = item.vehicleType;
                    objcvm.vehicleName = item.vehicleName;
                    objcvm.vehicleID = item.vehicleID;
                    objcvm.detail = item.detail;
                    objcvm.price = item.price;
                    objcvm.ID = item.equipmentID;
                    ViewModeltList.Add(objcvm);
                }

                return View(ViewModeltList);
            }
        }

        // GET: Equipment/Details/5
        public ActionResult Details(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.equipmentID == id).FirstOrDefault();
            }
            return View(equipmentModel);
        }

        // GET: Equipment/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<vehicle> vehicles = farmdb.vehicles.ToList();
                IEnumerable<SelectListItem> selvehicles = from v in vehicles
                                                          select new SelectListItem
                                                          {
                                                              Text = v.vehicleType,
                                                              Value = v.vehicleID.ToString()
                                                          };
                ViewBag.vehicles = selvehicles;

                List<unit> units = farmdb.units.ToList();
                IEnumerable<SelectListItem> selunits = from u in units
                                                       select new SelectListItem
                                                          {
                                                              Text = u.unitName,
                                                              Value = u.unitID.ToString()
                                                          };
                ViewBag.units = selunits;

                List<energy> energies = farmdb.energies.ToList();
                IEnumerable<SelectListItem> selenergies = from e in energies
                                                          select new SelectListItem
                                                       {
                                                           Text = e.energyName,
                                                           Value = e.energyID.ToString()
                                                       };
                ViewBag.energies = selenergies;
            }
            return View(new equipment());
        }

        // POST: Equipment/Create
        [HttpPost]
        public ActionResult Create(equipment equipmentModel, HttpPostedFileBase vehicleImg)
        {
            string folderPath = Server.MapPath("~/Content/img/upload/vehicle/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (vehicleImg != null && vehicleImg.ContentLength > 0)
            {
                if (vehicleImg.ContentType == "image/jpeg" || vehicleImg.ContentType == "image/jpg" || vehicleImg.ContentType == "image/png")
                {
                    var fileName = Path.GetFileName(vehicleImg.FileName);
                    var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/vehicle/"), fileName);
                    var fullPath = Server.MapPath("~/Content/img/upload/vehicle/") + vehicleImg.FileName;
                    if (System.IO.File.Exists(fullPath))
                    {
                        ViewBag.ActionMessage = "Same File already Exists";
                    }
                    else
                    {
                        vehicleImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        equipmentModel.vehicleImg = vehicleImg.FileName;
                    }
                }
                else
                {
                    ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                }
            }
            using (farmdb farmdb = new farmdb())
            {
                farmdb.equipments.Add(equipmentModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.equipmentID == id).FirstOrDefault();
                List<vehicle> vehicles = farmdb.vehicles.ToList();
                IEnumerable<SelectListItem> selvehicles = from v in vehicles
                                                          select new SelectListItem
                                                          {
                                                              Text = v.vehicleType,
                                                              Value = v.vehicleID.ToString()
                                                          };
                ViewBag.vehicles = selvehicles;

                List<unit> units = farmdb.units.ToList();
                IEnumerable<SelectListItem> selunits = from u in units
                                                       select new SelectListItem
                                                       {
                                                           Text = u.unitName,
                                                           Value = u.unitID.ToString()
                                                       };
                ViewBag.units = selunits;

                List<energy> energies = farmdb.energies.ToList();
                IEnumerable<SelectListItem> selenergies = from e in energies
                                                          select new SelectListItem
                                                          {
                                                              Text = e.energyName,
                                                              Value = e.energyID.ToString()
                                                          };
                ViewBag.energies = selenergies;
            }
            return View(equipmentModel);
        }

        // POST: Equipment/Edit/5
        [HttpPost]
        public ActionResult Edit(equipment equipmentModel)
        {
            using (farmdb farmdb = new farmdb())
            {
                farmdb.Entry(equipmentModel).State = System.Data.Entity.EntityState.Modified;
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Equipment/Delete/5
        public ActionResult Delete(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.equipmentID == id).FirstOrDefault();
                List<vehicle> vehicles = farmdb.vehicles.ToList();
                IEnumerable<SelectListItem> selvehicles = from v in vehicles
                                                          select new SelectListItem
                                                          {
                                                              Text = v.vehicleType,
                                                              Value = v.vehicleID.ToString()
                                                          };
                ViewBag.vehicles = selvehicles;

                List<unit> units = farmdb.units.ToList();
                IEnumerable<SelectListItem> selunits = from u in units
                                                       select new SelectListItem
                                                       {
                                                           Text = u.unitName,
                                                           Value = u.unitID.ToString()
                                                       };
                ViewBag.units = selunits;

                List<energy> energies = farmdb.energies.ToList();
                IEnumerable<SelectListItem> selenergies = from e in energies
                                                          select new SelectListItem
                                                          {
                                                              Text = e.energyName,
                                                              Value = e.energyID.ToString()
                                                          };
                ViewBag.energies = selenergies;
            }
            return View(equipmentModel);
        }

        // POST: Equipment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (farmdb farmdb = new farmdb())
            {
                equipment equipmentModel = farmdb.equipments.Where(x => x.equipmentID == id).FirstOrDefault();
                farmdb.equipments.Remove(equipmentModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
