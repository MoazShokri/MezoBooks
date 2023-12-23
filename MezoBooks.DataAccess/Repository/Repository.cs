using MezoBooks.DataAccess.Repository.IRepository;
using MezoBooksWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MezoBooks.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
            this._db = db;
            this.dbset = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
           
        }

        public T Get(Expression<Func<T, bool>> filter , string? includeProperies = null , bool tracked = false)
        {
            //IQueryable<T> query = dbset;
            //query = query.Where(filter);

            //if (!string.IsNullOrEmpty(includeProperies))
            //{
            //    foreach(var includeProp in includeProperies
            //        .Split(new char[] {','} , StringSplitOptions.RemoveEmptyEntries))
            //    { 
            //        query= query.Include(includeProp);
            //    }
            //}
            //return query.FirstOrDefault();
            //////////////////////////
            if (tracked)
            {
                IQueryable<T> query = dbset;

                query = query.Where(filter);
                if (includeProperies != null)
                {
                    foreach (var includeProp in includeProperies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();
            }
            else
            {
                IQueryable<T> query = dbset.AsNoTracking();

                query = query.Where(filter);
                if (includeProperies != null)
                {
                    foreach (var includeProp in includeProperies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();
            }


        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null ,string ? includeProperies = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
            {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(includeProperies))
            {
                foreach (var includeProp in includeProperies
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();

        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }
    }
}
