using Bing;
using CarFinder.Models;
using Newtonsoft.Json;
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
        /// The Selected Class is the class used to pass variable values to the SQL Queries. It consists of
        /// four strings, year, make, model, and trim.
        /// </summary>
        /// <returns>
        /// Class Object 
        /// 
        /// </returns>

        public class Selected
        {
            public string year { get; set; }
            public string make { get; set; }
            public string model { get; set; }
            public string trim { get; set; }
        }

        public class IdParam
        {
            public int id { get; set; }
        }

        /// <summary>
        /// GetAllYears returns DISTINCT values related to the CarFinder dataset
        /// </summary>
        /// <returns>
        /// SQL Server Stored Procedure
        /// No params required
        /// Returns only the Year (YYYY) in string format
        /// Simple SQL call via DISTINCT to populate the YEAR pull down
        /// Uses HttpPost method.
        /// </returns>
        /// 
        [HttpPost]
        public IHttpActionResult GetAllYears(Selected selected)
        {
            var retval = db.Database.SqlQuery<string>(
            "EXEC GetAllYears").ToList();

            return Ok(retval);
        }

        /// <summary>
        /// Get Makes By Year, returns the string variable MAKE
        /// </summary>
        /// <param name="selected">
        /// Class "Selected"
        /// </param>
        /// <returns>
        /// SQL Server Stored Procedure
        /// Get all makes for a particular car make by model year. 
        /// Takes a single param, string year and returns the string MAKE. 
        /// Used to populate the MAKE pull down. 
        /// Uses HttpPost method.
        /// </returns>

        [HttpPost]
        public IHttpActionResult GetMakesbyYear(Selected selected)
        {
            var Syear = new SqlParameter("@year", selected.year);

            var retval = db.Database.SqlQuery<string>(
                "exec GetMakesbyYear @year", Syear).ToList();

            return Ok(retval);
        }

        /// <summary>
        /// Get Models by Year & Make - returns the string Model SQL Server Stored Procedure
        /// </summary>
        /// <param name="selected"></param>
        /// <returns>
        /// SQL Server Stored Procedure
        /// Gets all the models for a particular car year and make.
        /// Uses the values passed to it, year and make.
        /// Returns only the models via a string.
        /// Uses HttpPost method.
        /// </returns>

        [HttpPost]
        public IHttpActionResult GetModelsbyYearMake(Selected selected)
        {
            var Syear = new SqlParameter("@year", selected.year);
            var Smake = new SqlParameter("@make", selected.make);
            var retval = db.Database.SqlQuery<string>(
                "exec GetModelsbyYearMake @year, @make", Syear, Smake).ToList();

            return Ok(retval);
        }

        /// <summary>
        /// Get Automobile Trims by Year, Make & Model
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>
        /// SQL Server Stored Procedure
        /// This stored proc takes 3 params as input - Year (model year), Make, and Model (e.g., Camry). 
        /// The proc returns the string value Trim.
        /// Uses HttpPost method.
        ///</returns>

        [HttpPost]
        public IHttpActionResult GetTrims(Selected selected)
        {
            var Syear = new SqlParameter("@year", selected.year);
            var Smake = new SqlParameter("@make", selected.make);
            var Smodel = new SqlParameter("@model", selected.model);
            var retval = db.Database.SqlQuery<string>(
                "exec GetTrimsbyYMM @year, @make, @model", Syear, Smake, Smodel).ToList();

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
        /// SQL Server Stored Procedure
        /// get Cars by Year, Make, Model, & Trim (getCarsbyYearMakeModelTrim) takes 4 params. Params can be alternately blank or
        /// completely filled in.
        /// Uses HttpPost method.
        /// </returns>

        [HttpPost]
        public IHttpActionResult getCars(Selected selected)
        {
            var Syear = new SqlParameter("@year", selected.year ?? "");
            var Smake = new SqlParameter("@make", selected.make ?? "");
            var Smodel = new SqlParameter("@model", selected.model ?? "");
            var Strim = new SqlParameter("@trim", selected.trim ?? "");
            var retval = db.Database.SqlQuery<Car>(
                "exec GetCarsbyYearMakeModelTrim @year, @make, @model, @trim", Syear, Smake, Smodel, Strim).ToList();

            return Ok(retval);
        }

        /// <summary>
        /// getCar calls the NHTSA and Bing APIs
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// SQL Server Stored Procedure.
        /// getCar returns a single instance of the Car Class plus calls the NHTSA and Bing APIs to retrieve recall data and an image.
        /// getCar is passed an object, class IdParam.
        /// Uses HttpPost method.
        /// </returns>

        [HttpPost]
        public async Task<IHttpActionResult> getCar(IdParam Id)
        {
            HttpResponseMessage response;
            string content = "";
            var Car = db.Cars.Find(Id.id);

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

            dynamic Recalls = JsonConvert.DeserializeObject(content);

            var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/v1/Image"));

            image.Credentials = new NetworkCredential("accountKey", "qZQohZcFCgVtaM/LA7T+zQyJiQiio4LDnVrIbRl04No=");   //"s8cUIpKPWJ609E7VqtBbx9HNdRp9Z2NbUne/HyPgxbQ"
            var marketData = image.Composite(
                "image",
                Car.model_year + " " + Car.make + " " + Car.model_name + " " + Car.model_trim + " " + " NOT EBAY ",
                null, null, null, null, null, null, null, null, null, null, null, null, null
                ).Execute();

            //Image = marketData.First().Image.First().MediaUrl;
            if(marketData != null)
            {
                var Images = marketData.FirstOrDefault().Image;
            


            //int imgCnt = Images.Count();
           
                foreach (var Img in Images)
                {

                    if (UrlCtrl.IsUrl(Img.MediaUrl))
                    {
                        Image = Img.MediaUrl;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            if(string.IsNullOrWhiteSpace(Image))
            {
                Image = "/assets/img/no-image-available.png";
            }
        
            return Ok(new { car = Car, recalls = Recalls, image = Image });
        }

        public static class UrlCtrl
        {
            public static bool IsUrl(string path)
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(path);
                request.Method = "HEAD";
                bool result = true;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    /* A WebException will be thrown if the status of the response is not `200 OK` */
                    result = false;
                }
                finally
                {
                    // Don't forget to close your response.
                    if (response != null)
                    {
                        response.Close();
                    }

                }

                return result;

            }
        }
    }
}


