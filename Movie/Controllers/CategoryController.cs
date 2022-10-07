using Microsoft.AspNetCore.Mvc;
using Movie.Data;
using Movie.Models;

namespace Movie.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.Categories;
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj) {
            if(ModelState.IsValid)
            {
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
                Category obj = _db.Find<Category>(id);
                return View(obj);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditPost(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                Category obj = _db.Find<Category>(id);
                return View(obj);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeletePost(Category obj)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Film> films = _db.Films.Where(f => f.CategoryId == obj.Id);
                foreach(var film in films)
                {
                    film.CategoryId = Guid.Empty;
                }
                _db.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
