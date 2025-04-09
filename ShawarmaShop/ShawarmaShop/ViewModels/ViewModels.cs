using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShawarmaShop.Models;

namespace ShawarmaShop.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public CustomerViewModel()
        {
            Customers = new ObservableCollection<Customer>();
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomer != null)
            {
                var index = Customers.IndexOf(existingCustomer);
                Customers[index] = customer;
            }
        }

        public void DeleteCustomer(Guid customerId)
        {
            var customerToRemove = Customers.FirstOrDefault(c => c.Id == customerId);
            if (customerToRemove != null)
            {
                Customers.Remove(customerToRemove);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SellerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Seller> _sellers;
        public ObservableCollection<Seller> Sellers
        {
            get => _sellers;
            set
            {
                _sellers = value;
                OnPropertyChanged();
            }
        }

        public SellerViewModel()
        {
            Sellers = new ObservableCollection<Seller>();
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public void UpdateSeller(Seller seller)
        {
            var existingSeller = Sellers.FirstOrDefault(s => s.Id == seller.Id);
            if (existingSeller != null)
            {
                var index = Sellers.IndexOf(existingSeller);
                Sellers[index] = seller;
            }
        }

        public void DeleteSeller(Guid sellerId)
        {
            var sellerToRemove = Sellers.FirstOrDefault(s => s.Id == sellerId);
            if (sellerToRemove != null)
            {
                Sellers.Remove(sellerToRemove);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ShawarmaViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Shawarma> _shawarmas;
        public ObservableCollection<Shawarma> Shawarmas
        {
            get => _shawarmas;
            set
            {
                _shawarmas = value;
                OnPropertyChanged();
            }
        }

        public ShawarmaViewModel()
        {
            Shawarmas = new ObservableCollection<Shawarma>();
        }

        public void AddShawarma(Shawarma shawarma)
        {
            Shawarmas.Add(shawarma);
        }

        public void UpdateShawarma(Shawarma shawarma)
        {
            var existingShawarma = Shawarmas.FirstOrDefault(s => s.Id == shawarma.Id);
            if (existingShawarma != null)
            {
                var index = Shawarmas.IndexOf(existingShawarma);
                Shawarmas[index] = shawarma;
            }
        }

        public void DeleteShawarma(Guid shawarmaId)
        {
            var shawarmaToRemove = Shawarmas.FirstOrDefault(s => s.Id == shawarmaId);
            if (shawarmaToRemove != null)
            {
                Shawarmas.Remove(shawarmaToRemove);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public OrderViewModel()
        {
            Orders = new ObservableCollection<Order>();
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = Orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                var index = Orders.IndexOf(existingOrder);
                Orders[index] = order;
            }
        }

        public void DeleteOrder(Guid orderId)
        {
            var orderToRemove = Orders.FirstOrDefault(o => o.Id == orderId);
            if (orderToRemove != null)
            {
                Orders.Remove(orderToRemove);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
