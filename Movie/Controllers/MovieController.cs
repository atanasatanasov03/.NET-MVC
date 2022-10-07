using Microsoft.AspNetCore.Mvc;
using Movie.Data;
using Movie.Models;
using Movie.ViewModels;

namespace Movie.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDbContext _db;

        public MovieController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Film> movies = _db.Films;
            foreach(var movie in movies)
            {
                var catId = movie.CategoryId;
                if(catId != Guid.Empty) { 
                    ViewData[movie.Name] = _db.Categories.FirstOrDefault(cat => cat.Id == catId).Name;
                } else
                {
                    ViewData[movie.Name] = "Category Deleted";
                }
            }
            
            return View(movies);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = _db.Categories;
            return View(new CreateMovieViewModel());
        }

        [HttpPost]
        public IActionResult Create(CreateMovieViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Film film = new Film
                {
                    CategoryId = obj.CategoryId,
                    Name = obj.Name
                };
                _db.Add(film);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(Guid id)
        {
            if (id != Guid.Empty)
            {
                Film obj = _db.Find<Film>(id);
                return View(obj);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditPost(Film obj)
        {
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
