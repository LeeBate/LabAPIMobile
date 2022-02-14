using LabXaml.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using LabXaml.APIs;
using System.ComponentModel;
using Xamarin.Essentials;

namespace LabXaml.ViewModels
{
    class CartViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ApiService apiService;

        public Command LogoutCommand { get; }

        public ObservableCollection<Order> Orders
        {
            get
            {
                return myorders;
            }
            set
            {
                myorders = value;
                var args = new PropertyChangedEventArgs(nameof(Orders));
                PropertyChanged?.Invoke(this, args);
            }
        }
        ObservableCollection<Order> myorders;

        public CartViewModel()
        {
            apiService = new ApiService();
            Orders = new ObservableCollection<Order>();

            GetCart();

            LogoutCommand = new Command(async () =>
            {
                var response = await apiService.Logout();
                if (response)
                {
                    Application.Current.MainPage = new NavigationPage( new LoginPage());
                }
            });
        }
        async void GetCart()
        {
            Orders = await apiService.GetUserOrders();
        }
    }
}
