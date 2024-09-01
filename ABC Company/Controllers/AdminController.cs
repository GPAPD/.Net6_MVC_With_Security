using ABC_Company.Models;
using Domain.Data;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Xml.Schema;

namespace ABC_Company.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : BaseController
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db) 
		{
			_db = db;
		}
		public IActionResult Index()
		{
			return View("Dashbord");
		}

		public IActionResult CreateItem() 
		{
            var model = new AdminItemModel();

            return View(model);
		}

		public IActionResult Save(ItemModel content) 
		{
			try 
			{
                Item item = new Item();
                if (content != null && content.Category > 0)
                {
                    item.Id = content.Id > 0 ? content.Id : 0;
                    FillItemModel(item, content);

                    if (item.Id > 0)
                    {
                        _db.Item.Update(item);
                    }
                    else
                    {
                        _db.Item.Add(item);
                    }

                   _db.SaveChanges();
                }
            }
            catch (Exception ex) 
            {
                 
            }
            return RedirectToAction("Index", "Admin");
        }


        /// <summary>
        /// This is a custome made methord, will take two params 1st one is the entity model type and the second one is the content
		///  /// <param name="model">Is the Entity type.</param>
		///  /// <param name="content">Is ItemModel model type.</param>
        /// </summary>
        public void FillItemModel(Item model, ItemModel content) 
		{
			//model.Id = content.Id;
            model.ItemName = content.ItemName;
            model.Description = content.Description;
            model.Price = content.Price;
            //Get Category data
            var category = _db.Category.FirstOrDefault(a => a.Id == content.Category);
            model.Category = category;
            model.ImageUrl = content.ImageUrl;
        }


        public ActionResult GetAllItemDetails(int id) 
        {
            var model = new CatModel();
            model.Items = new List<Item>();

            var items = _db.Item.Include(c => c.Category).ToList();

            if (items != null) 
            {
                model.Items = items;
            }
            
        
            return View("AdminItemList", model);  
        }

        public ActionResult EditItemDetails(long Id) 
        {
            var model = new AdminItemModel();
            
            var item = _db.Item.Include(c => c.Category).FirstOrDefault(a => a.Id == Id);

            if (item != null) 
            {
                model.Id = item.Id;
                model.ItemName = item.ItemName;
                model.Description = item.Description;
                model.Price = item.Price;
                model.Category = item.Category;
                model.ImageUrl = item.ImageUrl;
            }


            return View("CreateItem", model);
        }

        public ActionResult ViweAllOrders() 
        {

            var model = new OrderModel();

            model.MyOrderList = _db.Order
                  .Include(c => c.Shopcart)
                      .ThenInclude(i => i.ShopCartItems)
                          .ThenInclude(c => c.Item)
                  .ToList();

            return View(model);
        }

        public ActionResult UpdateOrderStatus(long OrdId) 
        {
            OrderModel model = new OrderModel();
            if (OrdId > 0)
            {
                var ord = _db.Order
                    .Where(a => a.Id == OrdId)
                    .Include(cs => cs.Shopcart)
                        .ThenInclude(i => i.ShopCartItems)
                        .ThenInclude(c => c.Item)
                    .FirstOrDefault();

                FillOrderModel(ord, model);
            }

            return View(model);

        }

        public void FillOrderModel(Order content, OrderModel model)
        {
            model.Id = content.Id;
            model.UserId = content.UserId;
            model.Shopcart = content.Shopcart;
            model.Address1 = content.Address1;
            model.Address2 = content.Address2;
            model.City = content.City;
            model.Country = content.Country;
            model.PostalCode = content.PostalCode;
            model.OrderDate = content.OrderDate;
            model.IsComplete = content.IsComplete;
            model.Tax = content.Tax;
            model.ShipCost = content.ShipCost;
            model.SubTotal = content.SubTotal;
            model.Total = content.Total;

        }

        public IActionResult UpdateOrder(OrderModel model) 
        {
            try 
            {
                //OrderModel model = new OrderModel();
                if (model.Id > 0)
                {
                    var ord = _db.Order
                        .Where(a => a.Id == model.Id)
                        .Include(cs => cs.Shopcart)
                            .ThenInclude(i => i.ShopCartItems)
                            .ThenInclude(c => c.Item)
                        .FirstOrDefault();

                    if (ord != null && model.IsComplete != ord.IsComplete) 
                    {
                        ord.IsComplete = model.IsComplete;

                        _db.Order.Update(ord);
                        _db.SaveChanges();
                    
                    }

                    FillOrderModel(ord, model);
                }

            }
            catch { }
        
            return View("UpdateOrderStatus",model);
        }

        //Genarate Order Reports 
        public IActionResult GenarateOrdersReport()
        {
            var orders = _db.Order.ToList();

            // Create the CSV content in memory
            var csvContent = new StringBuilder();

            string[] headers = { "OrderId", "CustomerName", "Address1", "Address2", "City", "Country", "Contact", "SubTotal", "Tax", "ShipCost", "Total", "OrderDate", "Status" };
            csvContent.AppendLine(RowToString(headers));

            foreach (var data in orders)
            {
                string[] rowData = { data.Id.ToString(), data.FristName + " " + data.SecondName, data.Address1, data.Address2, data.City, data.Country, data.Contact,
                             data.SubTotal.ToString(), data.Tax.ToString(), data.ShipCost.ToString(),
                             data.Total.ToString(), data.OrderDate.ToString("MM/dd/yyyy"), data.IsComplete ? "Shipped" : "Pending" };

                csvContent.AppendLine(RowToString(rowData));
            }

            var bytes = Encoding.UTF8.GetBytes(csvContent.ToString());

            return File(bytes, "text/csv", "OrderReport.csv");
        }

        private string RowToString(string[] row)
        {
            return string.Join(",", row);
        }
    }

}
