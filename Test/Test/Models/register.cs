//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class register
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public register()
        {
            this.landplots = new HashSet<landplot>();
        }
        [DisplayName("�ӴѺ���")]
        public int ID { get; set; }
        [DisplayName("���� ���ʡ��")]
        public string name { get; set; }
        [DisplayName("����¹�ɵá�")]
        public string registerID { get; set; }
        [DisplayName("�Ţ�ѵû�ЪҪ�")]
        public string cardID { get; set; }
        [DisplayName("��")]
        public Nullable<int> gender { get; set; }
        [DisplayName("�ѹ/��͹/�� �Դ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> birthday { get; set; }
        [DisplayName("�������Ѿ��")]
        public int tel { get; set; }
        [DisplayName("������")]
        public string email { get; set; }
        [DisplayName("ʶҹФ�ͺ����")]
        public Nullable<int> family { get; set; }
        [DisplayName("��ҹ�Ţ���")]
        public Nullable<int> no { get; set; }
        [DisplayName("������")]
        public Nullable<int> moo { get; set; }
        [DisplayName("���/���")]
        public string road { get; set; }
        [DisplayName("�ѧ��Ѵ")]
        public Nullable<int> province { get; set; }
        [DisplayName("�����/ࢵ")]
        public Nullable<int> ampher { get; set; }
        [DisplayName("�Ӻ�/�ǧ")]
        public Nullable<int> district { get; set; }
        [DisplayName("�����Դ���")]
        public string comment { get; set; }
        [DisplayName("�ٻ�Ҿ�ɵá�")]
        public string farmer_img { get; set; }
        [DisplayName("�ٻ���ºѵû�ЪҪ�")]
        public string card_img { get; set; }
        [DisplayName("��Ҥ��")]
        public Nullable<int> bank { get; set; }
        [DisplayName("ʶҹ�")]
        public Nullable<int> status { get; set; }
        [DisplayName("�ѹ����Ѻ��ا/���")]
        public System.DateTime dateUpdate { get; set; }

        public virtual ampher ampher1 { get; set; }
        public virtual bankuser bankuser { get; set; }
        public virtual district district1 { get; set; }
        public virtual family family1 { get; set; }
        public virtual gender gender1 { get; set; }
        public virtual province province1 { get; set; }
        public virtual status status1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<landplot> landplots { get; set; }
    }
}
