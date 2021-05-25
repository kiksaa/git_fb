using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Data;
using Farmbook.Models;

namespace Farmbook.Controllers
{
    public class AddressController : Controller
    {
        // GET: Addess
        public ActionResult Index()
        {
            AddressModel model = new AddressModel();
            model.provinces = PopulateDropDown("SELECT provinceID, provinceName FROM province", "provinceName", "provinceID");
            return View(model);
        }
        [HttpPost]
        public ActionResult GetProvinces()
        {
            var pro = new Province();
            IEnumerable<SelectListItem> province = pro.GetProvinces();
            return Json(province, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAmphers(string provinceId)
        {
            if (!string.IsNullOrWhiteSpace(provinceId) && provinceId.Length == 3)
            {
                var amp = new Ampher();
                IEnumerable<SelectListItem> ampher = amp.GetAmphers(provinceId);
                return Json(ampher, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public ActionResult GetDistricts(string ampherId)
        {
            if (!string.IsNullOrWhiteSpace(ampherId) && ampherId.Length == 3)
            {
                var dis = new District();
                IEnumerable<SelectListItem> district = dis.GetDistricts(ampherId);
                return Json(district, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpPost]
        public JsonResult AjaxMethod(string type, int value)
        {
            AddressModel model = new AddressModel();
            switch (type)
            {
                case "provinceID":
                    model.amphers = PopulateDropDown("SELECT ampherID, ampherName FROM ampher WHERE provinceID = " + value, "ampherName", "ampherID");
                    break;
                case "ampherID":
                    model.districts = PopulateDropDown("SELECT districtID, districtName FROM district  WHERE ampherID = " + value, "districtName", "districtID");
                    break;
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Index(int provinceID, int ampherID, int districtID)
        {
            AddressModel model = new AddressModel();
            model.provinces = PopulateDropDown("SELECT provinceID, provinceName FROM province", "provinceName", "provinceID");
            model.amphers = PopulateDropDown("SELECT ampherID, ampherName FROM ampher WHERE provinceID = " + provinceID, "ampherName", "ampherID");
            model.districts = PopulateDropDown("SELECT districtID, districtName FROM district  WHERE ampherID = " + ampherID, "districtName", "districtID");
            return View(model);
        }

        private static List<SelectListItem> PopulateDropDown(string query, string textColumn, string valueColumn)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = "Data Source=localhost;database=farmdb;user id=root;password=swi1234;";
            using (MySqlConnection con = new MySqlConnection(constr))

            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr[textColumn].ToString(),
                                Value = sdr[valueColumn].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }
    }
}