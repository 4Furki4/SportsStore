using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class CardSummaryViewComponent : ViewComponent
    {
        private Card card;

        public CardSummaryViewComponent(Card card)
        {
            this.card = card;
        }

        public IViewComponentResult Invoke()
        {
            return View(card);
        }
    }
}
