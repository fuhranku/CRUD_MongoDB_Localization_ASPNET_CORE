using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using vid_p1_ati_ii.Models;

namespace vid_p1_ati_ii.Controllers
{
    public class HomeController : Controller
    {
        private IMongoDatabase mongoDatabase;

        // Conectar con mongoDB
        public IMongoDatabase GetMongoDatabase()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            return mongoClient.GetDatabase("vid_p1_ati_ii");
        }

        // Cookies para mantener la cultura
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1)}
            );
            return LocalRedirect(returnUrl);
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            // Recuperar la conexión con la BD
            mongoDatabase = GetMongoDatabase();
            mongoDatabase.GetCollection<Student>("Students")
                .InsertOne(student);
            return RedirectToAction("Read");
        }
        // Read
        [HttpGet]
        public IActionResult Read()
        {
            mongoDatabase = GetMongoDatabase();
            var result = mongoDatabase.GetCollection<Student>("Students")
                .Find(FilterDefinition<Student>.Empty).ToList();
            return View(result);
        }
        // Update
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Recuperar conexión
            mongoDatabase = GetMongoDatabase();
            // Filtrar los detalles de 'Students' basado en el ID y se lo pasamos a la vista
            var student = mongoDatabase.GetCollection<Student>("Students")
                .Find<Student>(k => k.StudentID == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Update(Student student)
        {
            mongoDatabase = GetMongoDatabase();
            // Condición WHERE
            var filter = Builders<Student>.Filter.Eq("StudentID", student.StudentID);
            // UPDATE
            var updatestatement = Builders<Student>.Update.Set("StudentID", student.StudentID);
            updatestatement = updatestatement.Set("FirstName", student.FirstName);
            updatestatement = updatestatement.Set("LastName", student.LastName);
            updatestatement = updatestatement.Set("Email", student.Email);

            var result = mongoDatabase.GetCollection<Student>("Students").UpdateOne(filter, updatestatement);
            if (result.IsAcknowledged == false)
                return BadRequest("Unable to update student " + student.FirstName);
            return RedirectToAction("Read");
        }
        // Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Get the database connection  
            mongoDatabase = GetMongoDatabase();
            //fetch the details from studentDB and pass into view  
            Student student = mongoDatabase.GetCollection<Student>("Students").Find<Student>(k => k.StudentID == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Delete(Student student)
        {
            //Get the database connection  
            mongoDatabase = GetMongoDatabase();
            //Delete the student record  
            var result = mongoDatabase.GetCollection<Student>("Students").DeleteOne(k => k.StudentID == student.StudentID);
            if (result.IsAcknowledged == false)
            {
                return BadRequest("Unable to Delete student " + student.FirstName);
            }
            return RedirectToAction("Read");
        }
    }
}
