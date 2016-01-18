using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarFinder.Controllers
{
    public class ProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // ApplicationDbContext is what connects to the DB via Web.config

        //Product[] products = new Product[] 
        //    { 
        //new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
        //new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3 }, 
        //new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16 } 
        //    };
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return products;
        //}

        public IHttpActionResult GetProduct() //(int id)
        {
            
            //var retval = db.Cars.Find(id);   //this will allow an id to be passed to this controller to find the Car

            var retval = db.Database.SqlQuery<string>(
            "EXEC UniqueModels");
            //new SqlParameter("@prodId", id));

            return Ok(retval);

            //var product = products.FirstOrDefault(p => p.Id == id); // LOOK! We’re still using LINQ!
            //if (product == null)
            //{
            //    return NotFound();
            //}
            //return Ok(product);
        } 


    }
}
