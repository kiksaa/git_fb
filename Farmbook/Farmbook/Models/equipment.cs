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

    public partial class equipment
    {
        [DisplayName("�������ػ�ó�")]
        public int equipmentType { get; set; }
        [DisplayName("�����ػ�ó�")]
        public string equipmentID { get; set; }
        [DisplayName("�����ػ�ó�")]
        public string equipmentName { get; set; }
        [DisplayName("��������´")]
        public string detail { get; set; }
        [DisplayName("�Ҥҫ���")]
        public Nullable<int> price { get; set; }
        [DisplayName("˹��·�����")]
        public int unitBuy { get; set; }
        [DisplayName("˹��·����")]
        public int unitUse { get; set; }
        [DisplayName("�ѹ������")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dateBuy { get; set; }
        [DisplayName("���ҧҹ(��./���)")]
        public Nullable<int> workTime { get; set; }
        [DisplayName("��Ҿ�ѧ�ҹ���������ԧ/��.")]
        public Nullable<int> fuel { get; set; }
        [DisplayName("��������ѧ�ҹ")]
        public Nullable<int> energy { get; set; }
        [DisplayName("�ٻ�Ҿ�ػ�ó�")]
        public string equipmentImg { get; set; }
        [DisplayName("�ӴѺ���")]
        public int IDequip { get; set; }

        public virtual energy energy1 { get; set; }
        public virtual equipmenttype equipmenttype1 { get; set; }
        public virtual unit unit { get; set; }
        public virtual unit unit1 { get; set; }
    }
}
