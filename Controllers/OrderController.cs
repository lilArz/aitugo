using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YandexGoClone.Data;
using YandexGoClone.Models;

namespace YandexGoClone.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Delivery() => View();

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string fromAddress, string toAddress, string deliveryType)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = new Order
            {
                ClientId = user!.Id,
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Status = "Новый",
                Price = deliveryType == "moto" ? 60 : deliveryType == "cargo" ? 289 : 60,
                CreatedAt = DateTime.Now
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return RedirectToAction("MyOrders");
        }

        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _db.Orders
                .Where(o => o.ClientId == user!.Id)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            return View(orders);
        }

        // --- ВОТ СЮДА СЕЛИ НОВЫЕ МЕТОДЫ ДЛЯ ПАНЕЛИ КУРЬЕРА ---

        public async Task<IActionResult> CourierPanel()
        {
            var orders = await _db.Orders
                .Include(o => o.Client)
                .Where(o => o.Status == "Новый")
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null && order.Status == "Новый")
            {
                order.CourierId = user!.Id;
                order.Status = "В пути";
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("CourierPanel");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = "Доставлен";
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("CourierPanel");
        }
    }
}