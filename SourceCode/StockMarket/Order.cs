using System;

namespace StockMarket
{
    public class Order
    {
        public Order(Cat seller, string item, int price, TimeSpan timestamp)
        {
            Seller = seller;
            Item = item;
            Price = price;
            Timestamp = timestamp;
        }

        public Cat Seller { get; }
        public string Item { get; }
        public int Price { get; }
        public TimeSpan Timestamp { get; }
    }
}