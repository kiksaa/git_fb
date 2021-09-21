using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Farmbook.Models
{
    public class TheoryReport
    {
        [DisplayName("ลำดับที่")]
        public int ID { get; set; }
        [DisplayName("ชื่อขั้นตอนการทำงาน")]
        public string workName { get; set; }
        [DisplayName("ขั้นตอนการทำงาน")]
        public string workProcedure { get; set; }
        [DisplayName("การเช้าถึง")]
        public string access { get; set; }
        [DisplayName("ประเภทกสิกรรม")]
        public string farmingType { get; set; }
        [DisplayName("ชนิด")]
        public string type { get; set; }
        [DisplayName("สายพันธุ์")]
        public string sepecies { get; set; }
        [DisplayName("ปริมาณผลผลิต(กิโลกรัม)")]
        public float product { get; set; }
        [DisplayName("ผลิตภัณฑ์ที่ได้")]
        public string productType { get; set; }
        [DisplayName("โครงการ")]
        public string project { get; set; }
        
        [DisplayName("แหล่งอ้างอิง")]
        public string reference { get; set; }
        [DisplayName("รายละเอียด")]
        public string detail { get; set; }
        
        [DisplayName("ขั้นตอนที่")]
        public Nullable<int> stepNum { get; set; }
        [DisplayName("ชื่อขั้นตอน")]
        public string stepName { get; set; }
        [DisplayName("อายุ(วัน)")]
        public Nullable<int> age { get; set; }
        [DisplayName("ระยะเวลา(วัน)")]
        public Nullable<int> time { get; set; }
        [DisplayName("กิจกกรรม")]
        public string activity1 { get; set; }
        [DisplayName("ข้อสังเกต")]
        public string notice { get; set; }
        /*[DisplayName("ชื่อขั้นตอนการทำงาน")]
        public int plan { get; set; }*/
        [DisplayName("วันที่ปรับปรุง / แก้ไข")]
        public System.DateTime dateUpdate { get; set; }
    }
}