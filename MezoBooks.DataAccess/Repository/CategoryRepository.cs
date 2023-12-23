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
    public class CategoryRepository : Repository<Category> , ICategoryRepository    
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Category obj)
        {
           _db.Categories.Update(obj);
        }
    }
}
