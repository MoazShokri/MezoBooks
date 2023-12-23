using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MezoBooks.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Categories { get; }
        public ICompanyRepository Companies { get; }

        public IProductRepository Products { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IOrderHeaderRepository OrderHeader { get; }
		public IOrderDetailRepository OrderDetail { get; }
        public IProductImageRepository ProductImage { get; }


		void Save();
    }
}
