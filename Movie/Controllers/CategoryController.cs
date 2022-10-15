using Microsoft.AspNetCore.Mvc;
using Movie.Data;
using Movie.Models;
using Movie.ViewModels;

namespace Movie.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Create(CreateCategoryViewModel obj) {
            if(ModelState.IsValid)
            {
                string fileName = UploadPicture(obj);
                Category cat = new Category
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    CatImage = fileName,
                    CreatedDate = DateTime.Now
                };
                _db.Add(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        private string UploadPicture(CreateCategoryViewModel obj)
        {
            string fileName = null;

            if (obj.CatImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "-" + obj.CatImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    obj.CatImage.CopyTo(fileStream);
                }
            }

            return fileName;
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

        public IActionResult Single(Guid id)
        {
            if (id != Guid.Empty)
            {
                Category obj = _db.Find<Category>(id);
                IEnumerable<Film> movies = _db.Films.Where(f => f.CategoryId == obj.Id);

                SingleCategoryViewModel vm = new SingleCategoryViewModel
                {
                    category = obj,
                    movies = movies,
                    tripletsNum = Math.Ceiling((double)movies.Count() / 3)
                };
                return View(vm);
            }
            return RedirectToAction("Index");
        }
    }
}
