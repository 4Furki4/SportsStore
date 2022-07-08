using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Card card;
        public OrderController(IOrderRepository repositoryService, Card cardService)
        {
            repository = repositoryService;
            card = cardService;
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!card.Lines.Any())
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            if (ModelState.IsValid)
            {
                order.Lines = card.Lines.ToArray();
                repository.SaveOrder(order);
                card.Lines.Clear();
                return RedirectToPage("/Completed", new { orderId = order.OrderID });
            }
            else
                return View();
        }
        public IActionResult Checkout() => View(new Order());
    }
}
