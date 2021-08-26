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

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

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
        public ActionResult Create(fuel fuelModel, filedetail filesModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    if (fuelModel.file_fuleImg != null)
                    {
                        String FileExt = Path.GetExtension(fuelModel.file_fuleImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[fuelModel.file_fuleImg.ContentLength];
                                fuelModel.file_fuleImg.InputStream.Read(data, 0, fuelModel.file_fuleImg.ContentLength);
                                filesModel.fileName = "fule_" + fuelModel.file_fuleImg.FileName;
                                filesModel.fileData = data;
                                fuelModel.fuleImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.fuels.Add(fuelModel);
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
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
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == fuelModel.fuleImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == fuelModel.fuleImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
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
        public ActionResult Edit(fuel fuelModel)
        {
            try
            {
                filedetail filesModel = new filedetail();
                using (farmdb farmdb = new farmdb())
                {
                    if (fuelModel.file_fuleImg != null)
                    {
                        String FileExt = Path.GetExtension(fuelModel.file_fuleImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[fuelModel.file_fuleImg.ContentLength];
                                fuelModel.file_fuleImg.InputStream.Read(data, 0, fuelModel.file_fuleImg.ContentLength);
                                filesModel.fileName = "fule_" + fuelModel.file_fuleImg.FileName;
                                filesModel.fileData = data;
                                fuelModel.fuleImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.Entry(fuelModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
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
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == fuelModel.fuleImg).FirstOrDefault();
                    farmdb.fuels.Remove(fuelModel);
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
    }
}
