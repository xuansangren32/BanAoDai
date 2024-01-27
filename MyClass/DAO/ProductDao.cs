using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductDao
    {
        BanAoDaiDBContext db = new BanAoDaiDBContext();

        public Product Select(int id)
        {
            return db.Products.Find(id);
        }
        public List<Product> SearchByKey(string key)
        {
            return db.Products.SqlQuery("Select * From Products Where Name like '%" + key + "%'").ToList();
        }
    }
}
