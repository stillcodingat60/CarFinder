using Bing;
using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarFinder.Controllers
{
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext(); // ApplicationDbContext is what connects to the DB via Web.config
        
        /// <summary>
        /// GetAllYears returns DISTINCT values related to the CarFinder dataset
        /// </summary>
        /// <returns>
        /// No params required
        /// Returns only the Year (YYYY) in string format
        /// Simple
        /// </returns>
        /// 

        public IHttpActionResult GetAllYears()
        {
            var retval = db.Database.SqlQuery<string>(
            "EXEC GetAllYears").ToList();

            return Ok(retval);
        }

        /// <summary>
        /// Get Makes By Year, return the entire class Car
        /// </summary>
        /// <param name="year"></param>
        /// <returns>
        /// Get all makes for a particular car make by model year. takes a single param, string year.
        /// </returns>

         public IHttpActionResult GetMakesbyYear(string year)
        {
            var Syear = new SqlParameter("@year", year);
            var retval=db.Database.SqlQuery<Car>(
                "exec GetMakesbyYear @year", Syear).ToList();

             return Ok(retval);
        }

         public IHttpActionResult GetModelsbyYearMake(string year, string make)
         {
             var Syear = new SqlParameter("@year", year);
             var Smake = new SqlParameter("@make", make);
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetModelsbyYearMake @year, @make", Syear, Smake).ToList();

             return Ok(retval);
         }

        /// <summary>
        /// Get Auto Trims by Year, Make & Model
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>
        /// This stored proc takes 3 params as input - Year (model), Make, and Model (e.g., Camry). The proc returns the entire class
        /// of car.
        ///</returns>

         public IHttpActionResult GetTrimsbyYMM(string year, string make, string model)
         {
             var Syear = new SqlParameter("@year", year);
             var Smake = new SqlParameter("@make", make);
             var Smodel = new SqlParameter("@model", model);
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetTrimsbyYMM @year, @make, @model", Syear, Smake, Smodel).ToList();

             return Ok(retval);
         }

        /// <summary>
        /// Get Cars by Year
        /// </summary>
        /// <param name="year"></param>
        /// <returns>
        /// Get Cars By Year takes Year as a param and returns the entire class  of Car.
        /// </returns>

         public IHttpActionResult GetCarsbyYear(string year)
         {
             var Syear = new SqlParameter("@year", year);
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetCarsbyYear @year", Syear).ToList();

             return Ok(retval);
         }

        /// <summary>
        /// Get Cars by Year and Make
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns>
        /// Get Cars by Year and Make takes Year and Make as params and returns the entire class of Car.  
        /// </returns>

         public IHttpActionResult GetCarsbyYearMake(string year, string make)
         {
             var Syear = new SqlParameter("@year", year);
             var Smake = new SqlParameter("@make", make);
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetCarsbyYearMake @year, @make", Syear, Smake).ToList();

             return Ok(retval);
         }

        /// <summary>
        /// Get Cars by Year, Make, & Model
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>
        /// Get Cars by Year, Make, & Model (GetCarsbyYMM) takes 3 params and returns the entire class of Car.
        /// </returns>

         public IHttpActionResult GetCarsbyYMM(string year, string make, string model)
         {
             var Syear = new SqlParameter("@year", year);
             var Smake = new SqlParameter("@make", make);
             var Smodel = new SqlParameter("@model", model);
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetCarsbyYMM @year, @make, @model", Syear, Smake, Smodel).ToList();

             return Ok(retval);
         }

        /// <summary>
        /// Get Cars by Year, Make, Model, and Trims
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns>
        /// Get Cars By Year, Make, Model, & Trim (GetCarsbyYMMT) takes 4 params and returns the entire class of Car.
        /// </returns>

         public IHttpActionResult GetCarsbyYMMT(string year, string make, string model, string trim)
         {
             var Syear = new SqlParameter("@year", year);
             var Smake = new SqlParameter("@make", make);
             var Smodel = new SqlParameter("@model", model);
             var Strim = new SqlParameter("@trim", trim);
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetCarsbyYMMT @year, @make, @model, @trim", Syear, Smake, Smodel, Strim).ToList();

             return Ok(retval);
         }

        /// <summary>
        /// get Cars by Year, Make, Model, & Trim
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns>
        /// get Cars by Year, Make, Model, & Trim (getCarsbyYearMakeModelTrim) takes 4 params. Params can be alternately blank or
        /// completely filled in.
        /// </returns>

         public IHttpActionResult getCarsbyYearMakeModelTrim(string year, string make, string model, string trim)
         {
             var Syear = new SqlParameter("@year", year ?? "");
             var Smake = new SqlParameter("@make", make ?? "");
             var Smodel = new SqlParameter("@model", model ?? "");
             var Strim = new SqlParameter("@trim", trim ?? "");
             var retval = db.Database.SqlQuery<Car>(
                 "exec GetCarsbyYearMakeModelTrim @year, @make, @model, @trim", Syear, Smake, Smodel, Strim).ToList();

             return Ok(retval);
         }

         public async Task <IHttpActionResult> getCar(int Id)
         {
             HttpResponseMessage response;
             string content = "";
             var Car = db.Cars.Find(Id);
             var Recalls = "";
             var Image = "";
             using (var client = new HttpClient())
             {
                 client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                 try
                 {
                     response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + Car.model_year +
                                                                                     "/make/" + Car.make +
                                                                                     "/model/" + Car.model_name + "?format=json");
                     content = await response.Content.ReadAsStringAsync();
                 }
                 catch (Exception e)
                 {
                     return InternalServerError(e);
                 }
             }
             Recalls = content;

             var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));

             image.Credentials = new NetworkCredential("accountKey", "qZQohZcFCgVtaM/LA7T+zQyJiQiio4LDnVrIbRl04No=");   //"s8cUIpKPWJ609E7VqtBbx9HNdRp9Z2NbUne/HyPgxbQ"
             var marketData = image.Composite(
                 "image",
                 Car.model_year + " " + Car.make + " " + Car.model_name + " " + Car.model_trim,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null,
                 null
                 ).Execute();

             Image = marketData.First().Image.First().MediaUrl;
             return Ok(new { car = Car, recalls = Recalls, image = Image });
             //return Ok(Recalls);
         }

    }
}
