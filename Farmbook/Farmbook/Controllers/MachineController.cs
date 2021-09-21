using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class MachineController : Controller
    {
        #region Index
        // GET: Machine
        public ActionResult Index()
        {
            List<machine> machineList = new List<machine>();
            using (farmdb farmdb = new farmdb())
            {
                machineList = farmdb.machines.ToList<machine>();
                ViewBag.TotalMachine = machineList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from m in farmdb.machines
                            join t in farmdb.machinetypes on m.machineType equals t.machineID into mlist
                            from t in mlist.DefaultIfEmpty()
                            select new
                            {
                                t.machineT,
                                m.machineID,
                                m.machineName,
                                m.detail,
                                m.price,
                                m.IDmac
                            };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.machineType = item.machineT;
                    objcvm.machineName = item.machineName;
                    objcvm.machineID = item.machineID;
                    objcvm.detailM = item.detail;
                    objcvm.priceM = item.price;
                    objcvm.IDmac = item.IDmac;
                    ViewModeltList.Add(objcvm);
                }

                return View(ViewModeltList);
            }
        }
        #endregion
        #region Details
        // GET: Machine/Details/5
        public ActionResult Details(int id)
        {
            machine machineModel = new machine();
            using (farmdb farmdb = new farmdb())
            {
                machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();

                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(machineModel);
        }
        #endregion
        #region Create
        // GET: Machine/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(new machine());
        }
        // POST: Machine/Create
        [HttpPost]
        public ActionResult Create(machine machineModel, filedetail filesModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    if (machineModel.file_machineImg != null)
                    {
                        String FileExt = Path.GetExtension(machineModel.file_machineImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[machineModel.file_machineImg.ContentLength];
                                machineModel.file_machineImg.InputStream.Read(data, 0, machineModel.file_machineImg.ContentLength);
                                filesModel.fileName = "machine_" + machineModel.file_machineImg.FileName;
                                filesModel.fileData = data;
                                machineModel.machineImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }

                    farmdb.machines.Add(machineModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Edit
        // GET: Machine/Edit/5
        public ActionResult Edit(int id)
        {
            machine machineModel = new machine();
            using (farmdb farmdb = new farmdb())
            {
                machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();

                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == machineModel.machineImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == machineModel.machineImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }

                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(machineModel);
        }

        // POST: Machine/Edit/5
        [HttpPost]
        public ActionResult Edit(machine machineModel)
        {
            try
            {
                filedetail filesModel = new filedetail();
                using (farmdb farmdb = new farmdb())
                {
                    if (machineModel.file_machineImg != null)
                    {
                        String FileExt = Path.GetExtension(machineModel.file_machineImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[machineModel.file_machineImg.ContentLength];
                                machineModel.file_machineImg.InputStream.Read(data, 0, machineModel.file_machineImg.ContentLength);
                                filesModel.fileName = "machine_" + machineModel.file_machineImg.FileName;
                                filesModel.fileData = data;
                                machineModel.machineImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.Entry(machineModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region Delete
        // GET: Machine/Delete/5
        public ActionResult Delete(int id)
        {
            machine machineModel = new machine();
            using (farmdb farmdb = new farmdb())
            {
                machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == machineModel.machineImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == machineModel.machineImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(machineModel);
        }

        // POST: Machine/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    machine machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == machineModel.machineImg).FirstOrDefault();
                    farmdb.machines.Remove(machineModel);
                    farmdb.filedetails.Remove(filesModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
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
