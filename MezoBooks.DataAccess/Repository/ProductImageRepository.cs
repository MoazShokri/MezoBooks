using MezoBooks.DataAccess.Repository.IRepository;
using MezoBooks.Models;
using MezoBooksWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MezoBooks.DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage> , IProductImageRepository    
    {
        private readonly ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(ProductImage obj)
        {
           _db.ProductImages.Update(obj);
        }
    }
}
