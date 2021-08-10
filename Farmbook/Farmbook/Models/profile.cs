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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class profile
    {
        public int ID { get; set; }
        [DisplayName("���� ���ʡ��")]
        public string name { get; set; }
        [DisplayName("�����Ţ�ѵû�ЪҪ�")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "��سҵ�Ǩ�����Ţ�ѵû�ЪҪ��ա����")]
        public string cradID { get; set; }
        [DisplayName("��")]
        public int gender { get; set; }
        [DisplayName("�ѹ/��͹/�� �Դ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> birthday { get; set; }
        [DisplayName("�������Ѿ��")]
        public string tel { get; set; }
        [DisplayName("��ҹ�Ţ���")]
        public Nullable<int> no { get; set; }
        [DisplayName("������")]
        public Nullable<int> moo { get; set; }
        [DisplayName("�ѧ��Ѵ")]
        public Nullable<int> province { get; set; }
        [DisplayName("�����/ࢵ")]
        public Nullable<int> ampher { get; set; }
        [DisplayName("�Ӻ�/�ǧ")]
        public Nullable<int> district { get; set; }
        [DisplayName("������")]
        [Required(ErrorMessage = "������ͧ�س��û�Сͺ仴���")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string email { get; set; }
        [DisplayName("���ʼ�ҹ")]
        [Required(ErrorMessage = "���ʼ�ҹ�ͧ�س��û�Сͺ仴���")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string password { get; set; }
        [DisplayName("�������ɵá�")]
        public int registerType { get; set; }
        [DisplayName("�ѧ��Ѵ")]
        public string provinceStr { get; set; }
        [DisplayName("�����/ࢵ")]
        public string ampherStr { get; set; }
        [DisplayName("�Ӻ�/�ǧ")]
        public string districtStr { get; set; }

        public virtual ampher ampher1 { get; set; }
        public virtual district district1 { get; set; }
        public virtual gender gender1 { get; set; }
        public virtual registertype registertype1 { get; set; }
        public virtual province province1 { get; set; }
        public IEnumerable<SelectListItem> ProvinceList { get; set; }
        public IEnumerable<SelectListItem> AmpherList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
    }
}