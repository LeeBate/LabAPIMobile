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
    class ProductListViewModel : INotifyPropertyChanged
    {
         public event PropertyChangedEventHandler PropertyChanged;
        public  ObservableCollection<Product> MyListProduct
        {
            get
            {
               return  myproducts;
            }
            set
            {
                myproducts = value;
                var args = new PropertyChangedEventArgs(nameof(MyListProduct));
                PropertyChanged?.Invoke(this, args);
            }
        }
        ObservableCollection<Product> myproducts;
        public Command SelectCommand { get; set; }

        public Command AddCommand { get; set; }

        public Command BackCommand { get; }
        public Product SelectedProduct { get; set; }
        

        ApiService apiService;

       

        public ProductListViewModel()
        {
            
            MyListProduct = new ObservableCollection<Product>();
            apiService = new ApiService();

            GetProduct();

            SelectCommand = new Command(async() =>

            {
                var ttt = new { SelectedProduct, BackCommand = BackCommand ,AddCommand = AddCommand};
                var ProdDetail = new ProductDetailsPage
                {
                    BindingContext = new {SelectedProduct, BackCommand = BackCommand, AddCommand = AddCommand}
                };
                await Application.Current.MainPage.Navigation.PushModalAsync(ProdDetail);
            });

            BackCommand = new Command(async () => 
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });

            AddCommand = new Command(async () =>
            {
                Order Order = new Order();
                Order.Title = SelectedProduct.Name;
                Order.ProdID = SelectedProduct.ID;
                Order.Price = SelectedProduct.Price;
                Order.Username = Preferences.Get("username", "");
                var response = await apiService.AddOrder(Order);
                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Order", "Product added", "OK");
                }
            });

        }

        async void GetProduct()
        {
            MyListProduct = await apiService.GetProducts();

        }

    }

}