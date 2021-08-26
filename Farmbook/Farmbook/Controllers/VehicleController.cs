﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        public ActionResult Index()
        {
            List<vehicle> vehicleList = new List<vehicle>();
            using (farmdb farmdb = new farmdb())
            {
                vehicleList = farmdb.vehicles.ToList<vehicle>();
                ViewBag.TotalVehicle = vehicleList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from v in farmdb.vehicles
                            join t in farmdb.vehicletypes on v.vehicleType equals t.vehicleID into tlist
                            from t in tlist.DefaultIfEmpty()
                            select new
                            {
                                t.vehicleT,
                                v.vehicleName,
                                v.vehicleID,
                                v.detail,
                                v.price,
                                v.IDve,
                                
                            };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.vehicleType = item.vehicleT;
                    objcvm.vehicleName = item.vehicleName;
                    objcvm.vehicleID = item.vehicleID;
                    objcvm.detailV = item.detail;
                    objcvm.priceV = item.price;
                    objcvm.IDve = item.IDve;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
            
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int id)
        {
            vehicle vehicleModel = new vehicle();
            using (farmdb farmdb = new farmdb())
            {
                vehicleModel = farmdb.vehicles.Where(x => x.IDve == id).FirstOrDefault();

                List<vehicletype> vehicletypes = farmdb.vehicletypes.ToList();
                IEnumerable<SelectListItem> selvehicletypes = from v in vehicletypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = v.vehicleT,
                                                                  Value = v.vehicleID.ToString()
                                                              };
                ViewBag.vehicletypes = selvehicletypes;

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
            return View(vehicleModel);
        }

        // GET: Vehicle/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<vehicletype> vehicletypes = farmdb.vehicletypes.ToList();
                IEnumerable<SelectListItem> selvehicletypes = from v in vehicletypes
                                                              select new SelectListItem
                                                                {
                                                                    Text = v.vehicleT,
                                                                    Value = v.vehicleID.ToString()
                                                                };
                ViewBag.vehicletypes = selvehicletypes;

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
            return View(new vehicle());
        }

        // POST: Vehicle/Create
        [HttpPost]
        public ActionResult Create(vehicle vehicleModel, filedetail filesModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    if (vehicleModel.file_vehicleImg != null)
                    {
                        String FileExt = Path.GetExtension(vehicleModel.file_vehicleImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[vehicleModel.file_vehicleImg.ContentLength];
                                vehicleModel.file_vehicleImg.InputStream.Read(data, 0, vehicleModel.file_vehicleImg.ContentLength);
                                filesModel.fileName = "vehicle_" + vehicleModel.file_vehicleImg.FileName;
                                filesModel.fileData = data;
                                vehicleModel.vehicleImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.vehicles.Add(vehicleModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async System.Threading.Tasks.Task<ActionResult> RenderImage(int id)
        {
            farmdb farmdb = new farmdb();
            /*filedetail filedetailModel = farmdb.filedetails.Where(x => x.fileName == id).FirstOrDefault();*/
            filedetail item = await farmdb.filedetails.FindAsync(id);

            byte[] photoBack = item.fileData;

            return File(photoBack, "image/png");
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int id)
        {
            vehicle vehicleModel = new vehicle();
            using (farmdb farmdb = new farmdb())
            {
                vehicleModel = farmdb.vehicles.Where(x => x.IDve == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == vehicleModel.vehicleImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == vehicleModel.vehicleImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
                
                List<vehicletype> vehicletypes = farmdb.vehicletypes.ToList();
                IEnumerable<SelectListItem> selvehicletypes = from v in vehicletypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = v.vehicleT,
                                                                  Value = v.vehicleID.ToString()
                                                              };
                ViewBag.vehicletypes = selvehicletypes;

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
                /*return View(filedetailModel.fileData);*/
            }
            return View(vehicleModel);
        }
        // POST: Equipment/Edit/5
        [HttpPost]
        public ActionResult Edit(vehicle vehicleModel)
        {
            try
            {
                filedetail filesModel = new filedetail();
                using (farmdb farmdb = new farmdb())
                {
                    if (vehicleModel.file_vehicleImg != null)
                    {
                        String FileExt = Path.GetExtension(vehicleModel.file_vehicleImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[vehicleModel.file_vehicleImg.ContentLength];
                                vehicleModel.file_vehicleImg.InputStream.Read(data, 0, vehicleModel.file_vehicleImg.ContentLength);
                                filesModel.fileName = "vehicle_" + vehicleModel.file_vehicleImg.FileName;
                                filesModel.fileData = data;
                                vehicleModel.vehicleImg = filesModel.fileName;
                                ViewBag.Image = filesModel.fileData;
                               
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.Entry(vehicleModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int id)
        {
            vehicle vehicleModel = new vehicle();
            using (farmdb farmdb = new farmdb())
            {
                vehicleModel = farmdb.vehicles.Where(x => x.IDve == id).FirstOrDefault();
                List<vehicletype> vehicletypes = farmdb.vehicletypes.ToList();
                IEnumerable<SelectListItem> selvehicletypes = from v in vehicletypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = v.vehicleT,
                                                                  Value = v.vehicleID.ToString()
                                                              };
                ViewBag.vehicletypes = selvehicletypes;

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
            return View(vehicleModel);
        }

        // POST: Vehicle/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    vehicle vehicleModel = farmdb.vehicles.Where(x => x.IDve == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == vehicleModel.vehicleImg).FirstOrDefault();
                    farmdb.vehicles.Remove(vehicleModel);
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
        [HttpPost]
        public ActionResult DeleteFile(string name)
        {
            filedetail filesModel = new filedetail();
            using (farmdb farmdb = new farmdb())
            {
                filesModel = farmdb.filedetails.Where(x => x.fileName == name).FirstOrDefault();
            }
            return RedirectToAction("IndexSum", "Equipment");
        }
    }
}
