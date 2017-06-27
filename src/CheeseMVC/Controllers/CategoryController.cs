﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
	public class CategoryController : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			List<CheeseCategory> categories = new List<CheeseCategory>();
			categories = context.Categories.ToList();
			ViewBag.Title = "Current Cheese Categories";
			ViewBag.categories = categories;
			return View(categories);
		}

		private readonly CheeseDbContext context;

		public CategoryController(CheeseDbContext dbContext)
		{
			context = dbContext;
		}

		public IActionResult Add()
		{
			AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();

			return View(addCategoryViewModel);
		}

		[HttpPost]
		public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
		{
			if (ModelState.IsValid)
			{
				CheeseCategory newCategory = new CheeseCategory()
				{
					Name = addCategoryViewModel.Name
				};
				context.Categories.Add(newCategory);
				context.SaveChanges();
				return Redirect("/Category");
			}
			else
				return View(addCategoryViewModel);
		}

	}



	

}
