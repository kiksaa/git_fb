using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class SoftwareController : Controller
    {
        // GET: Software
        public ActionResult Index()
        {
            List<software> softwareList = new List<software>();
            using (farmdb farmdb = new farmdb())
            {
                softwareList = farmdb.softwares.ToList<software>();
                ViewBag.TotalSoftware = softwareList.Count();
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

        // GET: Software/Details/5
        public ActionResult Details(int id)
        {
            software softwareModel = new software();
            using (farmdb farmdb = new farmdb())
            {
                softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();
            }
            return View(softwareModel);
        }

        // GET: Software/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
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
        public ActionResult Create(software softwareModel, HttpPostedFileBase softwareImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/software/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (softwareImg != null && softwareImg.ContentLength > 0)
                {
                    if (softwareImg.ContentType == "image/jpeg" || softwareImg.ContentType == "image/jpg" || softwareImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(softwareImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/software/"), fileName);
                        var fullPath = Server.MapPath("~/Content/img/upload/software/") + softwareImg.FileName;
                        if (System.IO.File.Exists(fullPath))
                        {
                            ViewBag.ActionMessage = "Same File already Exists";
                        }
                        else
                        {
                            softwareImg.SaveAs(userfolderpath);
                            ViewBag.ActionMessage = "File has been uploaded successfully";
                            softwareModel.softwareImg = softwareImg.FileName;
                        }
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.softwares.Add(softwareModel);
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

        // GET: Software/Edit/5
        public ActionResult Edit(int id)
        {
            software softwareModel = new software();
            using (farmdb farmdb = new farmdb())
            {
                softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();

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
        public ActionResult Edit(software softwareModel, HttpPostedFileBase softwareImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/software/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (softwareImg != null && softwareImg.ContentLength > 0)
                {
                    if (softwareImg.ContentType == "image/jpeg" || softwareImg.ContentType == "image/jpg" || softwareImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(softwareImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/software/"), fileName);
                        softwareImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        softwareModel.softwareImg = softwareImg.FileName;
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(softwareModel).State = System.Data.Entity.EntityState.Modified;
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

        // GET: Software/Delete/5
        public ActionResult Delete(int id)
        {
            software softwareModel = new software();
            using (farmdb farmdb = new farmdb())
            {
                softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();

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
                using (farmdb farmdb = new farmdb())
                {
                    software softwareModel = farmdb.softwares.Where(x => x.IDsoft == id).FirstOrDefault();
                    farmdb.softwares.Remove(softwareModel);
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
