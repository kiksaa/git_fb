using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmbook.Models;

namespace Farmbook.Data
{
    public class Ampher
    {
        public IEnumerable<SelectListItem> GetAmphers()
        {
            List<SelectListItem> ampher = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return ampher;
        }
        public IEnumerable<SelectListItem> GetAmphers(string provinceId)
        {
            if (!String.IsNullOrWhiteSpace(provinceId))
            {
                using (var context = new farmdb())
                {
                    IEnumerable<SelectListItem> ampher = context.amphers.AsNoTracking()
                        .OrderBy(n => n.ampherName)
                        .Where(n => n.ampherID.ToString() == provinceId)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.ampherID.ToString(),
                               Text = n.ampherName
                           }).ToList();
                    return new SelectList(ampher, "Value", "Text");
                }
            }
            return null;
        }


    }
}