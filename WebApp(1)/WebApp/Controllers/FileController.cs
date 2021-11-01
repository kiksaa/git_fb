using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class FileController : Controller
    {
        #region View Uploaded files  
        public ActionResult FileUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FileUpload(filedetail filesModel)
        {
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                String FileExt = Path.GetExtension(filesModel.files.FileName).ToUpper();

                if (FileExt == ".PDF")
                {
                    Byte[] data = new byte[filesModel.files.ContentLength];
                    filesModel.files.InputStream.Read(data, 0, filesModel.files.ContentLength);
                    /*filesModel.fileID = 1;*/
                    filesModel.fileName = filesModel.files.FileName;
                    /*filesModel.fileContentType = filesModel.files.ContentType;*/
                    filesModel.fileData = data;
                    SaveFileDetails(filesModel);
                    return RedirectToAction("FileDetails");
                }
                else
                {
                    ViewBag.FileStatus = "Invalid file format.";
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult FileDetails()
        {
            List<filedetail> filesModel = new List<filedetail>();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                /*profile profileModel = new profile();
                profileModel = farmdb.profiles.Where(e => e.email == User.Identity.Name).FirstOrDefault();
                ViewBag.status = profileModel.registerType.ToString();*/

                filesModel = farmdb.filedetails.ToList<filedetail>();
                ViewBag.TotalFile = filesModel.Count();
                List<ViewModel> ViewModeltList = new List<ViewModel>();
                var data = from f in farmdb.filedetails
                           select new
                           {
                               f.fileID,
                               f.fileName,
                               f.fileData,
                               f.fileContentType
                           };
                foreach (var item in data)
                {
                    ViewModel objcvm = new ViewModel();
                    objcvm.fileID = item.fileID;
                    objcvm.fileName = item.fileName;
                    objcvm.fileData = item.fileData;
                    objcvm.fileContentType = item.fileContentType;
                    ViewModeltList.Add(objcvm);
                }
                return View(ViewModeltList);
            }
        }
        #endregion

        #region Upload Download file  
        public FileResult DownloadDocument(int id)
        {

            farmdbEntities farmdb = new farmdbEntities();
            var myFile = farmdb.filedetails.SingleOrDefault(x => x.fileID == id);

            if (myFile != null)
            {
                byte[] fileBytes = myFile.fileData;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, myFile.fileName);
            }
            return null;
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            filedetail filesModel = new filedetail();
            using (farmdbEntities farmdb = new farmdbEntities())
            {
                filesModel = farmdb.filedetails.Where(x => x.fileID == id).FirstOrDefault();
            }
            return File(filesModel.fileData, "application/pdf", filesModel.fileName);
        }
        #endregion

        #region Database related operations  
        private void SaveFileDetails(filedetail filesModel)
        {
            farmdbEntities farmdb = new farmdbEntities();
            farmdb.filedetails.Add(filesModel);
            farmdb.SaveChanges();
        }
        #endregion
    }
}