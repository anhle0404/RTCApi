using RTCApi.IRepo;
using RTCApi.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCApi.Repo.GenericRepo
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected StockManagementContext db { get; set; }
        protected DbSet<T> table = null;
        public GenericRepo()
        {
            db = new StockManagementContext();
            table = db.Set<T>();
        }

        public GenericRepo(StockManagementContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public List<T> GetAll()
        {
            return table.ToList();
        }

        public T GetByID(int id)
        {
            return table.Find(id);
        }

        public int Create(T item)
        {
            table.Add(item);
            return db.SaveChanges();
        }

        public int Update(T item)
        {
            table.Attach(item);
            db.Entry(item).State = EntityState.Modified;
            return db.SaveChanges();
        }
    }
}
