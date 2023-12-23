using MezoBooks.DataAccess.Repository.IRepository;
using MezoBooksWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MezoBooks.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Categories { get; private set; }
        public ICompanyRepository Companies { get; private set; }
         
        public IProductRepository Products { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }



        public UnitOfWork(ApplicationDbContext db)
        {
            this._db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Categories = new CategoryRepository(_db);
            Companies = new CompanyRepository(_db);
            Products = new ProductRepository(_db);
            ShoppingCart= new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            ProductImage= new ProductImageRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges(); 
        }
    }
}
