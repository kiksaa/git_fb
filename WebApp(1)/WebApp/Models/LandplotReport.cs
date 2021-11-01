using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class LandplotReport
    {
        [DisplayName("ลำดับที่")]
        public int ID { get; set; }
        [DisplayName("ชื่อเกษตรกร")]
        public string name { get; set; }
        [DisplayName("ชื่อแปลง")]
        public string plotName { get; set; }
        [DisplayName("รหัสแปลง")]
        public string areaCode { get; set; }
        [DisplayName("การถือครองที่ดิน (ประเภทกรรมสิทธิ์)")]
        public string ownership { get; set; }
        [DisplayName("ประเภทเอกสารสิทธิ์")]
        public string licenseName { get; set; }
        [DisplayName("เลขที่เอกสารสิทธิ์ (เลขที่โฉนดที่ดิน)")]
        public string titleDeed { get; set; }
        [DisplayName("ระวาง")]
        public string landSlip { get; set; }
        [DisplayName("เลขที่ดิน")]
        public Nullable<int> landNumber { get; set; }
        [DisplayName("เอกสาร (เอกสารสิทธิ์)")]
        public string license_img { get; set; }
        [DisplayName("เอกสาร (สัญญาเช่า)")]
        public string lease_img { get; set; }
        [DisplayName("ตำบล/แขวง")]
        public string districtName { get; set; }
        [DisplayName("อำเภอ/เขต")]
        public string ampherName { get; set; }
        [DisplayName("จังหวัด")]
        public string provinceName { get; set; }
        [DisplayName("พิกัดเริ่มต้น (ละติจูด)")]
        public string coordinatesStar { get; set; }
        [DisplayName("พิกัดสุดท้าย (ลองจิจูด)")]
        public string coordinatesEnd { get; set; }
        [DisplayName("เนื้อที่")]
        public Nullable<float> areaPlot { get; set; }
        [DisplayName("เนื้อที่หน่วยเป็นไร่")]
        public string areaPlotS { get; set; }
        [DisplayName("ผลผลิต (กก.)")]
        public float product { get; set; }
        [DisplayName("รายละเอียดแปลง")]
        public string plotDetails { get; set; }

        [DisplayName("โครงการ")]
        public string projectName { get; set; }
        [DisplayName("ขั้นตอนการทำงาน")]
        public string workName { get; set; }
        [DisplayName("หมายเหตุ")]
        public string note { get; set; }
        [DisplayName("วิธีการรับซื้อ")]
        public string nameBuy { get; set; }
        [DisplayName("สถานะแปลง")]
        public string plotStatus { get; set; }
        [DisplayName("ผู้รับผิดชอบ")]
        public string administrator { get; set; }
        public string activeName { get; set; }
    }
}