using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Farmbook.Models
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
                return no + " หมู่ที่ " + moo + " ถนน/ซอย " + road + " ตำบล/แขวง " + districtName + " อำเภอ/เขต " + ampherName + " จังหวัด " + provinceName ;
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


        [DisplayName("ลำดับที่")]
        public Nullable<int> IDve { get; set; }
        
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


        [DisplayName("ลำดับที่")]
        public Nullable<int> IDequip { get; set; }
        [DisplayName("ประเภทอุปกรณ์")]
        public string equipmentType { get; set; }
        [DisplayName("ชื่ออุปกรณ์")]
        public string equipmentName { get; set; }
        [DisplayName("รหัสอุปกรณ์")]
        public string equipmentID { get; set; }
        [DisplayName("รายละเอียด")]
        public string detailV { get; set; }
        [DisplayName("ราคาทุน")]
        public Nullable<int> priceV { get; set; }

        [DisplayName("ลำดับที่")]
        public Nullable<int> IDmac { get; set; }
        [DisplayName("ประเภทเครื่องจักร")]
        public string machineType { get; set; }
        [DisplayName("ชื่อเครื่องจักร")]
        public string machineName { get; set; }
        [DisplayName("รหัสเครื่องจักร")]
        public string machineID { get; set; }
        [DisplayName("รายละเอียด")]
        public string detailM { get; set; }
        [DisplayName("ราคาทุน")]
        public Nullable<int> priceM { get; set; }
        [DisplayName("ลำดับที่")]
        public Nullable<int> IDsoft { get; set; }
        [DisplayName("ประเภทซอฟต์แวร์")]
        public string softwareType { get; set; }
        [DisplayName("ชื่อซอฟต์แวร์")]
        public string softwareName { get; set; }
        [DisplayName("รหัสซอฟต์แวร์")]
        public string softwareID { get; set; }
        [DisplayName("รายละเอียด")]
        public string detailS { get; set; }
        [DisplayName("ราคาทุน")]
        public Nullable<int> priceS { get; set; }
        [DisplayName("ลำดับที่")]
        public Nullable<int> IDstap { get; set; }
        [DisplayName("ประเภทวัตถุดิบ")]
        public string stapleType { get; set; }
        [DisplayName("ชื่อวัตถุดิบ")]
        public string stapleName { get; set; }
        [DisplayName("รหัสวัตถุดิบ")]
        public string stapleID { get; set; }
        [DisplayName("รายละเอียด")]
        public string detailSt { get; set; }
        [DisplayName("ราคาทุน")]
        public Nullable<int> priceSt { get; set; }

        [DisplayName("ลำดับที่")]
        public Nullable<int> IDlab { get; set; }
        [DisplayName("ประเภทพนักงาน")]
        public string laborType { get; set; }
        [DisplayName("ตำแหน่งงาน")]
        public string positionName { get; set; }
        [DisplayName("รหัสพนักงาน")]
        public string laborID { get; set; }
        [DisplayName("ชื่อพนักงาน")]
        public string laborName { get; set; }
        [DisplayName("ค่าแรง")]
        public Nullable<int> salary { get; set; }
        [DisplayName("ลำดับที่")]
        public Nullable<int> IDfule { get; set; }
        [DisplayName("ประเภทพลังงานและเชื้อเพลิง")]
        public string fuelName { get; set; }
        [DisplayName("ชื่อพลังงานและเชื้อเพลิง")]
        public string fuelType { get; set; }
        [DisplayName("รายละเอียด")]
        public string detailF { get; set; }
        [DisplayName("ราคาทุน")]
        public Nullable<int> priceF { get; set; }
        [DisplayName("วิธีการรับซื้อ")]
        public string nameBuy { get; set; }
        [DisplayName("มาตรฐานการผลิต")]
        public string standardName { get; set; }
        [DisplayName("รายการ")]
        public string list { get; set; }
        [DisplayName("กรอกข้อมูล")]
        public bool fillin { get; set; }
        [DisplayName("รูปภาพ")]
        public bool img { get; set; }
        [DisplayName("ลำดับที่")]
        public int IDpro { get; set; }
        [DisplayName("รายการที่")]
        public Nullable<int> IDlist { get; set; }

        [DisplayName("ผลผลิตที่ได้")]
        public string product { get; set; }

        [DisplayName("ชื่อขั้นตอนการทำงาน")]
        public string workName { get; set; }
        [DisplayName("การเข้าถึง")]
        public string accessName { get; set; }
        [DisplayName("แหล่งอ้างอิง")]
        public string reference { get; set; }
        [DisplayName("ขั้นตอนการทำงาน")]
        public string theoryName { get; set; } 
        [DisplayName("เพศ")]
        public string genderName { get; set; } 
        [DisplayName("ประเภทเกษตรกร")]
        public string typeName { get; set; }

        [DisplayName("แก้ไขโดย")]
        public string adminBy { get; set; }
        [DisplayName("ขั้นตอนที่")]
        public Nullable<int> stepNum { get; set; }
        [DisplayName("ชื่อตอนที่")]
        public string stepName { get; set; }
        [DisplayName("อายุ(วัน)")]
        public Nullable<int> age { get; set; }
        [DisplayName("ระยะเวลา(วัน)")]
        public Nullable<int> time { get; set; }
        [DisplayName("กิจกรรม")]
        public string activity1 { get; set; }
        [DisplayName("ข้อสังเกต")]
        public string notice { get; set; }

        [DisplayName("หัวข้อ")]
        public string subject { get; set; }
        [DisplayName("รายละเอียด")]
        public string description { get; set; }
        [DisplayName("วันที่เริ่มต้น")]
        public System.DateTime start { get; set; }
        [DisplayName("วันที่สิ้นสุด")]
        public Nullable<System.DateTime> end { get; set; }
        [DisplayName("สีข้อความ")]
        public string themeColor { get; set; }
        [DisplayName("เต็มวัน")]
        public Nullable<bool> isFullDay { get; set; }
    }
}
