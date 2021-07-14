using System;
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
        public ActionResult Create(vehicle vehicleModel, HttpPostedFileBase vehicleImg)
        {
            try
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
                            vehicleModel.vehicleImg = vehicleImg.FileName;
                        }
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
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

        // GET: Equipment/Edit/5
        public ActionResult Edit(int id)
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

        // POST: Equipment/Edit/5
        [HttpPost]
        public ActionResult Edit(vehicle vehicleModel, HttpPostedFileBase vehicleImg)
        {
            try
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
                        vehicleImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        vehicleModel.vehicleImg = vehicleImg.FileName;
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
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
                    farmdb.vehicles.Remove(vehicleModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
