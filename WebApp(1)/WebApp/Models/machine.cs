//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class machine
    {
        [DisplayName("����������ͧ�ѡ�")]
        public int machineType { get; set; }
        [DisplayName("��������ͧ�ѡ�")]
        public string machineID { get; set; }
        [DisplayName("��������ͧ�ѡ�")]
        public string machineName { get; set; }
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
        [DisplayName("�ٻ�Ҿ����ͧ�ѡ�")]
        public string machineImg { get; set; }
        [DisplayName("�ӴѺ���")]
        public int IDmac { get; set; }

        public virtual energy energy1 { get; set; }
        public virtual machinetype machinetype1 { get; set; }
        public virtual unit unit { get; set; }
        public virtual unit unit1 { get; set; }

        [DataType(DataType.Upload)]
        [DisplayName("Select File")]
        public HttpPostedFileBase file_machineImg { get; set; }
    }
}
