using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class MachineController : Controller
    {
        // GET: Machine
        public ActionResult Index()
        {
            List<machine> machineList = new List<machine>();
            using (farmdb farmdb = new farmdb())
            {
                machineList = farmdb.machines.ToList<machine>();
                ViewBag.TotalMachine = machineList.Count();
                List<ViewModel> ViewModeltList = new List<ViewModel>();

                var data = from m in farmdb.machines
                            join t in farmdb.machinetypes on m.machineType equals t.machineID into mlist
                            from t in mlist.DefaultIfEmpty()
                            select new
                            {
                                t.machineT,
                                m.machineID,
                                m.machineName,
                                m.detail,
                                m.price,
                                m.IDmac
                            };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.machineType = item.machineT;
                    objcvm.machineName = item.machineName;
                    objcvm.machineID = item.machineID;
                    objcvm.detailM = item.detail;
                    objcvm.priceM = item.price;
                    objcvm.IDmac = item.IDmac;
                    ViewModeltList.Add(objcvm);
                }

                return View(ViewModeltList);
            }
        }

        // GET: Machine/Details/5
        public ActionResult Details(int id)
        {
            machine machineModel = new machine();
            using (farmdb farmdb = new farmdb())
            {
                machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();
            }
            return View(machineModel);
        }

        // GET: Machine/Create
        public ActionResult Create()
        {
            using (farmdb farmdb = new farmdb())
            {
                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(new machine());
        }
        // POST: Machine/Create
        [HttpPost]
        public ActionResult Create(machine machineModel, HttpPostedFileBase machineImg)
        {
            string folderPath = Server.MapPath("~/Content/img/upload/vehicle/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (machineImg != null && machineImg.ContentLength > 0)
            {
                if (machineImg.ContentType == "image/jpeg" || machineImg.ContentType == "image/jpg" || machineImg.ContentType == "image/png")
                {
                    var fileName = Path.GetFileName(machineImg.FileName);
                    var userfolderpath = Path.Combine(Server.MapPath("~/Content/img/upload/vehicle/"), fileName);
                    var fullPath = Server.MapPath("~/Content/img/upload/vehicle/") + machineImg.FileName;
                    if (System.IO.File.Exists(fullPath))
                    {
                        ViewBag.ActionMessage = "Same File already Exists";
                    }
                    else
                    {
                        machineImg.SaveAs(userfolderpath);
                        ViewBag.ActionMessage = "File has been uploaded successfully";
                        machineModel.machineImg = machineImg.FileName;
                    }
                }
                else
                {
                    ViewBag.ActionMessage = "Please upload only imag (jpg,gif,png)";
                }
            }
            using (farmdb farmdb = new farmdb())
            {
                farmdb.machines.Add(machineModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("IndexSum", "Equipment");
        }

        // GET: Machine/Edit/5
        public ActionResult Edit(int id)
        {
            machine machineModel = new machine();
            using (farmdb farmdb = new farmdb())
            {
                machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();

                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(machineModel);
        }

        // POST: Machine/Edit/5
        [HttpPost]
        public ActionResult Edit(machine machineModel)
        {
            using (farmdb farmdb = new farmdb())
            {
                farmdb.Entry(machineModel).State = System.Data.Entity.EntityState.Modified;
                farmdb.SaveChanges();
            }
            return RedirectToAction("IndexSum", "Equipment");
        }

        // GET: Machine/Delete/5
        public ActionResult Delete(int id)
        {
            machine machineModel = new machine();
            using (farmdb farmdb = new farmdb())
            {
                machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();
                List<machinetype> machinetypes = farmdb.machinetypes.ToList();
                IEnumerable<SelectListItem> selmachinetypes = from m in machinetypes
                                                              select new SelectListItem
                                                              {
                                                                  Text = m.machineT,
                                                                  Value = m.machineID.ToString()
                                                              };
                ViewBag.machinetypes = selmachinetypes;

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
            return View(machineModel);
        }

        // POST: Machine/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (farmdb farmdb = new farmdb())
            {
                machine machineModel = farmdb.machines.Where(x => x.IDmac == id).FirstOrDefault();
                farmdb.machines.Remove(machineModel);
                farmdb.SaveChanges();
            }
            return RedirectToAction("IndexSum", "Equipment");
        }
    }
}
