using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ReportController : Controller
    {
        // Index
        #region Index
        public ActionResult Index()
        {
            try
            {
                farmdbEntities farmdb = new farmdbEntities();
                profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();
                return View();
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion
        // GET: Report
        #region GetLandplotList
        public IList<LandplotReport> GetLandplotList()
        {
            List<landplot> landplotrList = new List<landplot>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                landplotrList = farmdb.landplots.ToList<landplot>();
                List<LandplotReport> ViewModeltList = new List<LandplotReport>();
                var data = from l in farmdb.landplots
                           join p in farmdb.provinces on l.province equals p.provinceID into plist
                           from p in plist.DefaultIfEmpty()
                           join a in farmdb.amphers on l.ampher equals a.ampherID into alist
                           from a in alist.DefaultIfEmpty()
                           join d in farmdb.districts on l.district equals d.districtID into dlist
                           from d in dlist.DefaultIfEmpty()
                           join r in farmdb.registers on l.farmerName equals r.ID into rlist
                           from r in rlist.DefaultIfEmpty()
                           join t in farmdb.typeownerships on l.typeOwnership equals t.ID into tlist
                           from t in tlist.DefaultIfEmpty()
                           join li in farmdb.licenses on l.license equals li.ID into lilist
                           from li in lilist.DefaultIfEmpty()
                           join pro in farmdb.projects on l.projectName equals pro.ID into prolist
                           from pro in prolist.DefaultIfEmpty()
                           join s in farmdb.status on l.plotStatus equals s.statusID into slist
                           from s in slist.DefaultIfEmpty()
                           join th in farmdb.theories on l.theoryName equals th.ID into thlist
                           from th in thlist.DefaultIfEmpty()
                           join b in farmdb.buymethods on l.buyMethod equals b.ID into blist
                           from b in blist.DefaultIfEmpty()
                           join ac in farmdb.actives on l.active equals ac.activeID into aclist
                           from ac in aclist.DefaultIfEmpty()
                           select new
                           {
                               l.ID,
                               l.plotName,
                               l.coordinatesStar,
                               l.coordinatesEnd,
                               p.provinceName,
                               a.ampherName,
                               d.districtName,
                               r.name,
                               t.ownership,
                               li.licenseName,
                               pro.proName,
                               s.statusName,
                               l.areaPlot,
                               l.areaPlotS,
                               l.active,
                               l.lease_img,
                               l.license_img,
                               th.product,
                               th.workName,
                               l.areaCode,
                               l.landNumber,
                               l.administrator,
                               l.plotDetails,
                               b.nameBuy,
                               ac.activeName,
                               Totalarea = th.product * l.areaPlot,
                               l.note,
                               l.titleDeed,
                               l.landSlip,
                           };
                foreach (var item in data)
                {
                    LandplotReport objcvm = new LandplotReport();
                    objcvm.ID = item.ID;
                    objcvm.plotName = item.plotName;
                    objcvm.coordinatesStar = item.coordinatesStar;
                    objcvm.coordinatesEnd = item.coordinatesEnd;
                    objcvm.provinceName = item.provinceName;
                    objcvm.ampherName = item.ampherName;
                    objcvm.districtName = item.districtName;
                    objcvm.name = item.name;
                    objcvm.ownership = item.ownership;
                    objcvm.licenseName = item.licenseName;
                    objcvm.lease_img = item.lease_img;
                    objcvm.license_img = item.license_img;
                    objcvm.projectName = item.proName;
                    objcvm.plotStatus = item.statusName;
                    objcvm.areaPlot = item.areaPlot;
                    objcvm.areaPlotS = item.areaPlotS;
                    objcvm.product = (float)item.Totalarea;
                    objcvm.workName = item.workName;
                    objcvm.areaCode = item.areaCode;
                    objcvm.landNumber = item.landNumber;
                    objcvm.administrator = item.administrator;
                    objcvm.plotDetails = item.plotDetails;
                    objcvm.nameBuy = item.nameBuy;
                    objcvm.activeName = item.activeName;
                    objcvm.note = item.note;
                    objcvm.titleDeed = item.titleDeed;
                    objcvm.landSlip = item.landSlip;
                    ViewModeltList.Add(objcvm);
                }
                return ViewModeltList;
            }
        }
        #endregion

        #region GetRegisterList
        public IList<RegisterReport> GetRegisterList()
        {
            List<register> registerList = new List<register>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                registerList = farmdb.registers.ToList<register>();
                List<RegisterReport> ViewModeltList = new List<RegisterReport>();
                var data = from r in farmdb.registers
                           join p in farmdb.provinces on r.province equals p.provinceID into plist
                           from p in plist.DefaultIfEmpty()
                           join a in farmdb.amphers on r.ampher equals a.ampherID into alist
                           from a in alist.DefaultIfEmpty()
                           join d in farmdb.districts on r.district equals d.districtID into dlist
                           from d in dlist.DefaultIfEmpty()
                               /*join s in farmdb.status on r.status equals s.statusID into slist
                               from s in slist.DefaultIfEmpty()*/
                           join b in farmdb.bankusers on r.bank equals b.ID into blist
                           from b in blist.DefaultIfEmpty()
                           join l in farmdb.landplots on r.ID equals l.farmerName into llist
                           from l in llist.DefaultIfEmpty()
                           join g in farmdb.genders on r.gender equals g.genderID into glist
                           from g in glist.DefaultIfEmpty()
                           join f in farmdb.families on r.family equals f.familyID into flist
                           from f in flist.DefaultIfEmpty()
                           join ac in farmdb.actives on r.active equals ac.activeID into aclist
                           from ac in aclist.DefaultIfEmpty()
                           select new
                           {
                               r.ID,
                               r.name,
                               r.registerID,
                               r.cardID,
                               r.no,
                               r.moo,
                               r.road,
                               r.card_img,
                               r.farmer_img,
                               p.provinceName,
                               a.ampherName,
                               d.districtName, /*s.statusName,*/
                               r.dateUpdate,
                               r.adminBy,
                               ac.activeName,
                               g.genderName,
                               r.birthday,
                               r.tel,
                               r.email,
                               f.familyName,
                               r.comment,
                               b.bankName,
                               TotalLandplot = llist.Where(a => a.active == null || a.active == 100).Count(),
                           };
                foreach (var item in data.Distinct())
                {
                    RegisterReport objcvm = new RegisterReport();
                    objcvm.ID = item.ID;
                    objcvm.name = item.name;
                    objcvm.registerID = item.registerID;
                    objcvm.cardID = item.cardID;
                    objcvm.no = item.no;
                    objcvm.moo = item.moo;
                    objcvm.road = item.road;
                    objcvm.card_img = item.card_img;
                    objcvm.farmer_img = item.farmer_img;
                    objcvm.province = item.provinceName;
                    objcvm.ampher = item.ampherName;
                    objcvm.district = item.districtName;
                    /*objcvm.status = item.statusName;*/
                    objcvm.dateUpdate = item.dateUpdate;
                    objcvm.adminBy = item.adminBy;
                    objcvm.active = item.activeName;
                    objcvm.gender = item.genderName;
                    objcvm.birthday = item.birthday;
                    objcvm.tel = item.tel;
                    objcvm.email = item.email;
                    objcvm.family = item.familyName;
                    objcvm.comment = item.comment;
                    objcvm.bank = item.bankName;
                    objcvm.areaNumber = item.TotalLandplot;
                    ViewModeltList.Add(objcvm);
                    ViewBag.TotalRegister = ViewModeltList.Count();
                }
                return ViewModeltList;
            }
        }
        #endregion

        #region GetTheoryList
        public IList<TheoryReport> GetTheoryList()
        {
            List<theory> theoryList = new List<theory>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                theoryList = farmdb.theories.ToList<theory>();
                List<TheoryReport> ViewModeltList = new List<TheoryReport>();
                var data = from t in farmdb.theories
                           join tt in farmdb.theorytypes on t.workProcedure equals tt.theoryID into tlist
                           from tt in tlist.DefaultIfEmpty()
                           join a in farmdb.accesses on t.access equals a.accessID into alist
                           from a in alist.DefaultIfEmpty()
                           join p in farmdb.projects on t.project equals p.ID into plist
                           from p in plist.DefaultIfEmpty()
                           join ac in farmdb.activities on t.ID equals ac.plan into aclist
                           from ac in aclist.DefaultIfEmpty()
                           where t.ID != ac.plan
                           select new
                           {
                               t.ID,
                               t.dateUpdate,
                               t.productType,
                               t.workName,
                               a.accessName,
                               t.reference,
                               tt.theoryName,
                               t.farmingType,
                               t.type,
                               t.sepecies,
                               t.detail,
                               t.product,
                               p.proName,
                               ac.stepNum,
                               ac.stepName,
                               ac.activity1,
                               ac.notice
                           };
                foreach (var item in data)
                {
                    TheoryReport objcvm = new TheoryReport();
                    objcvm.ID = item.ID;
                    objcvm.dateUpdate = item.dateUpdate;
                    objcvm.productType = item.productType;
                    objcvm.workName = item.workName;
                    objcvm.access = item.accessName;
                    objcvm.reference = item.reference;
                    objcvm.workProcedure = item.theoryName;
                    objcvm.farmingType = item.farmingType;
                    objcvm.type = item.type;
                    objcvm.sepecies = item.sepecies;
                    objcvm.detail = item.detail;
                    objcvm.product = item.product;
                    objcvm.project = item.proName;
                    objcvm.stepNum = item.stepNum;
                    objcvm.stepName = item.stepName;
                    objcvm.activity1 = item.activity1;
                    objcvm.notice = item.notice;
                    ViewModeltList.Add(objcvm);
                }
                #region Data2
                var data2 = from t in farmdb.theories
                            join tt in farmdb.theorytypes on t.workProcedure equals tt.theoryID into tlist
                            from tt in tlist.DefaultIfEmpty()
                            join a in farmdb.accesses on t.access equals a.accessID into alist
                            from a in alist.DefaultIfEmpty()
                            join p in farmdb.projects on t.project equals p.ID into plist
                            from p in plist.DefaultIfEmpty()
                            join ac in farmdb.activities on t.ID equals ac.plan into aclist
                            from ac in aclist.DefaultIfEmpty()
                            where t.ID == ac.plan
                            select new
                            {
                                t.ID,
                                t.dateUpdate,
                                t.productType,
                                t.workName,
                                a.accessName,
                                t.reference,
                                tt.theoryName,
                                t.farmingType,
                                t.type,
                                t.sepecies,
                                t.detail,
                                t.product,
                                p.proName,
                                ac.stepNum,
                                ac.stepName,
                                ac.age,
                                ac.time,
                                ac.activity1,
                                ac.notice
                            };
                foreach (var item in data2)
                {
                    TheoryReport objcvm = new TheoryReport();
                    objcvm.ID = item.ID;
                    objcvm.dateUpdate = item.dateUpdate;
                    objcvm.productType = item.productType;
                    objcvm.workName = item.workName;
                    objcvm.access = item.accessName;
                    objcvm.reference = item.reference;
                    objcvm.workProcedure = item.theoryName;
                    objcvm.farmingType = item.farmingType;
                    objcvm.type = item.type;
                    objcvm.sepecies = item.sepecies;
                    objcvm.detail = item.detail;
                    objcvm.product = item.product;
                    objcvm.project = item.proName;
                    objcvm.stepNum = item.stepNum;
                    objcvm.stepName = item.stepName;
                    objcvm.age = item.age;
                    objcvm.time = item.time;
                    objcvm.activity1 = item.activity1;
                    objcvm.notice = item.notice;
                    ViewModeltList.Add(objcvm);
                }
                #endregion
                return ViewModeltList;
            }
        }
        #endregion

        #region GetProjectList
        public IList<ProjectReport> GetProjectList()
        {
            List<project> ProjectList = new List<project>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                ProjectList = farmdb.projects.ToList<project>();
                List<ProjectReport> ViewModeltList = new List<ProjectReport>();
                var data = from p in farmdb.projects
                           join b in farmdb.buymethods on p.buyMethod equals b.ID into blist
                           from b in blist.DefaultIfEmpty()
                           join s in farmdb.standards on p.manuStandards equals s.ID into slist
                           from s in slist.DefaultIfEmpty()
                           join sl in farmdb.standardlists on p.ID equals sl.IDpro into sllist
                           from sl in sllist.DefaultIfEmpty()
                           select new
                           {
                               p.ID,
                               p.dataNow,
                               p.proName,
                               b.nameBuy,
                               s.standardName,
                               p.detail,
                               sl.IDlist,
                               sl.list,
                               sl.img,
                               sl.fillin
                           };
                foreach (var item in data)
                {
                    ProjectReport objcvm = new ProjectReport();
                    objcvm.ID = item.ID;
                    objcvm.dataNow = item.dataNow;
                    objcvm.proName = item.proName;
                    objcvm.buyMethod = item.nameBuy;
                    objcvm.manuStandards = item.standardName;
                    objcvm.detail = item.detail;
                    objcvm.IDlist = item.IDlist;
                    objcvm.list = item.list;
                    objcvm.img = item.img;
                    objcvm.fillin = item.fillin;
                    ViewModeltList.Add(objcvm);
                }
                return ViewModeltList;
            }
        }
        #endregion

        #region GetEventList
        public IList<EventReport> GetEventList()
        {
            List<@event> EventList = new List<@event>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                EventList = farmdb.events.ToList<@event>();
                List<EventReport> ViewModeltList = new List<EventReport>();
                var data = from e in farmdb.events
                           select new
                           {
                               e.ID,
                               e.subject,
                               e.start,
                               e.end,
                               e.description,
                               e.isFullDay
                           };
                foreach (var item in data)
                {
                    EventReport objcvm = new EventReport();
                    objcvm.ID = item.ID;
                    objcvm.subject = item.subject;
                    objcvm.start = item.start;
                    objcvm.end = item.end;
                    objcvm.description = item.description;
                    objcvm.isFullDay = item.isFullDay;
                    ViewModeltList.Add(objcvm);
                }
                return ViewModeltList;
            }
        }
        #endregion
        // Load File
        #region ExportToExcel
        public ActionResult ExportToExcel(int id)
        {
            var gv = new GridView();
            if (id == 1)
            {
                gv.DataSource = GetLandplotList().ToList();
                /*gv1.DataSource = GetRegisterList().ToList();*/
                /* Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();

                 // Access the worksheet collection.
                 WorksheetCollection worksheets = workbook.Worksheets;

                 // Access the first worksheet.
                 Aspose.Cells.Worksheet worksheet1 = workbook.Worksheets[0];
                 // Access the worksheet with the specified name.
                 Aspose.Cells.Worksheet worksheet2 = workbook.Worksheets["MainSheet"];
                 worksheet2.Cells["A"].Value = GetLandplotList();*/
            }
            if (id == 2)
            {
                gv.DataSource = GetRegisterList().ToList();
            }
            if (id == 3)
            {
                gv.DataSource = GetTheoryList().ToList();
            }
            if (id == 4)
            {
                gv.DataSource = GetProjectList().ToList();
            }
            if (id == 5)
            {
                gv.DataSource = GetEventList().ToList();
            }
            gv.DataBind();
            /*gv1.DataBind();*/
            Response.ClearContent();
            Response.Buffer = true;
            if (id == 1)
            {
                /*DataTable dt = (DataTable)GetLandplotList();
                DataTable dt1 = (DataTable)GetRegisterList();
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt);
                wb.Worksheets.Add(dt1);*/
                Response.AddHeader("content-disposition", "attachment; filename=LandplotReport.xls");
            }
            if (id == 2)
            {
                Response.AddHeader("content-disposition", "attachment; filename=RegisterReport.xls");
            }
            if (id == 3)
            {
                Response.AddHeader("content-disposition", "attachment; filename=ThoryReport.xls");
            }
            if (id == 4)
            {
                Response.AddHeader("content-disposition", "attachment; filename=ProjectReport.xls");
            }
            if (id == 5)
            {
                Response.AddHeader("content-disposition", "attachment; filename=EventReport.xls");
            }
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }
        #endregion
    }
}