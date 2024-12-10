using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain;
using NET8ASP.Models.ViewModels;

namespace NET8ASP.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext db;
        IWebHostEnvironment env;
        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            db = context;
            env = environment;
        }
        [HttpGet]
        [Route("/categories/{categoryId}/subcategories")]
        public IActionResult GetSubcategories(int categoryId)
        {
            var subcategories = db.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .Select(c => new { id = c.Id, name = c.Name })
                .ToList();

            return Json(subcategories);
        }
        [HttpGet("list")]
        public IActionResult ProductList(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery;
            var productsQuery = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchQuery));
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()));
            }
            var products = productsQuery.OrderBy(p => p.Id).Include(c => c.Category).ToList();
            var viewModel = new ProductViewModel
            {
                Products = products,
            };
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult AddProduct()
        {
            return View(new ProductViewModel
            {
                Product = new Product(),
                Categories = db.Categories.ToList(),
                Manufacturers = db.Manufacturers.ToList()
                
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile uploadedfile)
        {
            string uploadPath = "/uploads";
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (uploadedfile != null)
            {
                string path = Path.Combine("/uploads", uploadedfile.FileName);
                product.Image = path;
                using (var filestream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedfile.CopyToAsync(filestream);
                }
            }
            else
            {
                product.Image = "/img/SiteData/no_product_image.png";
            }

            Product newProduct = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Amount = product.Amount,
                ManufId = product.ManufId,
                CategoryId = product.CategoryId
            };
            
            db.Products.Add(newProduct);
            await db.SaveChangesAsync();


            return RedirectToAction("ProductList", "Product");
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(int? productId)
        {
            var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);


            var categories = await db.Categories.Where(c => c.ParentCategoryId == null).ToListAsync();
            var parentCategories = categories;
            var subCategories = await db.Categories.Where(c => c.ParentCategoryId != null).ToListAsync();

            var selectListCategories = new SelectList(parentCategories, "Id", "Name", product.CategoryId);


            if (productId == null)
            {
                return RedirectToAction("NAHUI");
            }
            else
            {
                return View(new ProductViewModel
                {
                    Product = product,
                    Categories = parentCategories,
                    SubCategories = subCategories,
                    Manufacturers = db.Manufacturers.ToList(),
                    SelectListCategories = selectListCategories
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel model, IFormFile uploadedfile)
        {
            var existingProduct = await db.Products.FindAsync(model.Product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            string uploadPath = "/uploads";
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (uploadedfile != null)
            {
                string path = Path.Combine("/uploads", uploadedfile.FileName);
                using (var filestream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedfile.CopyToAsync(filestream);
                }
                existingProduct.Image = path;
            }

            existingProduct.Name = model.Product.Name;
            existingProduct.Description = model.Product.Description;
            existingProduct.Amount = model.Product.Amount;
            existingProduct.Price = model.Product.Price;
            existingProduct.CategoryId = model.SubCategoryId ?? model.Product.CategoryId;
            existingProduct.ManufId = model.Product.ManufId;

            db.Entry(existingProduct).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("ProductList");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int? productId)
        {
            if (productId != null)
            {
                db.Products.Remove(db.Products.Find(productId));
                db.SaveChanges();
            }
            return RedirectToAction("ProductList", "Product");
        }
    }
}
