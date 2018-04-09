using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoRatings.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoRatings.Controllers
{
    public class HomeController : Controller
    {
        MongoClient _client;
        IMongoDatabase _database;

        public HomeController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("MongoRatings");
 
        }
       
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public List<Rating> GetDatabase()
        {
            return _database.GetCollection<Rating>("Ratings").Find(FilterDefinition<Rating>.Empty).ToList();
        }

        [HttpPost]
        public IActionResult Create(Rating model)
        {
            if (ModelState.IsValid)
            {
                model.Created_At = DateTime.Now;
                _database.GetCollection<Rating>("Ratings").InsertOne(model);
            }

            var refreshedDb = GetDatabase();

            return View("Ratings", refreshedDb);
        }

        public IActionResult Ratings(string searchQuery)
        {
            var ratings = _database.GetCollection<Rating>("Ratings").Find(FilterDefinition<Rating>.Empty).ToList();

            var search = from r in ratings
                              select r;
            if (!String.IsNullOrEmpty(searchQuery))
                search = search.Where(s => s.Show_Name.Contains(searchQuery));

            return View("Ratings", search.ToList());
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
                return NotFound();

            var rating_to_edit = _database.GetCollection<Rating>("Ratings").Find(i => i.Id == id).FirstOrDefault();

            if (rating_to_edit == null)
                return NotFound();

            return View(rating_to_edit);

        }

        [HttpPost]
        public IActionResult Edit(Rating rating)
        {
            try
            {
                //Build where condition and update statement
                var filter = Builders<Rating>.Filter.Eq("Id", rating.Id);

                var updater = Builders<Rating>.Update.Set("Show_Name", rating.Show_Name);
                updater = updater.Set("Season_Number", rating.Season_Number);
                updater = updater.Set("Episode_Number", rating.Episode_Number);
                updater = updater.Set("Episode_Name", rating.Episode_Name);
                updater = updater.Set("Humor_Rating", rating.Humor_Rating);
                updater = updater.Set("Story_Rating", rating.Story_Rating);
                updater = updater.Set("Overall_Rating", rating.Overall_Rating);

                var result = _database.GetCollection<Rating>("Ratings").UpdateOne(filter, updater);

                if (result.IsAcknowledged == false)
                    return BadRequest("Unable to update the rating for " + rating.Show_Name);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Ratings");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
                return NotFound();

            var rating_to_delete = _database.GetCollection<Rating>("Ratings").Find(i => i.Id == id).FirstOrDefault();

            if (rating_to_delete == null)
                return NotFound();

            return View(rating_to_delete);
        }

        [HttpPost]
        public IActionResult Delete(Rating model)
        {
            try
            {
                var result = _database.GetCollection<Rating>("Ratings").DeleteOne(i => i.Id == model.Id);

                if (result.IsAcknowledged == false)
                    return BadRequest("Unable to delete the rating for " + model.Show_Name);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Ratings");
        }
    }
}
