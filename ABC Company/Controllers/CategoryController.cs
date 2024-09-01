using ABC_Company.Models;
using Domain.Data;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABC_Company.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly AppDbContext _db;
		public CategoryController(AppDbContext db) 
		{
			_db = db;
		}


		public IActionResult Index()
		{
			var cat = new CatModel();
			cat.Items = new List<Item>();

			var itemList = _db.Item.Include(x => x.Category).Where(c=> c.Category.Id == 1).ToList();

			if (itemList != null)
			{
				cat.Items = itemList;
			}
			cat.CatName = "Buy Cars";
			return View("CategoryItem", cat);
		}

		public IActionResult CarParts() 
		{
			var cat = new CatModel();
			cat.Items = new List<Item>();

			var itemList = _db.Item.Include(x => x.Category).Where(c => c.Category.Id == 2).ToList();

			if (itemList != null)
			{
				cat.Items = itemList;
			}
			cat.CatName = "Buy Cars Parts";

			return View("CategoryItem",cat);
		}
	}
}
