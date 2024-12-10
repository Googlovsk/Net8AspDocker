using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models;
using NET8ASP.Models.Domain;
using NET8ASP.Models.ViewModels;

namespace NET8ASP.Controllers
{
    //[ApiController]
    public class HomeController : Controller
    {
        private AppDbContext db;
        int pageSize = 16;
        //int maxPage => (int)Math.Ceiling((decimal)db.Products.Count() / pageSize);

        public HomeController(AppDbContext context)
        {
            db = context;
        }
        [AllowAnonymous]
       
        public IActionResult Index(string searchQuery, int? categoryId, int page = 1)
        {
            var products = GetFilteredProducts(searchQuery, categoryId, page, out int totalItems);

            var viewModel = new HomeViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                },
                Products = products,
                CurrentCategory = categoryId,
                Categories = GetRootCategories()
            };

            return View(viewModel);
        }

        private List<Product> GetFilteredProducts(string searchQuery, int? categoryId, int page, out int totalItems)
        {
            var query = db.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                var subCategoryIds = db.Categories
                    .Where(c => c.ParentCategoryId == categoryId)
                    .Select(c => c.Id)
                    .ToList();

                if (subCategoryIds.Any())
                {
                    query = query.Where(p => subCategoryIds.Contains(p.CategoryId));
                }
                else
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()));
            }

            totalItems = query.Count();

            return query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToList();
        }
        private List<Category> GetRootCategories()
        {
            return db.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null)
                .ToList();
        }
        [HttpGet("api/Home")]
        public IActionResult GetProductsApi(string searchQuery, int? categoryId, int page = 1)
        {
            var products = GetFilteredProducts(searchQuery, categoryId, page, out int totalItems);

            var result = new
            {
                PagingInfo = new
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                },
                Products = products.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    Category = new
                    {
                        p.Category.Id,
                        p.Category.Name
                    }
                })
            };

            return Ok(result);
        }

        //[AllowAnonymous]
        //public IActionResult Index(string searchQuery, int? categoryId, int page = 1)
        //{
        //    ViewData["SearchQuery"] = searchQuery;
        //    var productsQuery = db.Products.AsQueryable();



        //    if (categoryId.HasValue)
        //    {
        //        var subCategoryIds = db.Categories
        //            .Where(c => c.ParentCategoryId == categoryId)
        //            .Select(c => c.Id)
        //            .ToList();

        //        if (subCategoryIds.Any())
        //        {
        //            productsQuery = productsQuery.Where(p => subCategoryIds.Contains(p.CategoryId));
        //        }
        //        else
        //        {
        //            productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
        //        }

        //        /// TODO: сделать вывод всех производителей у данной категори, если у данной категории нет подкатегорий
        //        var manufacturerIds = db.Products
        //           .Where(p => p.CategoryId == categoryId ||
        //                       db.Categories
        //                           .Where(c => c.ParentCategoryId == categoryId)
        //                           .Select(c => c.Id)
        //                           .Contains(p.CategoryId))
        //           .Select(p => p.ManufId)
        //           .Distinct()
        //           .ToList();

        //        var manufacturers = db.Manufacturers
        //            .Where(m => manufacturerIds.Contains(m.Id))
        //            .ToList();
        //        ///
        //    }

        //    if (!string.IsNullOrEmpty(searchQuery))
        //    {
        //        productsQuery = productsQuery.Where(p => p.Name.Contains(searchQuery));
        //    }
        //    if (!string.IsNullOrEmpty(searchQuery))
        //    {
        //        productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()));
        //    }
        //    var products = productsQuery
        //        .OrderBy(p => p.Id)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Include(p => p.Category)
        //        .ToList();
        //    var viewModel = new HomeViewModel
        //    {
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = pageSize,
        //            TotalItems = db.Products.Where(p => p.CategoryId == categoryId).Count()
        //        },
        //        Products = products,
        //        CurrentCategory = categoryId,
        //        Categories = db.Categories
        //            .Include(c => c.SubCategories)
        //            .Where(c => c.ParentCategoryId == null)
        //            .ToList()
        //    };
        //    return View(viewModel);
        //}

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
