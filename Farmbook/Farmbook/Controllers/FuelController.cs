using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class FuelController : Controller
    {
        // GET: Fuel
        public ActionResult Index()
        {
            List<fuel> fuelList = new List<fuel>();
            using (farmdb farmdb = new farmdb())
            {
                fuelList = farmdb.fuels.ToList<fuel>();
                ViewBag.TotalFuel = fuelList.Count();
                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from f in farmdb.fuels
                            join t in farmdb.fueltypes on f.fuelType equals t.fuelID into tlist
                            from t in tlist.DefaultIfEmpty()
                            select new
                            {
                                t.fuelType1,
                                f.fuelName,
                                f.detail,
                                f.price,
                                f.IDfule
                            };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.fuelName = item.fuelName;
                    objcvm.fuelType = item.fuelType1;
                    objcvm.detailF = item.detail;
                    objcvm.priceF = item.price;
                    objcvm.IDfule = item.IDfule;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }

        // GET: Fuel/Details/5
        public ActionResult Details(int id)
        {
            fuel fuelModel = new fuel();
            using (farmdb farmdb = new farmdb())
            {
                fuelModel = farmdb.fuels.Where(x => x.IDfule == id).FirstOrDefault();
            }
            return View(fuelModel);
        }

        // GET: Fuel/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<fueltype> fueltypes = farmdb.fueltypes.ToList();
                IEnumerable<SelectListItem> selfueltypes = from f in fueltypes
                                                           select new SelectListItem
                                                           {
                                                               Text = f.fuelType1,
                                                               Value = f.fuelID.ToString()
                                                           };
                ViewBag.fueltypes = selfueltypes;

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
            return View(new fuel());
        }

        // POST: Fuel/Create
        [HttpPost]
        public ActionResult Create(fuel fuelModel, HttpPostedFileBase fuleImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/fuel/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (fuleImg != null && fuleImg.ContentLength > 0)
                {
                    if (fuleImg.ContentType == "image/jpeg" || fuleImg.ContentType == "image/jpg" || fuleImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(fuleImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/fuel/"), fileName);
                        var fullPath = Server.MapPath("~/Content/img/upload/fuel/") + fuleImg.FileName;
                        if (System.IO.File.Exists(fullPath))
                        {
                            ViewBag.ActionMessage = "Same File already Exists";
                        }
                        else
                        {
                            fuleImg.SaveAs(userfolderpath);
                            ViewBag.ActionMessage = "File has been uploaded successfully";
                            fuelModel.fuleImg = fuleImg.FileName;
                        }
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.fuels.Add(fuelModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Fuel/Edit/5
        public ActionResult Edit(int id)
        {
            fuel fuelModel = new fuel();
            using (farmdb farmdb = new farmdb())
            {
                fuelModel = farmdb.fuels.Where(x => x.IDfule == id).FirstOrDefault();
                List<fueltype> fueltypes = farmdb.fueltypes.ToList();
                IEnumerable<SelectListItem> selfueltypes = from f in fueltypes
                                                           select new SelectListItem
                                                           {
                                                               Text = f.fuelType1,
                                                               Value = f.fuelID.ToString()
                                                           };
                ViewBag.fueltypes = selfueltypes;

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
            return View(fuelModel);
        }

        // POST: Fuel/Edit/5
        [HttpPost]
        public ActionResult Edit(fuel fuelModel, HttpPostedFileBase fuleImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/fuel/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (fuleImg != null && fuleImg.ContentLength > 0)
                {
                    if (fuleImg.ContentType == "image/jpeg" || fuleImg.ContentType == "image/jpg" || fuleImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(fuleImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/fuel/"), fileName);
                        fuleImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        fuelModel.fuleImg = fuleImg.FileName;
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(fuelModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Fuel/Delete/5
        public ActionResult Delete(int id)
        {
            fuel fuelModel = new fuel();
            using (farmdb farmdb = new farmdb())
            {
                fuelModel = farmdb.fuels.Where(x => x.IDfule == id).FirstOrDefault();
                List<fueltype> fueltypes = farmdb.fueltypes.ToList();
                IEnumerable<SelectListItem> selfueltypes = from f in fueltypes
                                                           select new SelectListItem
                                                              {
                                                                  Text = f.fuelType1,
                                                                  Value = f.fuelID.ToString()
                                                              };
                ViewBag.fueltypes = selfueltypes;

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
            return View(fuelModel);
        }

        // POST: Fuel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    fuel fuelModel = farmdb.fuels.Where(x => x.IDfule == id).FirstOrDefault();
                    farmdb.fuels.Remove(fuelModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
