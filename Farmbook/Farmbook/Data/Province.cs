using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Data
{
    public class Province
    {
        public IEnumerable<SelectListItem> GetProvinces()
        {
            using (var context = new farmdb())
            {
                List<SelectListItem> province = context.provinces.AsNoTracking()
                    .OrderBy(n => n.provinceName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.provinceID.ToString(),
                            Text = n.provinceName
                        }).ToList();
                var provinceip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select province ---"
                };
                province.Insert(0, provinceip);
                return new SelectList(province, "Value", "Text");
            }
        }
    }
}