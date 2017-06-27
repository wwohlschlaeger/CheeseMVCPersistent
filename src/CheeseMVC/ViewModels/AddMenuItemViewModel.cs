using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
	public class AddMenuItemViewModel
	{
		public int CheeseID { get; set; }
		public int MenuID { get; set; }
		public Menu Menu { get; set; }
		public List<SelectListItem> Cheeses { get; set; }

		public AddMenuItemViewModel()
		{

		}

		public AddMenuItemViewModel(Menu menu, IEnumerable<Cheese> cheeses)
		{
			Cheeses = new List<SelectListItem>();
			foreach (var cheese in cheeses)
			{
				Cheeses.Add(new SelectListItem
				{
					Value = cheese.ID.ToString(), // I thought it was supposed to be Cheese.ID.ToString(),
					Text = cheese.Name
				});
			}

			Menu = menu;
			MenuID = menu.ID;
			
		}
	}
}
