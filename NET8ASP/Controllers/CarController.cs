using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain;
using NET8ASP.Models.ViewModels;

namespace NET8ASP.Controllers
{
    public class CarController : Controller
    {
        AppDbContext db;
        IWebHostEnvironment env;
        public CarController(AppDbContext context, IWebHostEnvironment environment)
        {
            db = context;
            env = environment;
        }

        public IActionResult CarList()
        {
            return View(db.Cars.Include(c => c.Category).ToList());
        }


        [HttpGet]
        public IActionResult AddCar()
        {
            return View(new AddCarViewModel
            {
                Car = new Car(),
                Categories = db.Categories.ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddCar(Car car, IFormFile uploadedfile)
        {
            if (uploadedfile != null)
            {
                string path = $"/img/{uploadedfile.FileName}";
                car.Image = path;
                using (var filestream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedfile.CopyToAsync(filestream);
                }
            }
            db.Cars.Add(car);
            await db.SaveChangesAsync();

            return RedirectToAction("CarList", "Car");
        }
        [HttpGet]
        public IActionResult EditCar(int? carid)
        {
            if (carid == null)
            {
                return RedirectToAction("Table");
            }
            else
            {
                return View(new AddCarViewModel
                {
                    Car = db.Cars.Find(carid),
                    Categories = db.Categories.ToList()
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditCar(Car car, IFormFile uploadedfile)
        {
            // Загрузить существующую запись из базы данных
            var existingCar = await db.Cars.FindAsync(car.Id);
            if (existingCar == null)
            {
                return NotFound(); // Если запись не найдена, возвращаем ошибку
            }

            // Если файл загружен, обновить поле Image
            if (uploadedfile != null)
            {
                string path = $"/img/{uploadedfile.FileName}";
                using (var filestream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedfile.CopyToAsync(filestream);
                }
                existingCar.Image = path; // Обновляем изображение только если загружен новый файл
            }

            // Обновляем остальные свойства
            existingCar.Name = car.Name;
            existingCar.Description = car.Description;
            existingCar.Price = car.Price;
            existingCar.CategoryId = car.CategoryId;

            // Сохраняем изменения
            await db.SaveChangesAsync();

            return RedirectToAction("CarList", "Car");
        }


        public IActionResult DeleteCar(int? carId)
        {
            if (carId != null)
            {
                db.Cars.Remove(db.Cars.Find(carId));
                db.SaveChanges();
            }
            return RedirectToAction("CarList", "Car");
        }
    }
}
