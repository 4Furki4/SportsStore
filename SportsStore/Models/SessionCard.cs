using SportsStore.Infrastructure;
using System.Text.Json.Serialization;

namespace SportsStore.Models
{
    public class SessionCard : Card
    {
        [JsonIgnore]
        public ISession? Session { get; private set; }

        public static Card GetCard(IServiceProvider serviceProvider)
        {
            ISession? session = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;

            SessionCard card = session?.GetJson<SessionCard>("Card") ?? new();

            card.Session = session;

            return card;
        }
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session?.SetJson("Card", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session?.SetJson("Card", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Card");
        }

    }
}
