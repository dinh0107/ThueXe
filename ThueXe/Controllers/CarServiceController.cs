using Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXe.DAL;
using ThueXe.Models;

namespace ThueXe.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class CarServiceController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region CarService
        [ChildActionOnly]
        public ActionResult ListCarService()
        {
            var allcats = _unitOfWork.CarServiceRepository.Get(orderBy: q => q.OrderBy(a => a.Id));
            return PartialView(allcats);
        }
        [Route("them-trang-dich-vu")]
        public ActionResult CarService(string result = "")
        {
            ViewBag.ArticleCat = result;
            return View();
        }
        [Route("them-trang-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult CarService(CarService model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/carService/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ImageUrl")
                    {
                        model.ImageUrl = imgFile;
                    }
                }
                model.Slug = HtmlHelpers.ConvertToUnSign(null, model.Slug ?? model.Title);

                _unitOfWork.CarServiceRepository.Insert(model);
                _unitOfWork.Save();
                return RedirectToAction("CarService", new { result = "success" });
            }
            return View(model);
        }
        [Route("sua-trang-dich-vu")]
        public ActionResult UpdateCarService(int catId = 0)
        {
            var category = _unitOfWork.CarServiceRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("CarService");
            }
            return View(category);
        }
        [Route("sua-trang-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCarService(CarService category, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/carService/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ImageUrl")
                    {
                        category.ImageUrl = imgFile;
                    }
                }

                var file = Request.Files["Image"];

                if (file != null && file.ContentLength == 0)
                {
                    category.ImageUrl = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                }
                category.Slug = HtmlHelpers.ConvertToUnSign(null, category.Slug ?? category.Title);
                _unitOfWork.CarServiceRepository.Update(category);
                _unitOfWork.Save();
                return RedirectToAction("CarService", new { result = "update" });
            }
            return View(category);
        }
        [HttpPost]
        public bool DeleteCarService(int catId = 0)
        {

            var category = _unitOfWork.CarServiceRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.CarServiceRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Detail
        #endregion
    }
}