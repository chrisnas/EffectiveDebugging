using System.Collections.Generic;
using System.ComponentModel;

namespace StockMarket
{
    public class Cat : INotifyPropertyChanged
    {
        public Cat(string name, int tuna, int catnip, int boxes)
        {
            Name = name;

            Inventory = new Dictionary<string, int>
            {
                ["Tuna"] = tuna,
                ["Catnip"] = catnip,
                ["Boxes"] = boxes
            };

            Balance = 50;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<string> Items
        {
            get
            {
                yield return "Tuna";
                yield return "Catnip";
                yield return "Boxes";
            }
        }

        public string Name { get; }
        public Dictionary<string, int> Inventory { get; }
        public int Balance { get; set; }

        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}