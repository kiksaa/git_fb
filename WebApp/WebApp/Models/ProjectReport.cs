using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ProjectReport
    {
        [DisplayName("ลำดับที่")]
        public int ID { get; set; }

        [DisplayName("วันที่ปรับปรุง/แก้ไข")]
        public System.DateTime dataNow { get; set; }
        [DisplayName("ชื่อโครงการ")]
        public string proName { get; set; }
        [DisplayName("วิธีการรับซื้อ")]
        public string buyMethod { get; set; }
        [DisplayName("มาตรฐานการผลิต")]
        public string manuStandards { get; set; }
        [DisplayName("รายละเอียด")]
        public string detail { get; set; }
        [DisplayName("ลำดับรายการที่")]
        public Nullable<int> IDlist { get; set; }
        [DisplayName("รายการ")]
        public string list { get; set; }
        [DisplayName("กรอกข้อมูล")]
        public Nullable<bool> fillin { get; set; }
        [DisplayName("รูปภาพ")]
        public Nullable<bool> img { get; set; }
    }
}