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

    public partial class activity
    {
        [DisplayName("�ӴѺ���")]
        public int ID { get; set; }
        [Required(ErrorMessage = "��سҡ�͡������")]
        [DisplayName("��鹵͹���")]
        public Nullable<int> stepNum { get; set; }
        [Required(ErrorMessage = "��سҡ�͡������")]
        [DisplayName("���͢�鹵͹")]
        public string stepName { get; set; }
        [DisplayName("����(�ѹ)")]
        public int age { get; set; }
        [DisplayName("��������(�ѹ)")]
        public int time { get; set; }
        [DisplayName("�Ԩ�����")]
        public string activity1 { get; set; }
        [DisplayName("����ѧࡵ")]
        public string notice { get; set; }
        [Required(ErrorMessage = "��سҡ�͡������")]
        [DisplayName("���͢�鹵͹��÷ӧҹ")]
        public int plan { get; set; }

        public virtual theory theory { get; set; }
    }
}
