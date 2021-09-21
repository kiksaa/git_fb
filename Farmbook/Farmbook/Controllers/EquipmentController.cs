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
        #region Index
        // GET: Equipment
        public ActionResult Index()
        {
            List<equipment> EquipmentList = new List<equipment>();
            using (farmdb farmdb = new farmdb())
            {
                EquipmentList = farmdb.equipments.ToList<equipment>();
                ViewBag.TotalEquipment = EquipmentList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

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
        #endregion
        #region IndexSum
        public ActionResult IndexSum()
        {
            try
            {
                List<equipment> EquipmentList = new List<equipment>();
                using (farmdb farmdb = new farmdb())
                {
                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();

                    EquipmentList = farmdb.equipments.ToList<equipment>();
                }
                return View(EquipmentList);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion
        #region Details
        // GET: Equipment/Details/5
        public ActionResult Details(int id)
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
        #endregion
        #region Create
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
        public ActionResult Create(equipment equipmentModel, filedetail filesModel)
        {
            try 
            {
                using (farmdb farmdb = new farmdb())
                {
                    if (equipmentModel.file_equipmentImg != null)
                    {
                        String FileExt = Path.GetExtension(equipmentModel.file_equipmentImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[equipmentModel.file_equipmentImg.ContentLength];
                                equipmentModel.file_equipmentImg.InputStream.Read(data, 0, equipmentModel.file_equipmentImg.ContentLength);
                                filesModel.fileName = "equipment_" + equipmentModel.file_equipmentImg.FileName;
                                filesModel.fileData = data;
                                equipmentModel.equipmentImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }

                    farmdb.equipments.Add(equipmentModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Edit
        // GET: Equipment/Edit/5
        public ActionResult Edit(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();

                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == equipmentModel.equipmentImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == equipmentModel.equipmentImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
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
        public ActionResult Edit(equipment equipmentModel, HttpPostedFileBase equipmentImg)
        {
            try
            {
                filedetail filesModel = new filedetail();
                using (farmdb farmdb = new farmdb())
                {
                    if (equipmentModel.file_equipmentImg != null)
                    {
                        String FileExt = Path.GetExtension(equipmentModel.file_equipmentImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[equipmentModel.file_equipmentImg.ContentLength];
                                equipmentModel.file_equipmentImg.InputStream.Read(data, 0, equipmentModel.file_equipmentImg.ContentLength);
                                filesModel.fileName = "equipment_" + equipmentModel.file_equipmentImg.FileName;
                                filesModel.fileData = data;
                                equipmentModel.equipmentImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.Entry(equipmentModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Delete
        // GET: Equipment/Delete/5
        public ActionResult Delete(int id)
        {
            equipment equipmentModel = new equipment();
            using (farmdb farmdb = new farmdb())
            {
                equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == equipmentModel.equipmentImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == equipmentModel.equipmentImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
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
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    equipment equipmentModel = farmdb.equipments.Where(x => x.IDequip == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == equipmentModel.equipmentImg).FirstOrDefault();
                    farmdb.equipments.Remove(equipmentModel);
                    farmdb.filedetails.Remove(filesModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum");
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
            using (farmdb farmdb = new farmdb())
            {
                filesModel = farmdb.filedetails.Where(x => x.fileName == name).FirstOrDefault();
            }
            return File(filesModel.fileData, "application/pdf", filesModel.fileName);
        }
        #endregion
    }
}
