using ABC_Company.Models;
using Domain.Data;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABC_Company.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db) 
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //OrderModel model = new OrderModel();
            var model = new OrderModel();

          model.MyOrderList =  _db.Order
                .Where(a => a.UserId == UserIdClaim)
                .Include(c => c.Shopcart)
                    .ThenInclude(i => i.ShopCartItems)
                        .ThenInclude( c => c.Item)
                .ToList();

            return View(model);
        }

        public IActionResult ViewOrder(long OrdId) 
        {
            OrderModel model = new OrderModel();
            if (OrdId > 0) 
            {
                var ord = _db.Order
                    .Where(a => a.Id == OrdId)
                    .Include(cs => cs.Shopcart)
                        .ThenInclude(i => i.ShopCartItems)
                        .   ThenInclude(c => c.Item)
                    .FirstOrDefault();

                FillOrderModel(ord, model);
            }

            return View(model);
        }

        public void FillOrderModel(Order content , OrderModel model) 
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
            model.Tax=content.Tax;
            model.ShipCost = content.ShipCost;
            model.SubTotal = content.SubTotal;
            model.Total = content.Total;

        }
    }
}
