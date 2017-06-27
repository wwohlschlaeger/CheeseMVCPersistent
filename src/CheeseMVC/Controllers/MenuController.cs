using System.Collections.Generic;
using System.Linq;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
	public class MenuController : Controller
	{
		// GET: /<controller>/

		private readonly CheeseDbContext context;

		public MenuController(CheeseDbContext dbContext)
		{
			context = dbContext;
		}

		public IActionResult Index()
		{
			List<Menu> menus = context.Menus.ToList();
			ViewBag.Title = "Current Menus";
			ViewBag.menus = menus;
			return View(menus);
		}

		public IActionResult Add()
		{
			AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
			return View(addMenuViewModel);
		}

		[HttpPost]
		public IActionResult Add(AddMenuViewModel addMenuViewModel)
		{
			if (ModelState.IsValid)
			{
				Menu newMenu = new Menu()
				{
					Name = addMenuViewModel.Name
				};
				context.Menus.Add(newMenu);
				context.SaveChanges();
				return Redirect("/Menu"); // vid has just /Menu as redirect
			}
			else
				return View(addMenuViewModel);
		}

		[HttpGet]
		public IActionResult ViewMenu(int id)
		{
			ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel()
			{
				Menu = context.Menus.Single(c => c.ID == id),
				Items = context.CheeseMenus
								.Include(item => item.Cheese)
								.Where(cm => cm.MenuID == id)
								.ToList(),
			};

			return View(viewMenuViewModel);
		}

		[HttpGet] // /Menu/AddItem/id
		public IActionResult AddItem(int id)
		{
			List<Cheese> cheeses = context.Cheeses.ToList();
			Menu menu = context.Menus.Single(m => m.ID == id);
			//AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel()
			//{
			//	Menu = context.Menus.Single(m => m.ID == id),
			//	Cheeses = context.Cheeses.ToList()
			//};
			return View(new AddMenuItemViewModel(menu, cheeses));
		}

		[HttpPost]
		public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
		{
			if (ModelState.IsValid)
			{
				var cheeseID = addMenuItemViewModel.CheeseID;
				var menuID = addMenuItemViewModel.MenuID;
				IList<CheeseMenu> existingItems = context.CheeseMenus
													.Where(cm => cm.CheeseID == cheeseID)
													.Where(cm => cm.MenuID == menuID).ToList();

				if (existingItems.Count == 0)
				{
					CheeseMenu menuItem = new CheeseMenu();
					menuItem.Cheese = context.Cheeses.Single(c => c.ID == cheeseID);
					menuItem.Menu = context.Menus.Single(m => m.ID == menuID);
					menuItem.CheeseID = cheeseID;
					menuItem.MenuID = menuID;
					
					context.CheeseMenus.Add(menuItem);
					context.SaveChanges();
					return Redirect(string.Format("/Menu/ViewMenu/{0}", addMenuItemViewModel.MenuID));
												// "/Menu/ViewMenu/" + addMenuItemViewModel.MenuID
				}
			}
			return Redirect(string.Format("/Menu/ViewMenu/{0}", addMenuItemViewModel.MenuID));
			// return View(addMenuItemViewModel);
		}


	}
}
