using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
		IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();


			return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel();
			GetCategories(addCheeseViewModel);
			return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
		{
			if (ModelState.IsValid)
			{
				// Add the new cheese to my existing cheeses
				CheeseCategory newCheeseCategory =
					context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

				Cheese newCheese = new Cheese
				{
					Name = addCheeseViewModel.Name,
					Description = addCheeseViewModel.Description,
					Category = newCheeseCategory
				};

				context.Cheeses.Add(newCheese);
				context.SaveChanges();

				return Redirect("/Cheese");
			}

			GetCategories(addCheeseViewModel);
			return View(addCheeseViewModel);
		}

		private void GetCategories(AddCheeseViewModel addCheeseViewModel)
		{
			addCheeseViewModel.Categories = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
			foreach (var category in context.Categories)
			{
				addCheeseViewModel.Categories.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = category.Name, Value = category.ID.ToString() });
			}
		}

		public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

		/* public IActionResult Edit(int cheeseID)
		{
			ViewBag.title = "Edit Cheeses";
			ViewBag.cheeses = context.Cheeses.ToList();
			return View("/EditItem");
		}

		public IActionResult EditItem(Cheeses cheese)
		{
		AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel();
			addCheeseViewModel.Name = cheese.Name
			GetCategories(addCheeseViewModel);
			return View(addCheeseViewModel);
		}

		[HttpPost]
		public IActionResult EditItem(int cheeseID)
		{
		if (ModelState.IsValid)
			{
				// Add the new cheese to my existing cheeses
				CheeseCategory newCheeseCategory =
					context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

				Cheese newCheese = new Cheese
				{
					Name = addCheeseViewModel.Name,
					Description = addCheeseViewModel.Description,
					Category = newCheeseCategory
				};

				context.Cheeses.Add(newCheese);
				context.SaveChanges();

				return Redirect("/Cheese");
			}

			GetCategories(addCheeseViewModel);
			return View(addCheeseViewModel);
			}

		[HttpPost]
		public IActionResult Edit(int cheeseId, string name, string description)
		{
			foreach (int cheeseId in cheeseIds)
			{
				Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
				context.Cheeses.Remove(theCheese);
			}

			context.SaveChanges();

			return Redirect("/");
			return View();
		}*/
	}
}
