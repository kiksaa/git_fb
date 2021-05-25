using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Farmbook.Models
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
    }
}