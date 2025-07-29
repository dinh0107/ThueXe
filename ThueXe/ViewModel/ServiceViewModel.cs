using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThueXe.Models;

namespace ThueXe.ViewModel
{
    public class ListPriceViewModel
    {
        public IEnumerable<CarServicePrice> CarServicePrices { get; set; }
        public string Name { get; set; }
        public CarService CarService { get; set; }
    }
    public class ListDetailViewModel
    {
        public IEnumerable<CarServiceDetail> CarServiceDetails { get; set; }
        public string Name { get; set; }
        public CarService CarService { get; set; }
    }
    public class PriceViewModel
    {
        public CarService CarService { get; set; }
        public CarServicePrice  CarServicePrice { get; set; }
    }
    public class DetailViewModel
    {
        public CarService CarService { get; set; }
        public CarServiceDetail CarServiceDetail { get; set; }
    }
}