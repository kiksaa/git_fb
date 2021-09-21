using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Farmbook.Models
{
    public class EventReport
    {
        [DisplayName("ลำดับที่")]
        public int ID { get; set; }
        [DisplayName("หัวข้อ")]
        public string subject { get; set; }
        [DisplayName("รายละเอียด")]
        public string description { get; set; }
        [DisplayName("วันที่เริ่มต้น")]
        public System.DateTime start { get; set; }
        [DisplayName("วันที่สิ้นสุด")]
        public Nullable<System.DateTime> end { get; set; }
        [DisplayName("เต็มวัน")]
        public Nullable<bool> isFullDay { get; set; }
    }
}