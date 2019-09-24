namespace StockMarket
{
    public class Order
    {
        public Order(Cat seller, string item, int price)
        {
            Seller = seller;
            Item = item;
            Price = price;
        }

        public Cat Seller { get; }
        public string Item { get; }
        public int Price { get; }
    }
}