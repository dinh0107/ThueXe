using Helpers;
using PagedList;
using QRCoder;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXe.DAL;
using ThueXe.Models;
using ThueXe.ViewModel;

namespace ThueXe.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class CustomerController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public string GenerateNextCodeCustomer(string lastCode)
        {
            if (string.IsNullOrEmpty(lastCode))
                return "KH0001";

            string prefix = new string(lastCode.TakeWhile(char.IsLetter).ToArray());
            string numberPart = new string(lastCode.SkipWhile(char.IsLetter).ToArray());

            if (int.TryParse(numberPart, out int number))
            {
                number++;
                int numberLength = Math.Max(4, number.ToString().Length);
                return $"{prefix}{number.ToString($"D{numberLength}")}";
            }

            throw new Exception("Định dạng mã không hợp lệ");
        }
        private string GenerateNextCodeVoucher(string lastCode)
        {
            if (string.IsNullOrEmpty(lastCode))
                return "NA0001";

            string prefix = new string(lastCode.TakeWhile(char.IsLetter).ToArray());
            string numberPart = new string(lastCode.SkipWhile(char.IsLetter).ToArray());

            if (int.TryParse(numberPart, out int number))
            {
                number++;
                int numberLength = Math.Max(4, number.ToString().Length);
                return $"{prefix}{number.ToString($"D{numberLength}")}";
            }

            throw new Exception("Định dạng mã không hợp lệ.");
        }
        private string GenerateQRCodeImage(string code)
        {
            var imgPath = "/images/vouchers/" + DateTime.Now.ToString("yyyy/MM/dd");
            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

            string fileName = $"{code}.png";
            string filePath = Path.Combine(imgPath, fileName);
            string qrName = $"{Request.Url.Scheme}://{Request.Url.Authority}/khach-hang/voucher/{code}";

            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(qrName, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            using (var bitmap = qrCode.GetGraphic(20))
            {
                bitmap.Save(Server.MapPath(filePath), ImageFormat.Png);
            }

            return fileName;
        }

        #region 
        [Route("danh-sach-voucher")]
        public PartialViewResult ListVoucher(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 50;

            var vouchers = _unitOfWork.VoucherRepository.GetQuery(orderBy: o => o.OrderBy(a => a.Id));

            var model = new ListVoucherViewModel
            {
                Vouchers = vouchers.ToPagedList(pageNumber, pageSize)
            };

            return PartialView(model);
        }
        [Route("them-voucher")]
        public ActionResult CreateVoucher(string result)
        {
            ViewBag.Result = result;
            return View();
        }
        [Route("them-voucher")]
        [HttpPost]
        public ActionResult CreateVoucher(CreateVoucherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lastCode = _unitOfWork.VoucherRepository.GetQuery(orderBy: o => o.OrderByDescending(a => a.Id)).FirstOrDefault()?.Code;

                for (var i = 0; i < model.Quantity; i++)
                {
                    string newCode = GenerateNextCodeVoucher(lastCode);
                    lastCode = newCode;

                    var qrImage = GenerateQRCodeImage(newCode);

                    var voucher = new Voucher()
                    {
                        Code = newCode,
                        QRCode = DateTime.Now.ToString("yyyy/MM/dd") + "/" + qrImage
                    };

                    _unitOfWork.VoucherRepository.Insert(voucher);
                }

                _unitOfWork.Save();

                return RedirectToAction("CreateVoucher", new { result = "success" });
            }
            return View(model);
        }
        [Route("sua-voucher")]
        public PartialViewResult UpdateVoucher(int voucherId)
        {
            var voucher = _unitOfWork.VoucherRepository.GetById(voucherId);

            if (voucher == null)
            {
                return null;
            }

            return PartialView(voucher);
        }
        [Route("sua-voucher")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateVoucher(Voucher model)
        {
            var voucher = _unitOfWork.VoucherRepository.GetById(model.Id);

            if (voucher == null)
            {
                return RedirectToAction("CreateVoucher", new { result = "error" });
            }

            if (ModelState.IsValid)
            {
                voucher.Active = model.Active;

                _unitOfWork.VoucherRepository.Update(voucher);
                _unitOfWork.Save();

                return RedirectToAction("ListCustomer", new { result = "voucher" });
            }

            return PartialView(model);
        }
        [Route("kich-hoat-voucher")]
        public ActionResult ActiveVoucher(string code, string reuslt)
        {
            ViewBag.Result = reuslt;
            var voucher = _unitOfWork.VoucherRepository.GetQuery(a => a.Code.Contains(code)).FirstOrDefault();
            if (voucher == null)
            {
                return RedirectToAction("Index", "Mms");
            }

            var model = new ActiveVoucherViewModel
            {
                VoucherId = voucher.Id,
                Customers = _unitOfWork.CustomerRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate))
            };

            return View(model);
        }
        [HttpPost]
        [Route("kich-hoat-voucher")]
        public ActionResult ActiveVoucher(ActiveVoucherViewModel model)
        {
            var voucher = _unitOfWork.VoucherRepository.GetById(model.VoucherId);
            var customer = _unitOfWork.CustomerRepository.GetById(model.CustomerId);
            if (voucher == null)
            {
                return RedirectToAction("ActiveVoucher", new { result = "error" });
            }

            if (ModelState.IsValid)
            {
                if (customer != null)
                {
                    voucher.CustomerId = customer.Id;
                }
                else
                {
                    var checkMobile = _unitOfWork.CustomerRepository.GetQuery(a => a.Mobile.Contains(model.Mobile)).FirstOrDefault();

                    if (checkMobile != null)
                    {
                        ModelState.AddModelError("", @"Số điện thoại đã tồn tại.");
                    }

                    var lastCode = _unitOfWork.CustomerRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate)).FirstOrDefault()?.Code;

                    var newCustomer = new Customer
                    {
                        Name = model.Name,
                        Mobile = model.Mobile,
                        Code = GenerateNextCodeCustomer(lastCode),
                        Active = true
                    };

                    _unitOfWork.CustomerRepository.Insert(newCustomer);
                    _unitOfWork.Save();

                    voucher.CustomerId = newCustomer.Id;
                }

                voucher.Active = true;
                _unitOfWork.Save();
            }

            return View(model);
        }
        #endregion

        #region Customer
        [Route("danh-sach-khach-hang")]
        public ActionResult ListCustomer(int? page, string result, string name, string mobile)
        {
            var pageNumber = page ?? 1;
            var pageSize = 20;
            ViewBag.Result = result;

            var customers = _unitOfWork.CustomerRepository.GetQuery(orderBy: o => o.OrderByDescending(a => a.CreateDate));

            if (!string.IsNullOrEmpty(name))
            {
                customers = customers.Where(a => a.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                customers = customers.Where(a => a.Mobile.Contains(mobile));
            }
            var model = new ListCustomerViewModel
            {
                Customers = customers.ToPagedList(pageNumber, pageSize),
                Name = name,
                Mobile = mobile
            };

            return View(model);
        }
        [Route("them-khach-hang")]
        public ActionResult Customer()
        {
            var model = new InsertCustomerViewModel
            {
                Customer = new Customer()
            };

            return View(model);
        }
        [Route("them-khach-hang")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Customer(InsertCustomerViewModel model)
        {
            var lastCode = _unitOfWork.CustomerRepository.GetQuery(orderBy: o => o.OrderByDescending(a => a.CreateDate)).FirstOrDefault()?.Code;

            if (ModelState.IsValid)
            {
                model.Customer.Code = GenerateNextCodeCustomer(lastCode);
                model.Customer.Mobile = model.Mobile;

                _unitOfWork.CustomerRepository.Insert(model.Customer);
                _unitOfWork.Save();

                return RedirectToAction("ListCustomer", new { result = "success" });
            }
            return View(model);
        }
        [Route("sua-khach-hang")]
        public ActionResult UpdateCustomer(int customerId)
        {
            var customer = _unitOfWork.CustomerRepository.GetById(customerId);

            if (customer == null)
            {
                return RedirectToAction("ListCustomer", new { result = "error" });
            }

            return View(customer);
        }
        [Route("sua-khach-hang")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateCustomer(Customer model)
        {
            var customer = _unitOfWork.CustomerRepository.GetById(model.Id);

            if (customer == null)
            {
                return RedirectToAction("ListCustomer", new { result = "error" });
            }

            var customers = _unitOfWork.CustomerRepository.GetQuery(a => a.Id != customer.Id && a.Mobile.Contains(model.Mobile));

            if (customers.Any())
            {
                ModelState.AddModelError("", @"Số điện thoại đã tồn tại, vui lòng nhập lại.");
            }

            if (ModelState.IsValid)
            {
                customer.Active = model.Active;
                customer.Name = model.Name;
                customer.Mobile = model.Mobile;

                _unitOfWork.CustomerRepository.Update(customer);
                _unitOfWork.Save();

                return RedirectToAction("ListCustomer", new { result = "update" });
            }

            return View(model);
        }
        [HttpPost]
        public bool DeleteCustomer(int customerId)
        {
            var customer = _unitOfWork.CustomerRepository.GetById(customerId);
            
            if (customer == null)
            {
                return false;
            }

            customer.Trips.Clear();
            customer.Vouchers.Clear();

            _unitOfWork.CustomerRepository.Delete(customer);
            _unitOfWork.Save();

            return true;
        }
        public bool QuickUpdateCustomer(bool active = false, int customerId = 0)
        {
            var customer = _unitOfWork.CustomerRepository.GetById(customerId);
            if (customer == null)
            {
                return false;
            }

            customer.Active = active;

            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Trip
        [Route("danh-sach-chuyen-chay")]
        public ActionResult ListTrip(int? page, string code, string mobile, string result, int driverId = 0, int customerId = 0, int status = -1, string time = "month")
        {
            var pageNumber = page ?? 1;
            var pageSize = 20;
            ViewBag.Result = result;

            var trips = _unitOfWork.TripRepository.GetQuery(a => a.Active && a.TypeTrip == TypeTrip.Drive, o => o.OrderByDescending(a => a.FromDate).ThenByDescending(a => a.CreateDate));

            if (!string.IsNullOrEmpty(code))
            {
                trips = trips.Where(a => a.Customer.Code.Contains(code));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                trips = trips.Where(a => a.Customer.Mobile.Contains(mobile));
            }
            if (customerId > 0)
            {
                trips = trips.Where(a => a.CustomerId == customerId);
            }
            if (status >= 0)
            {
                trips = trips.Where(a => a.Status == (StatusTrip)status);
            }
            if (driverId > 0)
            {
                trips = trips.Where(a => a.DriverId == driverId);
            }

            DateTime today = DateTime.Today;
            DateTime startDate, endDate;

            switch (time)
            {
                case "today":
                    startDate = today;
                    endDate = today.AddDays(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "tomorrow":
                    startDate = today.AddDays(1);
                    endDate = startDate.AddDays(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "week":
                    startDate = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
                    endDate = startDate.AddDays(6).AddSeconds(86399);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "nextweek":
                    startDate = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1)).AddDays(7);
                    endDate = startDate.AddDays(6).AddSeconds(86399);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
            }

            var model = new ListTripViewModel
            {
                Code = code,
                Mobile = mobile,
                CustomerId = customerId,
                Status = status,
                Time = time,
                Trips = trips.ToPagedList(pageNumber, pageSize),
                Customers = _unitOfWork.CustomerRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate)),
                DriverId = driverId,
                Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.CreateDate)),
                TotalSales = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.PriceSale ?? a.Price),
                TotalToll = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.Tolls) ?? 0,
                TotalOther = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.Other) ?? 0
            };

            model.TotalRevenue = model.TotalSales - model.TotalToll - model.TotalOther;

            return View(model);
        }
        [Route("danh-sach-chuyen-chuyen")]
        public ActionResult ListTripChange(int? page, string code, string mobile, string result, int customerId = 0, int status = -1, string time = "month")
        {
            var pageNumber = page ?? 1;
            var pageSize = 20;
            ViewBag.Result = result;

            var trips = _unitOfWork.TripRepository.GetQuery(a => a.Active && a.TypeTrip == TypeTrip.Change, o => o.OrderByDescending(a => a.FromDate).ThenByDescending(a => a.CreateDate));

            if (!string.IsNullOrEmpty(code))
            {
                trips = trips.Where(a => a.Customer.Code.Contains(code));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                trips = trips.Where(a => a.Customer.Mobile.Contains(mobile));
            }
            if (customerId > 0)
            {
                trips = trips.Where(a => a.CustomerId == customerId);
            }
            if (status >= 0)
            {
                trips = trips.Where(a => a.Status == (StatusTrip)status);
            }

            DateTime today = DateTime.Today;
            DateTime startDate, endDate;

            switch (time)
            {
                case "today":
                    startDate = today;
                    endDate = today.AddDays(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "tomorrow":
                    startDate = today.AddDays(1);
                    endDate = startDate.AddDays(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "week":
                    startDate = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
                    endDate = startDate.AddDays(6).AddSeconds(86399);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "nextweek":
                    startDate = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1)).AddDays(7);
                    endDate = startDate.AddDays(6).AddSeconds(86399);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
                case "year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    trips = trips.Where(a => a.FromDate >= startDate && a.FromDate <= endDate);
                    break;
            }

            var model = new ListTripViewModel
            {
                Code = code,
                Mobile = mobile,
                CustomerId = customerId,
                Status = status,
                Time = time,
                Trips = trips.ToPagedList(pageNumber, pageSize),
                Customers = _unitOfWork.CustomerRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate)),
                Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.CreateDate)),
                TotalSales = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.PriceSale ?? a.Price),
                PriceChange = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.PriceChange) ?? 0
            };

            model.ActualRevenue = model.TotalSales - model.PriceChange;

            return View(model);
        }
        [Route("chuyen-cua-khach")]
        public PartialViewResult ListTripCustomer(int customerId)
        {
            var trips = _unitOfWork.TripRepository.GetQuery(a => a.Active && a.CustomerId == customerId, o => o.OrderByDescending(a => a.FromDate).ThenByDescending(a => a.CreateDate));

            var model = new ListCustomerTripViewModel
            {
                Trips = trips,
                Sales = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.Price),
                Revenue = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.PriceSale ?? a.Price)
            };

            return PartialView(model);
        }
        public ActionResult AllCustomerTrip(int customerId)
        {
            var customer = _unitOfWork.CustomerRepository.GetById(customerId);

            if (customer == null)
            {
                return RedirectToAction("ListCustomer");
            }

            var trips = _unitOfWork.TripRepository.GetQuery(a => a.CustomerId == customerId, o => o.OrderByDescending(a => a.FromDate).ThenByDescending(a => a.CreateDate));

            var tolls = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.Tolls) ?? 0;
            var others = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.Other) ?? 0;

            var model = new ListCustomerTripViewModel
            {
                Trips = trips,
                Sales = trips.Where(a => a.Status == StatusTrip.Complete).Sum(a => a.PriceSale ?? a.Price),
                Customer = customer
            };

            model.Revenue = model.Sales - tolls - others;

            return View(model);
        }
        [Route("them-chuyen-di")]
        public ActionResult CreateTrip(int customerId)
        {
            var customer = _unitOfWork.CustomerRepository.GetById(customerId);

            if (customer == null)
            {
                return RedirectToAction("ListCustomer");
            }

            var model = new TripViewModel
            {
                Trip = new Trip(),
                CustomerId = customerId,
                Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.CreateDate))
            };

            return View(model);
        }
        [Route("them-chuyen-di")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateTrip(TripViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Trip.CustomerId = model.CustomerId;
                model.Trip.FromDate = Convert.ToDateTime(model.FromDate);

                if (DateTime.TryParse(model.ToDate, out var toDate))
                {
                    model.Trip.ToDate = toDate;
                }

                model.Trip.Price = decimal.TryParse(model.Price?.Replace(".", ""), out var price) ? price : (decimal?)null;
                model.Trip.PriceSale = decimal.TryParse(model.PriceSale?.Replace(".", ""), out var priceSale) ? priceSale : (decimal?)null;
                model.Trip.PriceChange = decimal.TryParse(model.PriceChange?.Replace(".", ""), out var priceChange) ? priceChange : (decimal?)null;
                model.Trip.Tolls = decimal.TryParse(model.Tolls?.Replace(".", ""), out var tolls) ? tolls : (decimal?)null;
                model.Trip.Other = decimal.TryParse(model.Other?.Replace(".", ""), out var other) ? other : (decimal?)null;
                model.Trip.Pile = decimal.TryParse(model.Pile?.Replace(".", ""), out var pile) ? pile : (decimal?)null;

                model.Trip.DriverId = model.DriverId;

                _unitOfWork.TripRepository.Insert(model.Trip);
                _unitOfWork.Save();

                return RedirectToAction("AllCustomerTrip", new { customerId = model.CustomerId });
            }

            model.Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate));

            return View(model);
        }
        [Route("sua-chuyen-di")]
        public ActionResult UpdateTrip(int tripId)
        {
            var trip = _unitOfWork.TripRepository.GetById(tripId);

            if (trip == null)
            {
                return RedirectToAction("ListTrip");
            }

            var model = new TripViewModel
            {
                Trip = trip,
                FromDate = trip.FromDate.ToString("dd/MM/yyyy HH:mm"),
                ToDate = trip.ToDate != null ? Convert.ToDateTime(trip.ToDate).ToString("dd/MM/yyyy HH:mm") : null,
                Price = trip.Price?.ToString("N0"),
                PriceChange = trip.PriceChange?.ToString("N0"),
                PriceSale = trip.PriceSale?.ToString("N0"),
                Tolls = trip.Tolls?.ToString("N0"),
                Other = trip.Other?.ToString("N0"),
                Pile = trip.Pile?.ToString("N0"),
                DriverId = (int)trip.DriverId,
                Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.CreateDate))
            };

            return View(model);
        }
        [Route("sua-chuyen-di")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateTrip(TripViewModel model)
        {
            var trip = _unitOfWork.TripRepository.GetById(model.Trip.Id);

            if (trip == null)
            {
                return RedirectToAction("ListTrip");
            }

            if (ModelState.IsValid)
            {
                trip.From = model.Trip.From;
                trip.To = model.Trip.To;
                trip.FromDate = Convert.ToDateTime(model.FromDate);
                trip.Distance = model.Trip.Distance;
                trip.Note = model.Trip.Note;
                trip.Active = model.Trip.Active;
                trip.Status = model.Trip.Status;
                trip.DriverId = model.Trip.DriverId;
                trip.TypeCar = model.Trip.TypeCar;
                trip.TypeTrip = model.Trip.TypeTrip;
                trip.Source = model.Trip.Source;

                if (DateTime.TryParse(model.ToDate, out var toDate))
                {
                    trip.ToDate = toDate;
                }

                trip.Price = decimal.TryParse(model.Price?.Replace(".", ""), out var price) ? price : (decimal?)null;
                trip.PriceChange = decimal.TryParse(model.PriceChange?.Replace(".", ""), out var priceChange) ? priceChange : (decimal?)null;
                trip.PriceSale = decimal.TryParse(model.PriceSale?.Replace(".", ""), out var priceSale) ? priceSale : (decimal?)null;
                trip.Tolls = decimal.TryParse(model.Tolls?.Replace(".", ""), out var tolls) ? tolls : (decimal?)null;
                trip.Other = decimal.TryParse(model.Other?.Replace(".", ""), out var other) ? other : (decimal?)null;
                trip.Pile = decimal.TryParse(model.Pile?.Replace(".", ""), out var pile) ? pile : (decimal?)null;

                trip.DriverId = model.DriverId;

                _unitOfWork.TripRepository.Update(trip);
                _unitOfWork.Save();

                if (trip.TypeTrip == TypeTrip.Drive)
                {
                    return RedirectToAction("ListTrip", new { result = "update" });
                }
                else
                {
                    return RedirectToAction("ListTripChange", new { result = "update" });
                }
            }

            model.Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate));

            return View(model);
        }
        public bool DeleteTrip(int tripId)
        {
            var trip = _unitOfWork.TripRepository.GetById(tripId);

            if (trip == null)
            {
                return false;
            }

            _unitOfWork.TripRepository.Delete(trip);
            _unitOfWork.Save();

            return true;
        }
        #endregion

        public JsonResult CheckPhoneNumber(string mobile)
        {
            var customer = _unitOfWork.CustomerRepository.GetQuery(a => a.Mobile == mobile).FirstOrDefault();

            if (customer != null)
            {
                return Json("Số điện thoại đã tồn tại, vui lòng nhập lại..", JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}