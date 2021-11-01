using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;
using WebMatrix.WebData;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        // Return Home page.
        #region Index
        public ActionResult Index()
        {
            try
            {
                // count register
                List<register> registerList = new List<register>();
                List<landplot> landplotList = new List<landplot>();

                List<vehicle> vehicleList = new List<vehicle>();
                List<machine> MachineList = new List<machine>();
                List<equipment> EquipmentList = new List<equipment>();
                List<software> SoftwareList = new List<software>();
                List<staple> StapleList = new List<staple>();
                List<labor> LaborList = new List<labor>();
                List<fuel> FuelList = new List<fuel>();

                List<project> ProjectList = new List<project>();
                List<theory> TheoryList = new List<theory>();
                List<@event> EventList = new List<@event>();

                using (farmdbEntities farmdb = new farmdbEntities())
                {
                    registerList = farmdb.registers.Where(a => a.active == null).ToList();
                    var List = farmdb.registers.Where(a => a.active == 100).ToList();
                    ViewBag.TotalRegisters = registerList.Count() + List.Count();

                    landplotList = farmdb.landplots.Where(a => a.active == 100).ToList();
                    var landplot = farmdb.landplots.Where(a => a.active == null).ToList();
                    ViewBag.TotalLandPlot = landplotList.Count() + landplot.Count();

                    vehicleList = farmdb.vehicles.ToList<vehicle>();
                    ViewBag.TotalVehicle = vehicleList.Count();

                    MachineList = farmdb.machines.ToList<machine>();
                    ViewBag.TotalMachine = MachineList.Count();

                    EquipmentList = farmdb.equipments.ToList<equipment>();
                    ViewBag.TotalEquipment = EquipmentList.Count();

                    SoftwareList = farmdb.softwares.ToList<software>();
                    ViewBag.TotalSoftware = SoftwareList.Count();

                    StapleList = farmdb.staples.ToList<staple>();
                    ViewBag.TotalStaple = StapleList.Count();

                    LaborList = farmdb.labors.ToList<labor>();
                    ViewBag.TotalLabor = LaborList.Count();

                    FuelList = farmdb.fuels.ToList<fuel>();
                    ViewBag.TotalFuel = FuelList.Count();

                    ViewBag.Total = vehicleList.Count() + MachineList.Count() + EquipmentList.Count() + SoftwareList.Count() + StapleList.Count() + LaborList.Count() + FuelList.Count();

                    ProjectList = farmdb.projects.ToList<project>();
                    ViewBag.TotalProject = ProjectList.Count();

                    TheoryList = farmdb.theories.ToList<theory>();
                    ViewBag.TotalTheory = TheoryList.Count();

                    EventList = farmdb.events.ToList<@event>();
                    ViewBag.TotalEvent = EventList.Count();

                    profile profileModel = new profile();
                    profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                    ViewBag.status = profileModel.registerType.ToString();
                }
                return View(registerList);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Profile
        public ActionResult Profilea(string email)
        {
            login loginModel = new login();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                loginModel = farmdb.logins.Where(x => x.email == email).FirstOrDefault();
                List<profile> profileModel = farmdb.profiles.Where(p => p.email == User.Identity.Name).ToList();

                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from l in farmdb.profiles
                           join p in farmdb.provinces on l.province equals p.provinceID into plist
                           from p in plist.DefaultIfEmpty()
                           join a in farmdb.amphers on l.ampher equals a.ampherID into alist
                           from a in alist.DefaultIfEmpty()
                           join d in farmdb.districts on l.district equals d.districtID into dlist
                           from d in dlist.DefaultIfEmpty()
                           where l.email == User.Identity.Name
                           select new
                           {
                               l.ID,
                               l.name,
                               l.cradID,
                               l.email,
                               l.no,
                               l.moo,
                               l.birthday,
                               l.gender,
                               l.tel,
                               l.password,
                               p.provinceName,
                               a.ampherName,
                               d.districtName,
                               /* l.districtStr, l.ampherStr, l.provinceStr*/
                           };

                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.ID = item.ID;
                    objcvm.name = item.name;
                    objcvm.cardID = item.cradID;
                    objcvm.email = item.email;
                    objcvm.no = item.no;
                    objcvm.moo = item.moo;
                    objcvm.birthday = (DateTime)item.birthday;
                    objcvm.gender = item.gender;
                    objcvm.tel = item.tel;
                    objcvm.password = item.password;
                    objcvm.provinceName = item.provinceName;
                    objcvm.ampherName = item.ampherName;
                    objcvm.districtName = item.districtName;

                    ViewModeltList.Add(objcvm);
                }

                List<province> provinces = farmdb.provinces.ToList();
                IEnumerable<SelectListItem> selprovinces = from p in provinces
                                                           select new SelectListItem
                                                           {
                                                               Text = p.provinceName,
                                                               Value = p.provinceID.ToString()
                                                           };
                ViewBag.provinces = selprovinces;

                List<ampher> amphers = farmdb.amphers.ToList();
                IEnumerable<SelectListItem> selamphers = from a in amphers
                                                         select new SelectListItem
                                                         {
                                                             Text = a.ampherName,
                                                             Value = a.ampherID.ToString()
                                                         };
                ViewBag.amphers = selamphers;

                List<district> districts = farmdb.districts.ToList();
                IEnumerable<SelectListItem> seldistricts = from d in districts
                                                           select new SelectListItem
                                                           {
                                                               Text = d.districtName,
                                                               Value = d.districtID.ToString()
                                                           };
                ViewBag.districts = seldistricts;

                return View(ViewModeltList);
            }
        }
        #endregion
        //Return Register view
        public ActionResult Register()
        {
            return View();
        }

        #region SaveRegisterDetails
        [HttpPost]
        public ActionResult SaveRegisterDetails(profile registerDetails)
        {
            if (ModelState.IsValid)
            {
                using (var farmdb = new farmdbEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    profile reglog = new profile();
                    login log = new login();

                    reglog.name = registerDetails.name;
                    reglog.cradID = registerDetails.cradID;
                    reglog.gender = registerDetails.gender;
                    reglog.birthday = registerDetails.birthday;
                    reglog.email = registerDetails.email;
                    reglog.password = registerDetails.password;
                    reglog.registerType = registerDetails.registerType;
                    log.email = registerDetails.email;
                    log.password = registerDetails.password;

                    string password = registerDetails.password;
                    string email = registerDetails.email;

                    string Check = CheckValidUser(email);
                    if (Check == "Success")
                    {
                        string status = sendEmail(password, email);
                        farmdb.profiles.Add(reglog);
                        farmdb.logins.Add(log);
                        farmdb.SaveChanges();
                        ViewBag.Count = farmdb.profiles.SqlQuery(" SELECT * FROM profile ").Count();
                    }
                }
                if (ViewBag.Count > 0)
                {
                    return View("Login");
                }
                return View("Register");
            }
            else
            {
                return View("Register", registerDetails);
            }
        }
        #endregion

        #region CheckValidUser
        public string CheckValidUser(string email)
        {
            /*login model = new login();*/
            var farmdb = new farmdbEntities();
            List<login> loginModel = farmdb.logins.Where(l => l.email == email).ToList();
            if (loginModel.Count() > 0)
            {
                return ViewBag.Message1 = "อีเมล์นี้เคยลงทะเบียนใช้แล้ว";
            }
            return ViewBag.Message1 = "Success";
        }
        #endregion

        #region sendEmail
        public string sendEmail(string password, string email)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("adswi0112@gmail.com");
                mail.Subject = "ยืนยันตัวตนของคุณ " + email;
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = "คุณ " + email + "<br /><br />";
                htmlBody += "ขอบคุณสำหรับการลงทะเบียนฟาร์มบุ๊ค Farmbook ของเรา. และยินดีต้อนรับเข้าสู่ระบบ Farmbook ครับ/ค่ะ <br />";
                htmlBody += "<br/><b>ชื่อบัญชีของคุณคือ : </b>" + email + "<br />";
                htmlBody += "<b>รหัสผ่านของคุณคือ : </b>" + password + "<br />";
                htmlBody += "เข้าสู่ระบบได้ที่ลิงค์นี้ " + "http://farmbook.swi.co.th/Account/Login" + "<br /><br />";
                htmlBody += "ขอบคุณรับ/ค่ะ.";
                mail.Body = htmlBody;

                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("adswi0112@gmail.com", "adminswi0112");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                /*MessageBox.Show("mail Send");*/
                ViewBag.Message = "บันทึกเสร็จสิ้น!!";
                return ViewBag.Message2 = "โปรดตรวจสอบอีเมล์คุณ";
                /*Console.WriteLine(Mail.SendEMail(mailArgs, lstAttachments, true, dictHeaders).Item2);*/
            }
            catch
            {
                return ViewBag.Message2 = "อีเมล์คุณไม่ถูกต้อง";
            }
        }
        #endregion

        #region Login
        public ActionResult Login()
        {
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Login(login model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {
                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);
                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    /* if (model.email == "admin")
                     { 
                         return RedirectToAction("Index", "Account");
                     }
                     else
                     {
                         *//*return RedirectToAction("WebForm.aspx", "WebForm.aspx");*//*
                         return RedirectToAction("Create", "Register");
                     }*/

                    profile profileModel = new profile();
                    farmdbEntities farmdb = new farmdbEntities();
                    profileModel = farmdb.profiles.Where(e => e.email == model.email).FirstOrDefault();
                    var w = profileModel.registerType.ToString();
                    if (w == "100")
                    {
                        return RedirectToAction("Create", "Register");
                    }
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", " รหัสผ่านหรืออีเมล์คุณไม่ถูกต้อง ! ");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }
        #endregion

        #region IsValidUser
        //function to check if User is valid or not
        public login IsValidUser(login model)
        {
            using (var dataContext = new farmdbEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                login user = dataContext.logins.Where(query => query.email.Equals(model.email) && query.password.Equals(model.password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                {
                    return null;
                }
                //If user is not present false is returned.
                else
                {
                    return user;
                }
            }
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login");
        }
        #endregion

        #region ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            /*Create instance of entity model*/
            farmdbEntities farmdb = new farmdbEntities();
            login log = new login();
            /*Getting data from database for email validation*/
            List<login> loginModel = farmdb.logins.Where(l => l.email == email).ToList();

            if (loginModel.Count() > 0)
            {
                string password = loginModel.Select(p => p.password).First();
                string status = SendPassword(password, email);
                ViewBag.Message = "บัญชีถูกต้อง";
            }
            else
            {
                ViewBag.Message1 = "บัญชีนี้ไม่มีอยู่ในระบบ";
            }
            return View();
        }
        #endregion

        #region SendPassword
        public string SendPassword(string password, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("adswi0112@gmail.com");
                mail.Subject = "รหัสผ่านอีเมล์คุณ " + email;
                string userMessage = "";
                userMessage = userMessage + "<br/><b>อีเมล์ผู้ใช้คือ : </b> " + email;
                userMessage = userMessage + "<br/><b>รหัสผ่านของคุณคือ : </b> " + password;
                string Body = "คุณ " + email + ", <br/><br/>ข้อมูลบัญชีของคุณคือ : <br/></br> " + userMessage + "<br/><br/>Thanks";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //SMTP Server Address of gmail
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("adswi0112@gmail.com", "adminswi0112");
                /*smtp.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;*/
                //Smtp Email ID and Password For authentication
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return ViewBag.Message2 = "โปรดตรวจสอบอีเมล์เพื่อดูข้อมูลของคุณ";
            }
            catch
            {
                return ViewBag.Message2 = "บัญชีไม่ถูกต้อง";
            }
        }
        #endregion

        #region ResetPassword
        public ActionResult ResetPassword(string code, string email)
        {
            login model = new login();
            model.email = code;
            return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(login model)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(model.email, model.password);
                if (resetResponse)
                {
                    ViewBag.Message = "Successfully Changed";
                }
                else
                {
                    ViewBag.Message = "Something went horribly wrong!";
                }
            }
            return View(model);
        }
        #endregion
    }
}