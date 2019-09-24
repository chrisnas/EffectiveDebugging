using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace StockMarket
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly Clock _clock;
        private Random _rnd = new Random(42);
        private TimeSpan _time;

        public MainWindow()
        {
            InitializeComponent();

            _clock = new Clock(OnTick);

            Cats = new ObservableCollection<Cat>
            {
                new Cat("Felix", _rnd.Next(0, 10), _rnd.Next(0, 10), _rnd.Next(0, 10)),
                new Cat("Garfield", _rnd.Next(0, 10), _rnd.Next(0, 10), _rnd.Next(0, 10)),
                new Cat("Grumpy", _rnd.Next(0, 10), _rnd.Next(0, 10), _rnd.Next(0, 10))
            };

            Orders = new ObservableCollection<Order>();
            BindingOperations.EnableCollectionSynchronization(Orders, Orders);

            TransactionHistory = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(TransactionHistory, TransactionHistory);

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TimeSpan Time
        {
            get => _time;
            private set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Cat> Cats { get; }

        public ObservableCollection<Order> Orders { get; }

        public ObservableCollection<string> TransactionHistory { get; }

        private void OnTick()
        {
            Time += TimeSpan.FromMinutes(1);
            ClearExpiredOrders();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            _clock.Start();

            foreach (var cat in Cats)
            {
                Task.Factory.StartNew(CatThread, (cat, new Random(_rnd.Next())), TaskCreationOptions.LongRunning);
            }
        }

        private void CatThread(object state)
        {
            (var cat, var rnd) = (ValueTuple<Cat, Random>)state;

            var perceivedValue = new Dictionary<string, int>();

            while (true)
            {
                // Refresh the cat's perceived value for each item
                foreach (var item in Cat.Items)
                {
                    perceivedValue[item] = rnd.Next(1, 10);
                }

                foreach (var item in Cat.Items)
                {
                    var desire = _rnd.Next(0, 4);

                    if (desire == 0)
                    {
                        // The cat does not want this thing anymore
                        if (cat.Inventory[item] == 0)
                        {
                            // The cat does not own any anyway
                            continue;
                        }

                        // Check that we're not already selling it
                        var order = FindOrders(item).FirstOrDefault(o => o.Seller == cat && o.Item == item);

                        if (order != null)
                        {
                            // We are already selling it! Never mind then
                            continue;
                        }

                        // Gimme the monnies
                        Sell(item, cat, perceivedValue[item]);
                    }
                    else if (desire == 3)
                    {
                        // The cat wants more! Is there any for sale, and can we afford it?
                        var orders = FindOrders(item);

                        var bestOrders = orders.Where(o => o.Seller != cat && o.Price <= cat.Balance).OrderBy(o => o.Price);

                        foreach (var order in bestOrders)
                        {
                            // Gimme! Moar! Take my monnies!
                            if (Buy(order, cat))
                            {
                                break;
                            }
                        }
                    }
                }

                _clock.WaitForNextCycle();
            }
        }

        private void ClearExpiredOrders()
        {
            lock (Orders)
            {
                var expiredOrders = Orders.Where(o => (Time - o.Timestamp).TotalMinutes > 5).ToList();

                foreach (var order in expiredOrders)
                {
                    lock (order.Seller)
                    {
                        Orders.Remove(order);
                        order.Seller.PendingOrders -= 1;
                    }
                }
            }
        }

        private IReadOnlyList<Order> FindOrders(string item)
        {
            lock (Orders)
            {
                return Orders.Where(o => o.Item == item).ToList();
            }
        }

        private void Sell(string item, Cat seller, int price)
        {
            lock (seller)
            {
                if (seller.PendingOrders < 2)
                {
                    lock (Orders)
                    {
                        Orders.Add(new Order(seller, item, price, Time));
                        seller.PendingOrders += 1;
                    }
                }
            }
        }

        private bool Buy(Order order, Cat buyer)
        {
            lock (order.Seller)
            {
                _clock.WaitForNextCycle();

                lock (buyer)
                {
                    _clock.WaitForNextCycle();

                    lock (Orders)
                    {
                        // Is the order still there?
                        if (!Orders.Contains(order))
                        {
                            return false;
                        }
                    }

                    // Check that the order is still valid
                    if (order.Seller.Inventory[order.Item] < 1 || buyer.Balance < order.Price)
                    {
                        return false;
                    }

                    // The actual transaction
                    buyer.Balance -= order.Price;
                    order.Seller.Balance += order.Price;
                    buyer.Inventory[order.Item] += 1;
                    order.Seller.Inventory[order.Item] -= 1;

                    lock (Orders)
                    {
                        Orders.Remove(order);
                        order.Seller.PendingOrders -= 1;
                    }

                    // Refresh display and update transaction history
                    buyer.Refresh();
                    order.Seller.Refresh();

                    lock (TransactionHistory)
                    {
                        TransactionHistory.Add($"{buyer.Name} bought {order.Item} from {order.Seller.Name} for ${order.Price}");
                    }

                    return true;
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
