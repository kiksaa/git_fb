using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class LaborController : Controller
    {
        // GET: Labor
        public ActionResult Index()
        {
            List<labor> laborList = new List<labor>();
            using (farmdb farmdb = new farmdb())
            {
                laborList = farmdb.labors.ToList<labor>();
                ViewBag.TotalLabor = laborList.Count();
                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from l in farmdb.labors
                            join t in farmdb.labortypes on l.laborType equals t.laborID into tlist
                            from t in tlist.DefaultIfEmpty()
                            join p in farmdb.positions on l.position equals p.positionID into plist
                            from p in plist.DefaultIfEmpty()
                            select new
                            {
                                t.laborT,
                                l.laborID,
                                l.laborName,
                                l.salary,
                                p.positionName,
                                l.laborType,
                                l.IDlab
                            };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.laborType = item.laborT;
                    objcvm.positionName = item.positionName;
                    objcvm.laborID = item.laborID;
                    objcvm.laborName = item.laborName;
                    objcvm.salary = item.salary;
                    objcvm.IDlab = item.IDlab;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }

        // GET: Labor/Details/5
        public ActionResult Details(int id)
        {
            labor laborModel = new labor();
            using (farmdb farmdb = new farmdb())
            {
                laborModel = farmdb.labors.Where(x => x.IDlab == id).FirstOrDefault();
            }
            return View(laborModel);
        }

        // GET: Labor/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<labortype> labortypes = farmdb.labortypes.ToList();
                IEnumerable<SelectListItem> sellabortypes = from l in labortypes
                                                            select new SelectListItem
                                                              {
                                                                  Text = l.laborT,
                                                                  Value = l.laborID.ToString()
                                                              };
                ViewBag.labortypes = sellabortypes;

                List<position> positions = farmdb.positions.ToList();
                IEnumerable<SelectListItem> selpositions = from p in positions
                                                           select new SelectListItem
                                                            {
                                                                Text = p.positionName,
                                                                Value = p.positionID.ToString()
                                                            };
                ViewBag.positions = selpositions;
            }
            return View(new labor());
        }
         
    // POST: Labor/Create
    [HttpPost]
        public ActionResult Create(labor laborModel, HttpPostedFileBase laborImg)
        {
            try
            {
            string folderPath = Server.MapPath("~/Content/img/upload/labor/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (laborImg != null && laborImg.ContentLength > 0)
            {
                if (laborImg.ContentType == "image/jpeg" || laborImg.ContentType == "image/jpg" || laborImg.ContentType == "image/png")
                {
                    var fileName = Path.GetFileName(laborImg.FileName);
                    var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/labor/"), fileName);
                    var fullPath = Server.MapPath("~/Content/img/upload/labor/") + laborImg.FileName;
                    if (System.IO.File.Exists(fullPath))
                    {
                        ViewBag.ActionMessage = "Same File already Exists";
                    }
                    else
                    {
                        laborImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        laborModel.laborImg = laborImg.FileName;
                    }
                }
                else
                {
                    ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                }
            }
            using (farmdb farmdb = new farmdb())
            {
                farmdb.labors.Add(laborModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("IndexSum", "Equipment");
        }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: Labor/Edit/5
        public ActionResult Edit(int id)
        {
            labor laborModel = new labor();
            using (farmdb farmdb = new farmdb())
            {
                laborModel = farmdb.labors.Where(x => x.IDlab == id).FirstOrDefault();

                List<labortype> labortypes = farmdb.labortypes.ToList();
                IEnumerable<SelectListItem> sellabortypes = from l in labortypes
                                                            select new SelectListItem
                                                            {
                                                                Text = l.laborT,
                                                                Value = l.laborID.ToString()
                                                            };
                ViewBag.labortypes = sellabortypes;

                List<position> positions = farmdb.positions.ToList();
                IEnumerable<SelectListItem> selpositions = from p in positions
                                                           select new SelectListItem
                                                           {
                                                               Text = p.positionName,
                                                               Value = p.positionID.ToString()
                                                           };
                ViewBag.positions = selpositions;
            }
            return View(laborModel);
        }

        // POST: Labor/Edit/5
        [HttpPost]
        public ActionResult Edit(labor laborModel, HttpPostedFileBase laborImg)
        {
            try
            {
                string folderPath = Server.MapPath("~/Content/img/upload/labor/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (laborImg != null && laborImg.ContentLength > 0)
                {
                    if (laborImg.ContentType == "image/jpeg" || laborImg.ContentType == "image/jpg" || laborImg.ContentType == "image/png")
                    {
                        var fileName = Path.GetFileName(laborImg.FileName);
                        var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/labor/"), fileName);
                        laborImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        laborModel.laborImg = laborImg.FileName;
                    }
                    else
                    {
                        ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                    }
                }
                using (farmdb farmdb = new farmdb())
                {
                    farmdb.Entry(laborModel).State = System.Data.Entity.EntityState.Modified;
                    farmdb.SaveChanges();
                }
                return RedirectToAction("IndexSum", "Equipment");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // GET: Labor/Delete/5
        public ActionResult Delete(int id)
        {
            labor laborModel = new labor();
            using (farmdb farmdb = new farmdb())
            {
                laborModel = farmdb.labors.Where(x => x.IDlab == id).FirstOrDefault();

                List<labortype> labortypes = farmdb.labortypes.ToList();
                IEnumerable<SelectListItem> sellabortypes = from l in labortypes
                                                            select new SelectListItem
                                                            {
                                                                Text = l.laborT,
                                                                Value = l.laborID.ToString()
                                                            };
                ViewBag.labortypes = sellabortypes;

                List<position> positions = farmdb.positions.ToList();
                IEnumerable<SelectListItem> selpositions = from p in positions
                                                           select new SelectListItem
                                                           {
                                                               Text = p.positionName,
                                                               Value = p.positionID.ToString()
                                                           };
                ViewBag.positions = selpositions;
            }
            return View(laborModel);
        }

        // POST: Labor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    labor laborModel = farmdb.labors.Where(x => x.IDlab == id).FirstOrDefault();
                    farmdb.labors.Remove(laborModel);
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
