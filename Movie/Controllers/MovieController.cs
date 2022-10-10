using Microsoft.AspNetCore.Mvc;
using Movie.Data;
using Movie.Models;
using Movie.ViewModels;

namespace Movie.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(AppDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHostEnvironment = webHost;
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
                string fileName = UploadPicture(obj);
                Film film = new Film
                {
                    CategoryId = obj.CategoryId,
                    Name = obj.Name,
                    Description = obj.Description,
                    FilmImage = fileName,
                    CreatedDate = DateTime.Now
                };
                _db.Add(film);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        private string UploadPicture(CreateMovieViewModel obj)
        {
            string fileName = null;
            if(obj.FilmImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "-" + obj.FilmImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    obj.FilmImage.CopyTo(fileStream);
                }
            } else
            {
                fileName = "no-image.svg";  //NQMA KAK DA STANE VINAGI SHE IMA SNIMKA ;P
            }
            return fileName;
        }

        public IActionResult Edit(Guid id)
        {
            if (id != Guid.Empty)
            {
                Film obj = _db.Find<Film>(id);
                
                EditMovieViewModel vm = new EditMovieViewModel()
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    CategoryId = obj.CategoryId
                };
                ViewData["Categories"] = _db.Categories;

                return View(vm);
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

        public IActionResult Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                Film obj = _db.Find<Film>(id);

                return View(obj);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeletePost(Film obj)
        {
            if (ModelState.IsValid)
            {
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
                Film obj = _db.Find<Film>(id);
                Category cat = _db.Find<Category>(obj.CategoryId);

                SingleMovieViewModel vm = new SingleMovieViewModel()
                {
                    movie = obj,
                    category = cat
                };

                return View(vm);
            }
            return RedirectToAction("Index");
        }
    }
}
