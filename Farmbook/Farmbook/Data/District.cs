using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Data
{
    public class District
    {
        public IEnumerable<SelectListItem> GetDistricts()
        {
            List<SelectListItem> district = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return district;
        }
        public IEnumerable<SelectListItem> GetDistricts(string ampherId)
        {
            if (!String.IsNullOrWhiteSpace(ampherId))
            {
                using (var context = new farmdb())
                {
                    IEnumerable<SelectListItem> district = context.districts.AsNoTracking()
                        .OrderBy(n => n.districtName)
                        .Where(n => n.districtID.ToString() == ampherId)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.districtID.ToString(),
                               Text = n.districtName
                           }).ToList();
                    return new SelectList(district, "Value", "Text");
                }
            }
            return null;
        }
    }
}