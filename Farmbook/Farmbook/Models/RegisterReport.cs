using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Farmbook.Models
{
    public class RegisterReport
    {
        [DisplayName("ลำดับที่")]
        public int ID { get; set; }
        [DisplayName("ชื่อ นามสกุล")]
        public string name { get; set; }
        [DisplayName("ทะเบียนเกษตรกร")]
        public string registerID { get; set; }
        [DisplayName("เลขบัตรประชาชน")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "กรุณาตรวจหมายเลขบัตรประชาชนอีกครั้ง")]
        public string cardID { get; set; }
        [DisplayName("เพศ")]
        public string gender { get; set; }
        [DisplayName("วันเกิด")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> birthday { get; set; }
        [DisplayName("เบอร์โทรศัพท์")]
        public int tel { get; set; }
        [DisplayName("อีเมล์")]
        [Required(ErrorMessage = "อีเมล์ของคุณควรประกอบไปด้วย")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string email { get; set; }
        [DisplayName("สถานะครอบครัว")]
        public string family { get; set; }
        [DisplayName("บ้านเลขที่")]
        public Nullable<int> no { get; set; }
        [DisplayName("หมู่ที่")]
        public Nullable<int> moo { get; set; }
        [DisplayName("ถนน/ซอย")]
        public string road { get; set; }
        [DisplayName("จังหวัด")]
        public string province { get; set; }
        [DisplayName("อำเภอ/เขต")]
        public string ampher { get; set; }
        [DisplayName("ตำบล/แขวง")]
        public string district { get; set; }
        [DisplayName("ความคิดเห็น")]
        public string comment { get; set; }
        [DisplayName("รูปภาพเกษตรกร")]
        public string farmer_img { get; set; }
        [DisplayName("รูปถ่ายบัตรประชาชน")]
        public string card_img { get; set; }
        [DisplayName("จำนวนแปลง")]
        public Nullable<float> areaNumber { get; set; }
        [DisplayName("ธนาคาร")]
        public string bank { get; set; }
        /*[DisplayName("สถานะ")]
        public string status { get; set; }*/
        [DisplayName("วันที่ปรับปรุง/แก้ไข")]
        public System.DateTime dateUpdate { get; set; }
        [DisplayName("แก้ไขโดย")]
        public string adminBy { get; set; }
        public string active { get; set; }
       
    }
}