using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;
using System.Data.Entity;
using System.Data;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebMatrix.WebData;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Farmbook.Controllers
{
    public class AccountController : Controller
    {
        // Return Home page.
        public ActionResult Index()
        {
            // count register
            List<register> registerList = new List<register>();
            List<landplot> landplotrList = new List<landplot>();

            List<vehicle> vehicleList = new List<vehicle>();
            List<machine> MachineList = new List<machine>();
            List<equipment> EquipmentList = new List<equipment>();
            List<software> SoftwareList = new List<software>();
            List<staple> StapleList = new List<staple>();
            List<labor> LaborList = new List<labor>();
            List<fuel> FuelList = new List<fuel>();
            
            List<projectand> ProjectandList = new List<projectand>();
            List<theory> TheoryList = new List<theory>();
            
            using (farmdb farmdb = new farmdb())
            {
                registerList = farmdb.registers.ToList<register>();
                ViewBag.TotalRegisters = registerList.Count();

                landplotrList = farmdb.landplots.ToList<landplot>();
                ViewBag.TotalLandPlot = landplotrList.Count();

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

                ProjectandList = farmdb.projectands.ToList<projectand>();
                ViewBag.TotalProjectand = ProjectandList.Count();

                TheoryList = farmdb.theories.ToList<theory>();
                ViewBag.TotalTheory = TheoryList.Count();
            }
            return View(registerList);
        }

        public ActionResult Profile(string email)
        {
            login loginModel = new login();
            using (farmdb farmdb = new farmdb())
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
                               l.ID, l.name, l.cradID, l.email, l.no, l.moo, l.birthday,
                               l.gender, l.tel, l.password, p.provinceName, a.ampherName, d.districtName,
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
        //Return Register view
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveRegisterDetails(profile registerDetails)
        {
            if (ModelState.IsValid)
            {
                using (var farmdb = new farmdb())
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

                    farmdb.profiles.Add(reglog);
                    farmdb.logins.Add(log);
                    farmdb.SaveChanges();

                    ViewBag.Count = farmdb.profiles.SqlQuery(" SELECT * FROM profile ").Count();
                }
                /*var _emailService = */
                string password = registerDetails.password;
                string email = registerDetails.email;
                string status = sendEmail(password, email);
                ViewBag.Message = "บันทึกเสร็จสิ้น!!";
                return View("Login");
            }
            else
            {
                return View("Register", registerDetails);
            }
        }
        public string sendEmail(string password, string email)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpClient SMTPServer = new SmtpClient("127.0.0.1");
                var mail = new MailMessage();
                mail.From = new MailAddress("adswi0112@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Please activate your account.";
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = "Dear " + email + "<br /><br />";
                htmlBody += "Thank you for registering an account.  Please activate your account by visiting the URL below:<br /><br />";
                htmlBody += "<br/><b>Passsword: </b>" + password;
                /*htmlBody += "http://localhost:44357/signin.aspx?activate=" + password + "<br /><br />";*/
                htmlBody += "Thank you.";
                mail.Body = htmlBody;
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("adswi0112@gmail.com", "adminswi0112");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                /*MessageBox.Show("mail Send");*/
                return "Please check your email for account login detail.";
                /*Console.WriteLine(Mail.SendEMail(mailArgs, lstAttachments, true, dictHeaders).Item2);*/
            }
            catch
            {
                return "Error............";
            }
        }
        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "YourEmail@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }
        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("YourEmail@gmail.com", "Password");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult CheckValidUser(login model)
        {
            var farmdb = new farmdb();
            string result = "Fail";
            var DataItem = farmdb.logins.Where(x => x.email == model.email && x.password == model.password).SingleOrDefault();
            if (DataItem != null)
            {
                Session["UserID"] = DataItem.ID.ToString();
                Session["UserName"] = DataItem.email.ToString();
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
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
                    return RedirectToAction("Create", "Register");

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

        //function to check if User is valid or not
        public login IsValidUser(login model)
        {
            using (var dataContext = new farmdb())
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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        /*[HttpPost]
        public ActionResult ForgotPassword(string UserName)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.UserExists(UserName))
                {
                    string To = UserName, UserID, Password, SMTPPort, Host;
                    string token = WebSecurity.GeneratePasswordResetToken(UserName);
                    if (token == null)
                    {
                        // If user does not exist or is not confirmed.
                        return View("Index");
                    }
                    else
                    {
                        //Create URL with above token
                        var lnkHref = "<a href='" + Url.Action("ResetPassword", "Account", new { email = UserName, code = token }, "http") + "'>Reset Password</a>";
                        //HTML Template for Send email
                        string subject = "Your changed password";
                        string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;
                        //Get and set the AppSettings using configuration manager.
                        EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);
                        //Call send email methods.
                        EmailManager.SendEmail(UserID, subject, body, To, UserID, Password, SMTPPort, Host);
                    }
                }
            }
            return View();
        }*/
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            /*Create instance of entity model*/
            farmdb farmdb = new farmdb();
            login log = new login();
            /*Getting data from database for email validation*/
            List<login> loginModel = farmdb.logins.Where(l => l.email == email).ToList();
            string password = loginModel.Select(p => p.password).First();
            /*var data = (from login in farmdb.logins
                        where login.email == email
                        select login.email);*/
            if (loginModel.Count() > 0)
            {
                string status = SendPassword(password, email);
                ViewBag.Message = 1;
            }
            else
            {
                ViewBag.Message = 0;
            }
            return View();
        }
        public string SendPassword(string password, string email)
         {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("adswi0112@gmail.com");
                mail.Subject = "Your password for account " + email;
                string userMessage = "";
                userMessage = userMessage + "<br/><b>Login Id:</b> " + email;
                userMessage = userMessage + "<br/><b>Passsword: </b>" + password;

                string Body = "Dear " + email + ", <br/><br/>Login detail for your account is a follows:<br/></br> " + userMessage + "<br/><br/>Thanks";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //SMTP Server Address of gmail
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("adswi0112@gmail.com", "adminswi0112");
                /*smtp.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;*/
                // Smtp Email ID and Password For authentication
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return "Please check your email for account login detail.";
            }
            catch
            {
                return "Error............";
            }
        }
        public class EmailManager
        {
            public static void AppSettings(out string UserID, out string Password, out string SMTPPort, out string Host)
            {
                UserID = ConfigurationManager.AppSettings.Get("UserID");
                Password = ConfigurationManager.AppSettings.Get("Password");
                SMTPPort = ConfigurationManager.AppSettings.Get("SMTPPort");
                Host = ConfigurationManager.AppSettings.Get("Host");
            }
            public static void SendEmail(string From, string Subject, string Body, string To, string UserID, string Password, string SMTPPort, string Host)
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress(From);
                mail.Subject = Subject;
                mail.Body = Body;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.Port = Convert.ToInt16(SMTPPort);
                smtp.Credentials = new NetworkCredential(UserID, Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
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

    }
}
