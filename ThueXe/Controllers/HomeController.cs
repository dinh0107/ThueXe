using System;
using System.Collections.Generic;
using System.Linq;
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

        private IEnumerable<ArticleCategory> ArticleCategories =>
            _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        private IEnumerable<ProductCategory> ProductCategories =>
            _unitOfWork.ProductCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));



        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ProductCategories = ProductCategories.Where(a => a.ShowMenu),
                ArticleCategories = ArticleCategories.Where(a => a.ShowMenu),
                Banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active && a.GroupId == 1 && a.Image != null).FirstOrDefault()
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.ArticleCategory.TypePost == TypePost.Policy, o => o.OrderByDescending(a => a.CreateDate)),
                ProductCategories = ProductCategories.Where(a => a.ShowFooter),
                ArticleCategories = ArticleCategories.Where(a => a.ShowFooter)
            };
            return PartialView(model);
        }
        public ActionResult Index()
        {
            var banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active  , o => o.OrderBy(a => a.Sort ));
            var service = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && a.TypePost == TypePost.Service, o => o.OrderBy(a => a.CategorySort));
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategory.TypePost == TypePost.Article && a.Home), o => o.OrderByDescending(a => a.CreateDate));
            var model = new HomeViewModel
            {
                Banners = banner,
                Services = service,
                Articles = articles,
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}