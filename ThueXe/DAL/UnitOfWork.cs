using ThueXe.Models;
using System;

namespace ThueXe.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataEntities _context = new DataEntities();

        private GenericRepository<Admin> _adminRepository;
        private GenericRepository<ArticleCategory> _articategoryRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<ConfigSite> _configRepository;
        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<Driver> _driverRepository;
        private GenericRepository<Expense> _expenseRepository;
        private GenericRepository<Introduce> _introduceRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<ProductCategory> _productCategoryRepository;
        private GenericRepository<Trip> _tripRepository;
        private GenericRepository<Voucher> _voucherRepository;

        public GenericRepository<Admin> AdminRepository =>
            _adminRepository ?? (_adminRepository = new GenericRepository<Admin>(_context));
        public GenericRepository<Article> ArticleRepository =>
            _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository =>
            _articategoryRepository ?? (_articategoryRepository = new GenericRepository<ArticleCategory>(_context));
        public GenericRepository<Banner> BannerRepository =>
            _bannerRepository ?? (_bannerRepository = new GenericRepository<Banner>(_context));
        public GenericRepository<ConfigSite> ConfigSiteRepository =>
            _configRepository ?? (_configRepository = new GenericRepository<ConfigSite>(_context));
        public GenericRepository<Contact> ContactRepository =>
            _contactRepository ?? (_contactRepository = new GenericRepository<Contact>(_context));
        public GenericRepository<Customer> CustomerRepository =>
            _customerRepository ?? (_customerRepository = new GenericRepository<Customer>(_context));
        public GenericRepository<Driver> DiverRepository =>
            _driverRepository ?? (_driverRepository = new GenericRepository<Driver>(_context));
        public GenericRepository<Expense> ExpenseRepository =>
            _expenseRepository ?? (_expenseRepository = new GenericRepository<Expense>(_context));
        public GenericRepository<Introduce> IntroduceRepository =>
            _introduceRepository ?? (_introduceRepository = new GenericRepository<Introduce>(_context));
        public GenericRepository<Product> ProductRepository =>
            _productRepository ?? (_productRepository = new GenericRepository<Product>(_context));
        public GenericRepository<ProductCategory> ProductCategoryRepository =>
            _productCategoryRepository ?? (_productCategoryRepository = new GenericRepository<ProductCategory>(_context));
        public GenericRepository<Trip> TripRepository =>
            _tripRepository ?? (_tripRepository = new GenericRepository<Trip>(_context));
        public GenericRepository<Voucher> VoucherRepository =>
            _voucherRepository ?? (_voucherRepository = new GenericRepository<Voucher>(_context));

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
