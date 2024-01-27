using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Model;

namespace MyClass.DAO
{
    public class CategoryDao
    {
        private BanAoDaiDBContext db = new BanAoDaiDBContext();
        public List<Category> getList(string status="All")
        {
            List<Category> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list   = db.Categorys.Where(x => x.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categorys.Where(x => x.Status == 0).ToList();
                        break;
                    }
                    default:
                    {
                      list=  db.Categorys.ToList();
                        break;
                    }
            }
     
            return list;
        }
        public Category getRow(int? id)
        {
            if (id == null)
            {
                return null;

            }
            else
            {
                return db.Categorys.Find(id);
            }

        }
        //add
        public int Insert(Category row)
        {
            db.Categorys.Add(row);
            return db.SaveChanges();
        }
        //update
        public int Update(Category row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Delete
        public int Delete(Category row)
        {
            db.Categorys.Remove(row);
            return db.SaveChanges();
        }
    }
}
