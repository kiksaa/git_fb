using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ViewLandPlot
    {
        public int ID { get; set; }

        [DisplayName("ชื่อแปลง")]
        public string plotName { get; set; }
        [DisplayName("ชื่อเกษตรกร")]
        public string name { get; set; }
        [DisplayName("เนื้อที่(ไร่)")]
        public Nullable<float> areaPlot { get; set; }
        [DisplayName("รหัสแปลง")]
        public string areaCode { get; set; }
        [DisplayName("การถือครองที่ดิน")]
        public string ownership { get; set; }
        [DisplayName("ประเภทเอกสารสิทธิ์")]
        public string licenseName { get; set; }
        [DisplayName("ตำบล/แขวง")]
        public string districtName { get; set; }
        [DisplayName("อำเภอ/เขต")]
        public string ampherName { get; set; }
        [DisplayName("จังหวัด")]
        public string provinceName { get; set; }
        [DisplayName("สถานะแปลง")]
        public string plotStatus { get; set; }
        [DisplayName("โครงการ")]
        public string projectName { get; set; }
        [DisplayName("จำนวนแปลง")]
        public Nullable<float> areaNumber { get; set; }
        [DisplayName("ที่อยู่")]
        public string addressplot
        {
            get
            {
                return " ตำบล/แขวง " + districtName + " อำเภอ/เขต  " + ampherName + " จังหวัด " + provinceName;
            }
        }
        [DisplayName("ผู้รับผิดชอบ")]
        public string administrator { get; set; }
        [DisplayName("เลขที่เอกสารสิทธิ์ (เลขที่โฉนดที่ดิน)")]
        public string titleDeed { get; set; }
        [DisplayName("เลขที่ดิน")]
        public Nullable<int> landNumber { get; set; }
        [DisplayName("เอกสาร (สัญญาเช่า)")]
        public string lease_img { get; set; }
        [DisplayName("รายละเอียดแปลง")]
        public string plotDetails { get; set; }
        [DisplayName("วิธีการรับซื้อ")]
        public string nameBuy { get; set; }
        [DisplayName("เอกสาร (เอกสารสิทธิ์)")]
        public string license_img { get; set; }
        [DisplayName("ผลผลิต (กก.)")]
        public float product { get; set; }
        [DisplayName("ขั้นตอนการทำงาน")]
        public string workName { get; set; }
    }
}