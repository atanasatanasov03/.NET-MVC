using Microsoft.AspNetCore.Mvc;
using Movie.Data;
using Movie.Models;

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
            return View(movies);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = _db.Categories;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Film obj)
        {
            if (ModelState.IsValid)
            {
                obj.CategoryId = Guid.Parse((string)TempData["SelectedCat"]);
                _db.Add(obj);
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
