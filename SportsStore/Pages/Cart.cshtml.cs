using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;

        public CartModel(IStoreRepository repository, Card cardService)
        {
            this.repository = repository;
            Card = cardService;
        }

        public Card Card { get; set; }

        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";

            //Card = HttpContext.Session.GetJson<Card>("cart") ?? new Card();
        }

        public IActionResult OnPost(long Id, string returnUrl)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.Id == Id);
            if(product != null)
            {
                Card.AddItem(product, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long ProductID, string returnUrl)
        {
            Card.RemoveLine(Card.Lines.First(cl => cl.Product.Id == ProductID).Product);

            return RedirectToPage(new { returnUrl = returnUrl });

        }
    }
}
