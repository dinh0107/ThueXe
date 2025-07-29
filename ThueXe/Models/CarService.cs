using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThueXe.Models
{
    public class CarService
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [UIHint("EditorBox")]
        public string Slug { get; set; }
        [UIHint("TextArea")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        public virtual ICollection<CarServiceDetail> Details { get; set; }
        public virtual ICollection<CarServicePrice>  CarServicePrices { get; set; }
    }

    public class CarServiceDetail
    {
        public int Id { get; set; }
        public int CarServiceId { get; set; }
        public string Image { get; set; }
        [Required]
        [Display(Name = "Tên xe"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Mô tả"), UIHint("TextArea")]
        public string Desciption { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Loại")]
        public CarServiceType CarServiceType { get; set; }
        public virtual CarService CarService { get; set; }
    }
    public class CarServicePrice
    {
        public int Id { get; set; }
        public int CarServiceId { get; set; }
        [Required]
        [Display(Name = "Lộ trình"), UIHint("TextBox")]
        public string RouteDescription { get; set; }
        [Required]
        [Display(Name = "Giá"), UIHint("TextBox")]
        public string Price { get; set; }
        [Display(Name = "Km"), UIHint("TextBox")]
        public string Km { get; set; }
        public bool Hot { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        public virtual CarService CarService { get; set; }
    }
    public enum CarServiceType
    {
        [Display(Name = "Ưu điểm")]
        Highlight,
        [Display(Name = "Dòng xe")]
        SupportedCar,
        [Display(Name = "Quy trình")]
        ProcessStep,
        [Display(Name = "Cam kết")]
        Policy
    }
}