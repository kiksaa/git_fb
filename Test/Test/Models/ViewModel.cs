using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class ViewModel
    {
        [DisplayName("ลำดับที่")]
        public int ID { get; set; }
        [DisplayName("ชื่อเกษตรกร")]
        public string name { get; set; }
        [DisplayName("เลขทะเบียนเกษตรกร")]
        public string registerID { get; set; }
        [DisplayName("เลขบัตรประชาชน")]
        public string cardID { get; set; }
        [DisplayName("บ้านเลขที่")]
        public Nullable<int> no { get; set; }
        [DisplayName("หมู่ที่")]
        public Nullable<int> moo { get; set; }
        [DisplayName("ถนน/ซอย")]
        public string road { get; set; }
        [DisplayName("จังหวัด")]
        public string provinceName { get; set; }
        [DisplayName("อำเภอ/แขวง")]
        public string ampherName { get; set; }
        [DisplayName("ตำบล/เขต")]
        public string districtName { get; set; }
        [DisplayName("สถานะ")]
        public string statusName { get; set; }
        [DisplayName("วันที่ปรับปรุง")]
        public System.DateTime dateUpdate { get; set; }

        [DisplayName("ที่อยู่")]
        public string address
        {
            get
            {
                return no + " หมู่ที่ " + moo + " ถนน/ซอย " + road + " " + districtName + " " + ampherName + " " + provinceName ;
            }
        }
        public int bank { get; set; }
        [DisplayName("ธนาคาร")]
        public int bankType { get; set; }
        [DisplayName("ชื่อบัญชี")]
        public string bankName { get; set; }
        [DisplayName("หมายเลขบัญชี")]
        public string bankNo { get; set; }
        [DisplayName("โครงการ")]
        public string projectName { get; set; }
        [DisplayName("ชื่อแปลง")]
        public string plotName { get; set; }
        [DisplayName("เนื้อที่(ไร่)")]
        public Nullable<float> areaPlot { get; set; }
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

        [DisplayName("อีเมล์")]
        public string email { get; set; }
        [DisplayName("ที่อยู่")]
        public string addresspro
        {
            get
            {
                return no + " หมู่ที่ " + moo;
            }
        }
        [DisplayName("เพศ")]
        public int gender { get; set; }

        [DisplayName("วัน/เดือน/ปี เกิด")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime birthday { get; set; }

        [DisplayName("เบอร์โทรศัพท์")]
        public string tel { get; set; }
        [DisplayName("รหัสผ่าน")]
        public string password { get; set; }


        [DisplayName("ประเภทพาหนะ")]
        public string vehicleType { get; set; }
        [DisplayName("ชื่อพาหนะ")]
        public string vehicleName { get; set; }
        [DisplayName("รหัสพาหนะ")]
        public string vehicleID { get; set; }
        [DisplayName("รายละเอียด")]
        public string detail { get; set; }
        [DisplayName("ราคาทุน")]
        public Nullable<int> price { get; set; }
        [DisplayName("หย่วยที่...")]
        public string unitName { get; set; }
        [DisplayName("ประเภทพลังงาน")]
        public string energyName { get; set; }

    }
}