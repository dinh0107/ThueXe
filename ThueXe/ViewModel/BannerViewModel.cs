using ThueXe.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ThueXe.ViewModel
{
    public class BannerViewModel
    {
        public Banner Banner { get; set; }
        public SelectList SelectGroup { get; set; }

        public BannerViewModel()
        {
            var listgroup = new Dictionary<int, string>
            {
                { 1, "Lợi ích" },
                { 2, "Ảnh xe" },
                { 3, "Phản hồi" },
                { 4, "Những con số" }
            };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }

    public class ListBannerViewModel
    {
        public PagedList.IPagedList<Banner> Banners { get; set; }

        public int? GroupId { get; set; }

        public SelectList SelectGroup { get; set; }

        public ListBannerViewModel()
        {
            var listgroup = new Dictionary<int, string>
            {
                { 1, "Lợi ích" },
                { 2, "Ảnh xe" },
                { 3, "Phản hồi" },
                { 4, "Những con số" }
            };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
}
