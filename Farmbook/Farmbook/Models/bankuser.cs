//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Farmbook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class bankuser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public bankuser()
        {
            this.registers = new HashSet<register>();
        }
        [DisplayName("�ӴѺ���")]
        public int ID { get; set; }
        [DisplayName("��Ҥ��")]
        public int bankID { get; set; }
        [DisplayName("���ͺѭ��")]
        public string bankName { get; set; }
        [DisplayName("�Ţ���ѭ��")]
        public string bankNo { get; set; }

        public virtual bank bank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<register> registers { get; set; }
    }
}
