﻿using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Mvc;
using ThueXe.Models;

namespace ThueXe.ViewModel
{
    public class ListVoucherViewModel
    {
        public IPagedList<Voucher> Vouchers { get; set; }
    }

    public class CreateVoucherViewModel
    {
        [Display(Name = "Số lượng"), Required(ErrorMessage = "Nhập số lượng mã cần tạo"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Quantity { get; set; }
    }

    public class ActiveVoucherViewModel
    {
        [Display(Name = "Tên khách hàng"), Required(ErrorMessage = "Hãy Tên khách hàng"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), StringLength(10, ErrorMessage = "Tối đa 10 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }
        public int VoucherId { get; set; }
        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public SelectList VoucherSelectList { get; set; }
    }

    public class InsertCustomerViewModel
    {
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập Số điện thoại"), StringLength(10, ErrorMessage = "Tối đa 10 ký tự"), UIHint("TextBox"), Remote("CheckPhoneNumber", "Customer")]
        public string Mobile { get; set; }
        public Customer Customer { get; set; }
    }

    public class ListCustomerViewModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public IPagedList<Customer> Customers { get; set; }

    }

    public class TripViewModel
    {
        public Trip Trip { get; set; }
        [Display(Name = "Giá"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Price { get; set; }
        [Display(Name = "Giá chuyển"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string PriceChange { get; set; }
        [Display(Name = "Giá khuyến mãi"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string PriceSale { get; set; }
        [Display(Name = "Phí cầu đường"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Tolls { get; set; }
        [Display(Name = "Xăng"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Gas { get; set; }
        [Display(Name = "Cọc"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Pile { get; set; }
        [Display(Name = "Khác"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Other { get; set; }
        [Display(Name = "Ngày đi"), Required(ErrorMessage = "Hãy chọn Ngày đi"), UIHint("DateTimePicker")]
        public string FromDate { get; set; }
        [Display(Name = "Ngày về"), UIHint("DateTimePicker")]
        public string ToDate { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Hãy chọn Lái xe")]
        public int DriverId { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
    }

    public class ListCustomerTripViewModel
    {
        public decimal? Revenue { get; set; }
        public decimal? Sales { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Trip> Trips { get; set; }
    }

    public class ListTripViewModel
    {
        public string Code { get; set; }
        public string Mobile { get; set; }
        public string Time { get; set; }
        public int CustomerId { get; set; }
        public int DriverId { get; set; }
        public int Status { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? TotalSales { get; set; }
        public decimal? TotalToll { get; set; }
        public decimal? TotalOther { get; set; }
        public decimal? PriceChange { get; set; }
        public decimal? ActualRevenue { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public IPagedList<Trip> Trips { get; set; }
    }
}