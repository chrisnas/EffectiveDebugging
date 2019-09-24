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
        private readonly DispatcherTimer _timer;
        private readonly Clock _clock = new Clock();
        private Random _rnd = new Random(42);
        private TimeSpan _time;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += TimerTick;

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

        private void TimerTick(object sender, EventArgs e)
        {
            Time += TimeSpan.FromMinutes(1);
            _clock.Tick();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            _timer.Start();

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
                    var desire = _rnd.Next(0, 6);

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
                    else if (desire == 5)
                    {
                        // The cat wants more! Is there any for sale, and can we afford it?
                        var orders = FindOrders(item);

                        var bestOrder = orders.Where(o => o.Seller != cat).OrderBy(o => o.Price).FirstOrDefault();

                        if (bestOrder == null || bestOrder.Price > cat.Balance)
                        {
                            // Meow :(
                            continue;
                        }

                        // Gimme! Moar! Take my monnies!
                        Buy(bestOrder, cat);
                    }
                }

                _clock.WaitForNextCycle();
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
            lock (Orders)
            {
                Orders.Add(new Order(seller, item, price));
            }
        }

        private void Buy(Order order, Cat buyer)
        {
            lock (buyer)
            {
                _clock.WaitForNextCycle();

                lock (order.Seller)
                {
                    _clock.WaitForNextCycle();

                    // Check that the order is still valid
                    if (order.Seller.Inventory[order.Item] < 1 || buyer.Balance < order.Price)
                    {
                        return;
                    }

                    // The actual transaction
                    buyer.Balance -= order.Price;
                    order.Seller.Balance += order.Price;
                    buyer.Inventory[order.Item] += 1;
                    order.Seller.Inventory[order.Item] -= 1;

                    lock (Orders)
                    {
                        Orders.Remove(order);
                    }

                    // Refresh display and update transaction history
                    buyer.Refresh();
                    order.Seller.Refresh();

                    lock (TransactionHistory)
                    {
                        TransactionHistory.Add($"{buyer.Name} bought {order.Item} from {order.Seller.Name} for ${order.Price}");
                    }
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
