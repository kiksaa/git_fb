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
        #region Index
        // GET: Labor
        public ActionResult Index()
        {
            List<labor> laborList = new List<labor>();
            using (farmdb farmdb = new farmdb())
            {
                laborList = farmdb.labors.ToList<labor>();
                ViewBag.TotalLabor = laborList.Count();

                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();

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
        #endregion
        #region Details
        // GET: Labor/Details/5
        public ActionResult Details(int id)
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
        #endregion
        #region Create
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
        public ActionResult Create(labor laborModel, filedetail filesModel)
        {
            try
            {
                using (farmdb farmdb = new farmdb())
                {
                    if (laborModel.file_laborImg != null)
                    {
                        String FileExt = Path.GetExtension(laborModel.file_laborImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[laborModel.file_laborImg.ContentLength];
                                laborModel.file_laborImg.InputStream.Read(data, 0, laborModel.file_laborImg.ContentLength);
                                filesModel.fileName = "labor_" + laborModel.file_laborImg.FileName;
                                filesModel.fileData = data;
                                laborModel.laborImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
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
        #endregion
        #region Edit
        // GET: Labor/Edit/5
        public ActionResult Edit(int id)
        {
            labor laborModel = new labor();
            using (farmdb farmdb = new farmdb())
            {
                laborModel = farmdb.labors.Where(x => x.IDlab == id).FirstOrDefault();

                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == laborModel.laborImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == laborModel.laborImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }

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
                filedetail filesModel = new filedetail();
                using (farmdb farmdb = new farmdb())
                {
                    if (laborModel.file_laborImg != null)
                    {
                        String FileExt = Path.GetExtension(laborModel.file_laborImg.FileName).ToUpper();
                        if (FileExt != null)
                        {
                            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                            {
                                Byte[] data = new byte[laborModel.file_laborImg.ContentLength];
                                laborModel.file_laborImg.InputStream.Read(data, 0, laborModel.file_laborImg.ContentLength);
                                filesModel.fileName = "labor_" + laborModel.file_laborImg.FileName;
                                filesModel.fileData = data;
                                laborModel.laborImg = filesModel.fileName;
                            }
                            else
                            {
                                ViewBag.FileStatus = "Invalid file format.";
                                return View();
                            }
                        }
                        farmdb.filedetails.Add(filesModel);
                    }
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
        #endregion
        #region Delete
        // GET: Labor/Delete/5
        public ActionResult Delete(int id)
        {
            labor laborModel = new labor();
            using (farmdb farmdb = new farmdb())
            {
                laborModel = farmdb.labors.Where(x => x.IDlab == id).FirstOrDefault();
                filedetail filedetailModel = farmdb.filedetails.Where(f => f.fileName == laborModel.laborImg).FirstOrDefault();
                if (filedetailModel != null)
                {
                    if (filedetailModel.fileName == laborModel.laborImg)
                    {
                        ViewBag.img = filedetailModel.fileData;
                    }
                }
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
                    filedetail filesModel = farmdb.filedetails.Where(f => f.fileName == laborModel.laborImg).FirstOrDefault();
                    farmdb.labors.Remove(laborModel);
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
