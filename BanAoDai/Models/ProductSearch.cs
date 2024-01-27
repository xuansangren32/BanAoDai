using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanAoDai.Models
{
    public class ProductSearch
    {
        BanAoDaiDBContext db = new BanAoDaiDBContext();
       
        public List<Product> SearchByKey(string key)
        {
            return db.Products.SqlQuery("Select * From Products Where Name like '%" + key + "%'").ToList();
        }
        
    }
}