using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ThueXe.DAL;
using ThueXe.Models;
using ThueXe.ViewModel;

namespace ThueXe.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        private IEnumerable<ArticleCategory> ArticleCategories() =>
            _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        private IEnumerable<ProductCategory> ProductCategories =>
            _unitOfWork.ProductCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));



        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ProductCategories = ProductCategories.Where(a => a.ShowMenu),
                ArticleCategories = ArticleCategories().Where(a => a.ShowMenu),
                Banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active && a.GroupId == 1 && a.Image != null).FirstOrDefault()
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                ArticleCategories = ArticleCategories().Where(a => a.ShowFooter)
            };
            return PartialView(model);
        }
        public ActionResult Index()
        {
            var banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active  , o => o.OrderBy(a => a.Sort ));
            var service = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && (a.TypePost == TypePost.Service && a.Home), o => o.OrderBy(a => a.CategorySort));
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategory.TypePost == TypePost.Article && a.Home), o => o.OrderByDescending(a => a.CreateDate));
            var model = new HomeViewModel
            {
                Banners = banner,
                Services = service,
                Articles = articles.Take(6),
            };
            return View(model);
        }
        [Route("{url}")]
        public ActionResult ServiceCar(string url)
        {
            var carService = _unitOfWork.CarServiceRepository.GetQuery(a => a.Slug ==  url).FirstOrDefault();
            if(carService == null)
            {
                return RedirectToAction("Index");
            }
            var banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort));
            var service = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && (a.TypePost == TypePost.Service && a.Home), o => o.OrderBy(a => a.CategorySort));
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategory.TypePost == TypePost.Article && a.Home), o => o.OrderByDescending(a => a.CreateDate));
            var model = new ServiceCarViewModel
            {
                Banners = banner,
                Services = service,
                Articles = articles.Take(6),
                CarService = carService
            };
            return View(model);
        }
        [Route("news/{url}", Order = 0)]
        public ActionResult ArticleCategory(string url, int? page)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && a.Url == url).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var articles = _unitOfWork.ArticleRepository.GetQuery(
                a => a.Active && (a.ArticleCategoryId == category.Id || a.ArticleCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;

            if (articles.Count() == 1)
            {
                var fi = articles.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new ArticleCategoryViewModel
            {
                Category = category,
                Articles = articles.ToPagedList(pageNumber, 11),
                Categories = ArticleCategories(),
            };

            if (category.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ArticleCategoryRepository.GetById(category.ParentId);
            }
            return View(model);
        }
        [Route("{url}.html")]
        public ActionResult ArticleDetail(string url, string view = "")
        {
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Url == url).FirstOrDefault();
            if (article == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            if (view == "")
            {
                article.View++;
                _unitOfWork.ArticleRepository.Update(article);
                _unitOfWork.Save();
            }

            var model = new ArticleViewModel
            {
                Article = article,
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategoryId == article.ArticleCategoryId && a.Id != article.Id)).OrderByDescending(a => a.CreateDate).Take(6)
            };
            if (article.ArticleCategory.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ArticleCategoryRepository.GetById(article.ArticleCategory.ParentId);
            }
            return View(model);
        }
        public PartialViewResult ArticleHot()
        {
            var articles = _unitOfWork.ArticleRepository
                .GetQuery(a => a.Active || a.View >= 100,
                          o => o.OrderByDescending(a => a.CreateDate))
                .ToList();

            var model = new NavArticleViewModel
            {
                Articles = articles
            };

            return PartialView(model);

        }
        public PartialViewResult Form()
        {
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ContactForm(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }

            if (model.ToDate != null)
            {
                model.ToDate = Convert.ToDateTime(model.ToDate);
            }
          
            _unitOfWork.ContactRepository.Insert(model);
            _unitOfWork.Save();

            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Điểm đi: {model.From},</p>" +
                       $"<p>Điểm đến: {model.To},</p>" +
                       $"<p>Ngày đi: {model.FromDate.ToString("dd/MM/yyyy HH:mm")},</p>" +
                       $"<p>Ngày về: {model.ToDate?.ToString("dd/MM/yyyy HH:mm")},</p>" +
                       $"<p>Loại xe: {model.TypeCar},</p>" +
                       $"<p>Số điện thoại: {model.Mobile},</p>" +
                       $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, Email, Email, Password, "Thuê xe Nam Anh"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc với bạn sớm nhất có thể." });
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}