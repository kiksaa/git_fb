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
            List<equipment> EquipmentList = new List<equipment>();
            using (farmdb farmdb = new farmdb())
            {
                registerList = farmdb.registers.ToList<register>();
                ViewBag.TotalRegisters = registerList.Count();

                landplotrList = farmdb.landplots.ToList<landplot>();
                ViewBag.TotalLandPlot = landplotrList.Count();

                EquipmentList = farmdb.equipments.ToList<equipment>();
                ViewBag.TotalEquipment = EquipmentList.Count();
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
                var _emailService = 
                ViewBag.Message = "บันทึกเสร็จสิ้น!!";
                return View("Register");
            }
            else
            {
                return View("Register", registerDetails);
            }
        }
        /*[HttpPost]
        public ViewResult Email(Models.profile _objModelMail)
        {
            if (ModelState.IsValid)
            {
                profile mail = new profile();
                mail.email.Add(_objModelMail.email);
                mail.From = new MailAddress("samatcha@gmail.com");
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("username", "password"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
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
