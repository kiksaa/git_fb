using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class StapleController : Controller
    {
        #region Index
        // GET: Staple
        public ActionResult Index()
        {
            List<staple> stapleList = new List<staple>();
            using (farmdb farmdb = new farmdb())
            {
                stapleList = farmdb.staples.ToList<staple>();
                ViewBag.TotalStaple = stapleList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from s in farmdb.staples
                            join t in farmdb.stapletypes on s.stapleType equals t.stapleID into slist
                            from t in slist.DefaultIfEmpty()
                            select new
                            {
                                s.stapleID,
                                t.stapleT,
                                s.stapleName,
                                s.detail,
                                s.price,
                                s.IDstap
                            };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.stapleType = item.stapleT;
                    objcvm.stapleID = item.stapleID;
                    objcvm.stapleName = item.stapleName;
                    objcvm.detailSt = item.detail;
                    objcvm.priceSt = item.price;
                    objcvm.IDstap = item.IDstap;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }
        #endregion
        #region Details
        // GET: Staple/Details/5
        public ActionResult Details(int id)
        {
            staple stapleModel = new staple();
            using (farmdb farmdb = new farmdb())
            {
                stapleModel = farmdb.staples.Where(x => x.IDstap == id).FirstOrDefault();

                List<stapletype> stapletypes = farmdb.stapletypes.ToList();
                IEnumerable<SelectListItem> selstapletypes = from s in stapletypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = s.stapleT,
                                                                 Value = s.stapleID.ToString()
                                                             };
                ViewBag.stapletypes = selstapletypes;

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
            return View(stapleModel);
        }
        #endregion
        #region Create
        // GET: Staple/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<stapletype> stapletypes = farmdb.stapletypes.ToList();
                IEnumerable<SelectListItem> selstapletypes = from s in stapletypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = s.stapleT,
                                                                 Value = s.stapleID.ToString()
                                                             };
                ViewBag.stapletypes = selstapletypes;

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
            return View(new staple());
        }

        // POST: Staple/Create
        [HttpPost]
        public ActionResult Create(staple stapleModel, filedetail filesModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    if (stapleModel.file_stapleImg != null)
                    {
                        String FileExt = Path.GetExtension(stapleModel.file_stapleImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[stapleModel.file_stapleImg.ContentLength];
                                stapleModel.file_stapleImg.InputStream.Read(data, 0, stapleModel.file_stapleImg.ContentLength);
                                filesModel.fileName = "staple_" + stapleModel.file_stapleImg.FileName;
                                filesModel.fileData = data;
                                stapleModel.stapleImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.staples.Add(stapleModel);
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
        // GET: Staple/Edit/5
        public ActionResult Edit(int id)
        {
            staple stapleModel = new staple();
            using (farmdb farmdb = new farmdb())
            {
                stapleModel = farmdb.staples.Where(x => x.IDstap == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == stapleModel.stapleImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == stapleModel.stapleImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
                List<stapletype> stapletypes = farmdb.stapletypes.ToList();
                IEnumerable<SelectListItem> selstapletypes = from s in stapletypes
                                                             select new SelectListItem
                                                             {
                                                                 Text = s.stapleT,
                                                                 Value = s.stapleID.ToString()
                                                             };
                ViewBag.stapletypes = selstapletypes;

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
            return View(stapleModel);
        }

        // POST: Staple/Edit/5
        [HttpPost]
        public ActionResult Edit(staple stapleModel)
        {
            try
            {
                filedetail filesModel = new filedetail();
                using (farmdb farmdb = new farmdb())
                {
                    if (stapleModel.file_stapleImg != null)
                    {
                        String FileExt = Path.GetExtension(stapleModel.file_stapleImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[stapleModel.file_stapleImg.ContentLength];
                                stapleModel.file_stapleImg.InputStream.Read(data, 0, stapleModel.file_stapleImg.ContentLength);
                                filesModel.fileName = "staple_" + stapleModel.file_stapleImg.FileName;
                                filesModel.fileData = data;
                                stapleModel.stapleImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
                    farmdb.Entry(stapleModel).State = System.Data.Entity.EntityState.Modified;
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
        // GET: Staple/Delete/5
        public ActionResult Delete(int id)
        {
            staple stapleModel = new staple();
            using (farmdb farmdb = new farmdb())
            {
                stapleModel = farmdb.staples.Where(x => x.IDstap == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == stapleModel.stapleImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == stapleModel.stapleImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
                List<stapletype> stapletypes = farmdb.stapletypes.ToList();
                IEnumerable<SelectListItem> selstapletypes = from s in stapletypes
                                                             select new SelectListItem
                                                              {
                                                                  Text = s.stapleT,
                                                                  Value = s.stapleID.ToString()
                                                              };
                ViewBag.stapletypes = selstapletypes;

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
            return View(stapleModel);
        }

        // POST: Staple/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    staple stapleModel = farmdb.staples.Where(x => x.IDstap == id).FirstOrDefault();
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == stapleModel.stapleImg).FirstOrDefault();
                    farmdb.staples.Remove(stapleModel);
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
