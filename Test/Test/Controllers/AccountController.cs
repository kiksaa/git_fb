using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using System.Data.Entity;
using System.Data;
using System.Web.Security;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Test.Controllers
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
            }
            return View(registerList);
        }

        //Return Register view
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Profile(String email)
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
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
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

                    var check = farmdb.profiles.FirstOrDefault(s => s.email == registerDetails.email);
                    if (check == null)
                    {
                        registerDetails.password = GetMD5(registerDetails.password);
                        farmdb.Configuration.ValidateOnSaveEnabled = false;
                        farmdb.profiles.Add(registerDetails);
                        farmdb.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Email already exists";
                        return View();
                    }
                }
                /*var _emailService = */
                ViewBag.Message = "บันทึกเสร็จสิ้น!!";
                return View("Register");
            }
            else
            {
                return View("Register", registerDetails);
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
        // GET: /Account/ConfirmEmail
        /*[AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }*/
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
                    if (model.email == "admin")
                    { 
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        /*return RedirectToAction("WebForm.aspx", "WebForm.aspx");*/
                        return RedirectToAction("Index", "Account");
                    }
                    
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
                    return null;
                //If user is not present false is returned.
                else
                    return user;

            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }
    }
}
