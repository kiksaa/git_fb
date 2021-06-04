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
        // GET: Staple
        public ActionResult Index()
        {
            List<staple> stapleList = new List<staple>();
            using (farmdb farmdb = new farmdb())
            {
                stapleList = farmdb.staples.ToList<staple>();
                ViewBag.TotalStaple = stapleList.Count();

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

        // GET: Staple/Details/5
        public ActionResult Details(int id)
        {
            staple stapleModel = new staple();
            using (farmdb farmdb = new farmdb())
            {
                stapleModel = farmdb.staples.Where(x => x.IDstap == id).FirstOrDefault();
            }
            return View(stapleModel);
        }

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
        public ActionResult Create(staple stapleModel, HttpPostedFileBase stapleImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/staple/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (stapleImg != null && stapleImg.ContentLength > 0)
                {
                    if (stapleImg.ContentType == "image/jpeg" || stapleImg.ContentType == "image/jpg" || stapleImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(stapleImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/staple/"), fileName);
                        var fullPath = Server.MapPath("~/Content/img/upload/staple/") + stapleImg.FileName;
                        if (System.IO.File.Exists(fullPath))
                        {
                            ViewBag.ActionMessage = "Same File already Exists";
                        }
                        else
                        {
                            stapleImg.SaveAs(userfolderpath);
                            ViewBag.ActionMessage = "File has been uploaded successfully";
                            stapleModel.stapleImg = stapleImg.FileName;
                        }
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.staples.Add(stapleModel);
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

        // GET: Staple/Edit/5
        public ActionResult Edit(int id)
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

        // POST: Staple/Edit/5
        [HttpPost]
        public ActionResult Edit(staple stapleModel, HttpPostedFileBase stapleImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/staple/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (stapleImg != null && stapleImg.ContentLength > 0)
                {
                    if (stapleImg.ContentType == "image/jpeg" || stapleImg.ContentType == "image/jpg" || stapleImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(stapleImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/staple/"), fileName);
                        stapleImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        stapleModel.stapleImg = stapleImg.FileName;
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(stapleModel).State = System.Data.Entity.EntityState.Modified;
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

        // GET: Staple/Delete/5
        public ActionResult Delete(int id)
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

        // POST: Staple/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    staple stapleModel = farmdb.staples.Where(x => x.IDstap == id).FirstOrDefault();
                    farmdb.staples.Remove(stapleModel);
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
