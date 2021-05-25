using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
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
                           join t in farmdb.equipmenttypes on e.equipmentType equals t.equipmentID into tlist
                           from t in tlist.DefaultIfEmpty()
                           select new
                           {
                               t.equipmentT,
                               e.equipmentID,
                               e.equipmentName,
                               e.detail,
                               e.price,
                               e.IDequip
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.equipmentType = item.equipmentT;
                    objcvm.equipmentName = item.equipmentName;
                    objcvm.equipmentID = item.equipmentID;
                    objcvm.detail = item.detail;
                    objcvm.price = item.price;
                    objcvm.IDequip = item.IDequip;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }

        public ActionResult IndexSum()
        {
            List<equipment> EquipmentList = new List<equipment>();
            using (farmdb farmdb = new farmdb())
            {
                EquipmentList = farmdb.equipments.ToList<equipment>();
            }
            return View(EquipmentList);
        }
        // GET: Equipment/Details/5
        public ActionResult Details(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();
            }
            return View(equipmentModel);
        }

        // GET: Equipment/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<equipmenttype> equipmenttypes = farmdb.equipmenttypes.ToList();
                IEnumerable<SelectListItem> selequipmenttypes = from e in equipmenttypes
                                                                select new SelectListItem
                                                          {
                                                              Text = e.equipmentT,
                                                              Value = e.equipmentID.ToString()
                                                          };
                ViewBag.equipmenttypes = selequipmenttypes;

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
        public ActionResult Create(equipment equipmentModel, HttpPostedFileBase equipmentImg)
        {
            string folderPath = Server.MapPath("~/Content/img/upload/vehicle/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (equipmentImg != null && equipmentImg.ContentLength > 0)
            {
                if (equipmentImg.ContentType == "image/jpeg" || equipmentImg.ContentType == "image/jpg" || equipmentImg.ContentType == "image/png")
                {
                    var fileName = Path.GetFileName(equipmentImg.FileName);
                    var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/vehicle/"), fileName);
                    var fullPath = Server.MapPath("~/Content/img/upload/vehicle/") + equipmentImg.FileName;
                    if (System.IO.File.Exists(fullPath))
                    {
                        ViewBag.ActionMessage = "Same File already Exists";
                    }
                    else
                    {
                        equipmentImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        equipmentModel.equipmentImg = equipmentImg.FileName;
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
            return RedirectToAction("IndexSum");
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();
                List<equipmenttype> equipmenttypes = farmdb.equipmenttypes.ToList();
                IEnumerable<SelectListItem> selequipmenttypes = from e in equipmenttypes
                                                                select new SelectListItem
                                                                {
                                                                    Text = e.equipmentT,
                                                                    Value = e.equipmentID.ToString()
                                                                };
                ViewBag.equipmenttypes = selequipmenttypes;

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
            return RedirectToAction("IndexSum");
        }

        // GET: Equipment/Delete/5
        public ActionResult Delete(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();
                List<equipmenttype> equipmenttypes = farmdb.equipmenttypes.ToList();
                IEnumerable<SelectListItem> selequipmenttypes = from e in equipmenttypes
                                                                select new SelectListItem
                                                                {
                                                                    Text = e.equipmentT,
                                                                    Value = e.equipmentID.ToString()
                                                                };
                ViewBag.equipmenttypes = selequipmenttypes;

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
                equipment equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();
                farmdb.equipments.Remove(equipmentModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("IndexSum");
        }
    }
}
