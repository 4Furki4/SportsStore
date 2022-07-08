namespace SportsStore.Models
{
    public class Card
    {
        public List<CardLine> Lines { get; set; } = new();

        public virtual void AddItem(Product product, int quantity)
        {
            CardLine? line = Lines.Where(p=>p.Product.Id == product.Id).FirstOrDefault();

            if (line == null)
                Lines.Add(new CardLine { Product = product, Quantity = quantity });
            else
                line.Quantity += quantity;
        }

        public virtual void RemoveLine(Product product)
        {
            Lines.RemoveAll(l => l.Product.Id == product.Id);
        }

        public decimal ComputeTotalValue() =>
            Lines.Sum(e => e.Product.Price * e.Quantity);
        
        public virtual void Clear() => Lines.Clear();
    }
    public class CardLine
    {
        public int CardLineId { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
    }
}
