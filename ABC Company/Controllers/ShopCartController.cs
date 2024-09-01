using ABC_Company.Models;
using Domain.Data;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ABC_Company.Controllers
{
	[Authorize]
    public class ShopCartController : BaseController
	{
		private readonly AppDbContext _db;

		public ShopCartController(AppDbContext db) 
		{
			_db = db;
		}
		public IActionResult Index() 
		{
			ShopCartModel model = new ShopCartModel(); 
			var cart = GetShopCart();
			model.ShopCart = cart;

            NetTotal(model, cart);

            return View("ShopCart",model);
		}

		public IActionResult AddToCart(long ItemId = 0,int qty = 1) 
		{
			ShopCartModel model = new ShopCartModel();
			var cart = GetShopCart();
			try 
			{
			
				if (cart != null)
				{
					var cartItems = _db.ShopCartItems.FirstOrDefault(c => c.Item.Id == ItemId);
					if (cartItems != null && cartItems.Item != null)
					{
						cartItems.Qty += qty;
						cartItems.ItemPrice += cartItems.Item.Price * qty;

                    }
					else
					{
						if (cart.ShopCartItems == null) 
						{
							cart.ShopCartItems = new List<ShopCartItems>();

                        }
						var newItem = _db.Item.Include(a => a.Category).FirstOrDefault(i => i.Id == ItemId);
						cart.ShopCartItems.Add(new ShopCartItems
						{
							ItemId = ItemId,
							ShopCartId = cart.Id,
							Qty = qty,
							ItemPrice = newItem.Price
						});
					}
					_db.SaveChanges();

				}

				model.ShopCart = cart;

                NetTotal(model, cart);

                return RedirectToAction("Index");


            }
			catch (Exception ex) 
			{
			
			}
			return View("ShopCart", model);
		}

		public ShopCart GetShopCart(long cartId = 0) 
		{
			var shopCart = new ShopCart();

			// from session
			if (cartId == 0) 
			{
				shopCart = _db.ShopCart
					.Include(a => a.ShopCartItems)
						.ThenInclude(b => b.Item)
						.Where(c => c.IsCompleted != true)
					.FirstOrDefault(a => a.ApplicationUserId == UserIdClaim);

				if (shopCart == null)
				{
					var Newcart = new ShopCart
					{
						ApplicationUserId = UserIdClaim,
						Tax = 0,
						ShipCost = 0,
						IsCompleted = false,
						UpdatedDate = DateTime.UtcNow,
						ShopCartItems = new List<ShopCartItems>()

					};
					_db.ShopCart.Add(Newcart);
					_db.SaveChanges();

					return Newcart;
				}
				return shopCart;
			}
			shopCart = _db.ShopCart
				.Include(a => a.ShopCartItems)
					.ThenInclude(b => b.Item)
				.FirstOrDefault(a => a.Id == cartId);

			return shopCart;
		}

		public void GetItemTotal(ShopCart ct , ShopCartModel m) 
		{
			decimal subTotal = 0;

			if (ct.ShopCartItems != null && ct.ShopCartItems.Count > 0) 
			{
				for (int i = 0; i < ct.ShopCartItems.Count; i++) 
				{
					var amount = ct.ShopCartItems.ElementAt(i).ItemPrice;
					subTotal += amount;
                }			
			}
			else 
			{
				m.NetTotal = 0;
				m.ShipCost = 0;
				m.SubTotal = 0;
				m.Tax = 0;
			}
			
			m.SubTotal += subTotal;
		}

		public decimal GetTax(decimal subTotal, decimal shipCost) 
		{
            decimal netTotal = 0;

            decimal tax = 0;


            if (subTotal > 0) 
			{
                netTotal = subTotal + shipCost;

                tax = Math.Round(netTotal * 2 / 100, 2); // 10% from the order + shipcost values as tax;
            }
  
            return tax;
		}


        ///<summary>
        ///This methord is for remove item from shopcart
        ///<param name="subTotal">Returns decimal</param>
        ///</summary>
        public decimal GetShppingCost(decimal subTotal) 
		{
			decimal netTotal = 0;

            netTotal = Math.Round(subTotal*1/100,2);// 10% from the order value as tax;


            return netTotal;
					
		}

        public IActionResult RemoveItem(long ItemId) 
		{
            ShopCartModel model = new ShopCartModel();

            var cart = GetShopCart();

            if (cart != null)
            {
                var cartItems = _db.ShopCartItems.FirstOrDefault(c => c.Item.Id == ItemId);

				if (cartItems != null) 
				{
					_db.ShopCartItems.Remove(cartItems);

                }

                _db.SaveChanges();

            }

            model.ShopCart = cart;

			NetTotal(model, cart);

            return View("ShopCart", model);
        }


		public ActionResult ProceedToCheckout(long catId) 
		{
			var model = new OrderModel();

            return View("ShppingAddress",model);
		}

		public void NetTotal(ShopCartModel model,ShopCart cart) 
		{
            GetItemTotal(cart, model);

            if (model.SubTotal > 0)
            {
                model.ShipCost = GetShppingCost(model.SubTotal); // 1% from The order value Shipping

                model.Tax = GetTax(model.SubTotal, model.ShipCost); // 10% from The order + shipcost value Shipping

                model.NetTotal = (model.SubTotal + model.Tax + model.ShipCost);
            }

        }


		public ActionResult AddOrder(OrderModel model)
		{
			var newOrder = new Order();

			try 
			{
                var cart = GetShopCart();

                if (model != null)
                {
                    newOrder = new Order
                    {
                        ShopcartId = cart.Id,
                        UserId = UserIdClaim,
                        FristName = model.FristName,
                        SecondName = model.SecondName,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        City = model.City,
                        PostalCode = model.PostalCode,
                        Country = model.Country,
                        Contact = model.Contact,
                        IsComplete = false,
                        OrderDate = DateTime.Now
                    };
                    if (newOrder.ShopcartId > 0)
                    {
                        if (cart != null)
                        {
                            var shopCart = new ShopCartModel();

                            shopCart.ShopCart = cart;

                            NetTotal(shopCart, cart);

                            if (shopCart.SubTotal > 0)
                            {
                                newOrder.SubTotal = shopCart.SubTotal;
                                newOrder.Tax = shopCart.Tax;
                                newOrder.ShipCost = shopCart.ShipCost;
                                newOrder.Total = shopCart.NetTotal;
								
                            }
                            if (newOrder.Total > 0)
                            {
                                _db.Order.Add(newOrder);
                               int Orderadded =  _db.SaveChanges();

                                if(Orderadded > 0)
                                {
                                    var updateCart = GetShopCart(cart.Id);
                                    if (updateCart != null)
                                    {
                                        updateCart.IsCompleted = true;
                                        _db.SaveChanges();
                                    }

                                }
                            }

                        }
                    }
                }
            }
			catch (Exception ex)
			{
                return RedirectToAction("ShppingAddress",model);
            }

			model.ThisOrder = newOrder;


            return View("ThankYouPage", model);
		}

    }
}
