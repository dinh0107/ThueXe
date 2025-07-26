using ThueXe.DAL;
using ThueXe.Models;
using ThueXe.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Drawing;

namespace ThueXe.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class ProductController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        private IEnumerable<ProductCategory> ProductCategories =>
            _unitOfWork.ProductCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));

        private SelectList ParentProductCategoryList => new SelectList(ProductCategories.Where(a => a.ParentId == null), "Id", "CategoryName");

        #region ProductCategory
        [ChildActionOnly]
        public ActionResult ListCategory()
        {
            var allcats = _unitOfWork.ProductCategoryRepository.Get(orderBy: q => q.OrderBy(a => a.CategorySort));
            return PartialView(allcats);
        }
        [Route("them-danh-muc-dich-vu")]
        public ActionResult ProductCategory(string result = "")
        {
            ViewBag.ProductCat = result;
            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");

            var model = new InsertProductCatViewModel
            {
                ProductCategory = new ProductCategory { CategorySort = 1 },
            };

            return View(model);
        }
        [Route("them-danh-muc-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductCategory(InsertProductCatViewModel model, FormCollection fc)
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
                    var imgPath = "/images/productCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = System.Drawing.Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ProductCategory.Image")
                    {
                        model.ProductCategory.Image = imgFile;
                    }
                }

                model.ProductCategory.Url = HtmlHelpers.ConvertToUnSign(null, model.ProductCategory.Url ?? model.ProductCategory.CategoryName);
                model.ProductCategory.ParentId = model.ParentId;

                _unitOfWork.ProductCategoryRepository.Insert(model.ProductCategory);
                _unitOfWork.Save();
                return RedirectToAction("ProductCategory", new { result = "success" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            return View(model);
        }
        [Route("sua-danh-muc-dich-vu")]
        public ActionResult UpdateCategory(int catId = 0)
        {
            var category = _unitOfWork.ProductCategoryRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("ProductCategory");
            }

            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            var model = new InsertProductCatViewModel
            {
                ProductCategory = category,
            };

            return View(model);
        }
        [Route("sua-danh-muc-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCategory(InsertProductCatViewModel model, FormCollection fc)
        {
            var category = _unitOfWork.ProductCategoryRepository.GetById(model.ProductCategory.Id);
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/productCategory/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = System.Drawing.Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "ProductCategory.Image")
                    {
                        category.Image = imgFile;
                    }
                }

                var file = Request.Files["ProductCategory.CoverImage"];
                var file2 = Request.Files["ProductCategory.Image"];

                if (file2 != null && file2.ContentLength == 0)
                {
                    category.Image = fc["CurrentFile2"] == "" ? null : fc["CurrentFile2"];
                }

                category.Url = HtmlHelpers.ConvertToUnSign(null, model.ProductCategory.Url ?? model.ProductCategory.CategoryName);
                category.CategoryName = model.ProductCategory.CategoryName;
                category.CategorySort = model.ProductCategory.CategorySort;
                category.Body = model.ProductCategory.Body;
                category.CategoryActive = model.ProductCategory.CategoryActive;
                category.ParentId = model.ParentId;
                category.ShowMenu = model.ProductCategory.ShowMenu;
                category.ShowFooter = model.ProductCategory.ShowFooter;
                category.TitleMeta = model.ProductCategory.TitleMeta;
                category.DescriptionMeta = model.ProductCategory.DescriptionMeta;

                _unitOfWork.Save();
                return RedirectToAction("ProductCategory", new { result = "update" });
            }
            ViewBag.RootCats = new SelectList(_unitOfWork.ProductCategoryRepository.Get(a => a.ParentId == null, q => q.OrderBy(a => a.CategorySort)), "Id", "CategoryName");
            return View(category);
        }
        [HttpPost]
        public bool DeleteCategory(int catId = 0)
        {
            var category = _unitOfWork.ProductCategoryRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.ProductCategoryRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateProductCat(int sort = 1, bool active = false, bool footer = false, bool menu = false, int productCatId = 0)
        {
            var productCat = _unitOfWork.ProductCategoryRepository.GetById(productCatId);
            if (productCat == null)
            {
                return false;
            }
            productCat.CategorySort = sort;
            productCat.CategoryActive = active;
            productCat.ShowMenu = menu;
            productCat.ShowFooter = footer;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Product
        [Route("danh-sach-dich-vu")]
        public ActionResult ListProduct(int? page, string name, int? childId, int rootId = 0, string sort = "date-desc", string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var products = _unitOfWork.ProductRepository.GetQuery().AsNoTracking();

            if (rootId > 0)
            {
                products = products.Where(a => a.ProductCategoryId == rootId);
            }
            else if (childId > 0)
            {
                products = products.Where(a => a.ProductCategoryId == childId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(l => l.Name.Contains(name));
            }

            switch (sort)
            {
                case "sort-asc":
                    products = products.OrderBy(a => a.Sort);
                    break;
                case "sort-desc":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                case "hot":
                    products = products.OrderByDescending(a => a.Sort);
                    break;
                case "date-asc":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                default:
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
            }
            var model = new ListProductViewModel
            {
                SelectCategories = new SelectList(ProductCategories.Where(a => a.ParentId == null), "Id", "CategoryName"),
                Products = products.ToPagedList(pageNumber, pageSize),
                ChildId = childId,
                RootId = rootId,
                Name = name,
                Sort = sort
            };
            if (childId.HasValue)
            {
                model.ChildCategoryList = new SelectList(ProductCategories.Where(a => a.ParentId == childId), "Id", "Categoryname");
            }
            return View(model);
        }
        [Route("them-dich-vu")]
        public ActionResult Product()
        {
            var model = new InsertProductViewModel
            {
                Product = new Product { Sort = 1, Active = true },
                Categories = ProductCategories
            };
            return View(model);
        }
        [Route("them-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Product(InsertProductViewModel model, FormCollection fc)
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
                    var imgPath = "/images/products/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Product.Image")
                    {
                        model.Product.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Product.CoverImage")
                    {
                        model.Product.CoverImage = imgFile;
                    }
                }

                model.Product.Feedback = fc["Pictures"];
                model.Product.ProductCategoryId = model.CategoryId;
                model.Product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);

                var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == model.Product.Url).Count();
                if (count > 1)
                {
                    model.Product.Url += "-" + DateTime.Now.Millisecond;
                    _unitOfWork.Save();
                }

                _unitOfWork.ProductRepository.Insert(model.Product);
                _unitOfWork.Save();

                return RedirectToAction("ListProduct", new { result = "success" });
            }

            model.Categories = ProductCategories;
            return View(model);
        }
        [Route("sua-dich-vu")]
        public ActionResult UpdateProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }

            var category = _unitOfWork.ProductCategoryRepository.GetById(product.ProductCategoryId);

            var model = new InsertProductViewModel
            {
                Product = product,
                Categories = ProductCategories,
                //Price = product.Price?.ToString("N0"),
                //PriceSale = product.PriceSale?.ToString("N0"),
            };

            return View(model);
        }
        [Route("sua-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProduct(InsertProductViewModel model, FormCollection fc)
        {
            var product = _unitOfWork.ProductRepository.GetById(model.Product.Id);
            if (product == null)
            {
                return RedirectToAction("ListProduct");
            }

            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/products/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Product.Image")
                    {
                        product.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Product.CoverImage")
                    {
                        product.CoverImage = imgFile;
                    }
                }

                var file = Request.Files["Product.CoverImage"];

                if (file != null && file.ContentLength == 0)
                {
                    product.CoverImage = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                }

                product.Feedback = fc["Pictures"] == "" ? null : fc["Pictures"];
                product.Url = HtmlHelpers.ConvertToUnSign(null, model.Product.Url ?? model.Product.Name);
                product.ProductCategoryId = model.CategoryId;
                product.Name = model.Product.Name;
                product.Body = model.Product.Body;
                product.Active = model.Product.Active;
                product.Home = model.Product.Home;
                product.TitleMeta = model.Product.TitleMeta;
                product.DescriptionMeta = model.Product.DescriptionMeta;
                product.Sort = model.Product.Sort;

                _unitOfWork.Save();

                var count = _unitOfWork.ProductRepository.GetQuery(a => a.Url == product.Url).Count();
                if (count > 1)
                {
                    product.Url += "-" + DateTime.Now.Millisecond;
                    _unitOfWork.Save();
                }

                return RedirectToAction("ListProduct", new { result = "update" });
            }

            model.Categories = ProductCategories;

            return View(model);
        }
        [HttpPost]
        public bool DeleteProduct(int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool QuickUpdate(int? quantity, bool? status, bool active, int sort = 0, int proId = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null)
            {
                return false;
            }
            if (status != null)
            {
                product.Active = Convert.ToBoolean(status);
            }
            if (sort >= 0)
            {
                product.Sort = sort;
            }
            product.Active = active;
            _unitOfWork.Save();
            return true;
        }
        #endregion

        public JsonResult GetChildCategory(int? parentId)
        {
            var categories = ProductCategories.Where(a => a.ParentId == parentId);
            return Json(categories.Select(a => new { a.Id, Name = a.CategoryName }), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}