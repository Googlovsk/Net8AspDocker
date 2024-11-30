using Microsoft.AspNetCore.Mvc;
using NET8ASP.Data.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NET8ASP.Views.Shared.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public AppDbContext db;
        public NavigationMenuViewComponent(AppDbContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["catId"];
            return View(db.Categories.ToList());
        }
    }
}
