using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class FileModel
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public HttpPostedFileBase Files { get; set; }
    }
}