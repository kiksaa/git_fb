using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Farmbook.Models
{
    public class AddressModel
    {
        public AddressModel()
        {
            this.provinces = new List<SelectListItem>();
            this.amphers = new List<SelectListItem>();
            this.districts = new List<SelectListItem>();
        }
        public List<SelectListItem> provinces { get; set; }
        public List<SelectListItem> amphers { get; set; }
        public List<SelectListItem> districts { get; set; }

        public int provinceID { get; set; }
        public int ampherID { get; set; }
        public int districtID { get; set; }

        [Display(Name = "Province")]
        public string provinceId { get; set; }
        public IEnumerable<SelectListItem> province { get; set; }

        [Required]
        [Display(Name = "Ampher")]
        public string ampherId { get; set; }
        public IEnumerable<SelectListItem> ampher { get; set; }

        [Required]
        [Display(Name = "District")]
        public string districtId { get; set; }
        public IEnumerable<SelectListItem> district { get; set; }
    }


}