using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SoftwareController : Controller
    {
        #region Index
        // GET: Software
        public ActionResult Index()
        {
            List<software> softwareList = new List<software>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                softwareList = farmdb.softwares.ToList<software>();
                ViewBag.TotalSoftware = softwareList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from s in farmdb.softwares
                           select new
                           {
                               s.softwareID,
                               s.softwareName,
                               s.softwareType,
                               s.detail,
                               s.price,
                               s.IDsoft
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.softwareID = item.softwareID;
                    objcvm.softwareName = item.softwareName;
                    objcvm.detailS = item.detail;
                    objcvm.priceS = item.price;
                    objcvm.IDsoft = item.IDsoft;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }
        #endregion
        #region Details
        // GET: Software/Details/5
        public ActionResult Details(int id)
        {
            software softwareModel = new software();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();

                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == softwareModel.softwareImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == softwareModel.softwareImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }

                List<softwaretype> softwaretypes = farmdb.softwaretypes.ToList();
                IEnumerable<SelectListItem> selsoftwaretypes = from s in softwaretypes
                                                               select new SelectListItem
                                                               {
                                                                   Text = s.softType,
                                                                   Value = s.softType.ToString()
                                                               };
                ViewBag.softwaretypes = selsoftwaretypes;

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
            return View(softwareModel);
        }
        #endregion
        #region Create
        // GET: Software/Create
        public ActionResult Create()
        {
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                List<softwaretype> softwaretypes = farmdb.softwaretypes.ToList();
                IEnumerable<SelectListItem> selsoftwaretypes = from s in softwaretypes
                                                               select new SelectListItem
                                                               {
                                                                   Text = s.softType,
                                                                   Value = s.softType.ToString()
                                                               };
                ViewBag.softwaretypes = selsoftwaretypes;

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
            return View(new software());
        }

        // POST: Software/Create
        [HttpPost]
        public ActionResult Create(software softwareModel, filedetail filesModel)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    if (softwareModel.file_softwareImg != null)
                    {
                        String FileExt = Path.GetExtension(softwareModel.file_softwareImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[softwareModel.file_softwareImg.ContentLength];
                                softwareModel.file_softwareImg.InputStream.Read(data, 0, softwareModel.file_softwareImg.ContentLength);
                                filesModel.fileName = "software_" + softwareModel.file_softwareImg.FileName;
                                filesModel.fileData = data;
                                softwareModel.softwareImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.softwares.Add(softwareModel);
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
        // GET: Software/Edit/5
        public ActionResult Edit(int id)
        {
            software softwareModel = new software();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == softwareModel.softwareImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == softwareModel.softwareImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
                List<softwaretype> softwaretypes = farmdb.softwaretypes.ToList();
                IEnumerable<SelectListItem> selsoftwaretypes = from s in softwaretypes
                                                               select new SelectListItem
                                                               {
                                                                   Text = s.softType,
                                                                   Value = s.softType.ToString()
                                                               };
                ViewBag.softwaretypes = selsoftwaretypes;

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
            return View(softwareModel);
        }

        // POST: Software/Edit/5
        [HttpPost]
        public ActionResult Edit(software softwareModel)
        {
            try
            {
                filedetail filesModel = new filedetail();
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    if (softwareModel.file_softwareImg != null)
                    {
                        String FileExt = Path.GetExtension(softwareModel.file_softwareImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[softwareModel.file_softwareImg.ContentLength];
                                softwareModel.file_softwareImg.InputStream.Read(data, 0, softwareModel.file_softwareImg.ContentLength);
                                filesModel.fileName = "software_" + softwareModel.file_softwareImg.FileName;
                                filesModel.fileData = data;
                                softwareModel.softwareImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.Entry(softwareModel).State = System.Data.Entity.EntityState.Modified;
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
        // GET: Software/Delete/5
        public ActionResult Delete(int id)
        {
            software softwareModel = new software();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == softwareModel.softwareImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == softwareModel.softwareImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
                List<softwaretype> softwaretypes = farmdb.softwaretypes.ToList();
                IEnumerable<SelectListItem> selsoftwaretypes = from s in softwaretypes
                                                               select new SelectListItem
                                                               {
                                                                   Text = s.softType,
                                                                   Value = s.softType.ToString()
                                                               };
                ViewBag.softwaretypes = selsoftwaretypes;

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
            return View(softwareModel);
        }

        // POST: Software/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    software softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == softwareModel.softwareImg).FirstOrDefault();
                    farmdb.softwares.Remove(softwareModel);
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
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                filesModel = farmdb.filedetails.Where(x => x.fileName == name).FirstOrDefault();
            }
            return File(filesModel.fileData, "application/pdf", filesModel.fileName);
        }
        #endregion
    }
}