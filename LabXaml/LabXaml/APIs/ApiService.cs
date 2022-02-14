using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LabXaml.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace LabXaml.APIs
{
    class ApiService
    {
        HttpClient client;

        public ApiService()
        {
            client = new HttpClient();
        }

        public async Task<ObservableCollection<Product>> GetProducts()
        {
            ObservableCollection<Product> Items = null;

            try
            {
                var respones = await client.GetAsync("http://10.0.2.2:50435/api/Products/");
                if (respones.IsSuccessStatusCode)
                {
                    var content = await respones.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<ObservableCollection<Product>>(content);
                    return Items;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return null;
        }

        public async Task<bool> Register(Register Item)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Item);
                StringContent sContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://10.0.2.2:55215/api/Account/Register", sContent);
                Console.WriteLine("Result" + response);
                Console.WriteLine("Content" + sContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<bool> Login(Login login)
        {
            User Items = null;
            try
            {
                var dict = new Dictionary<string, string>();
                dict.Add("Content-Type", "application/x-www-form-urlencode");
                dict.Add("grant_type", "password");
                dict.Add("username", login.Email);
                dict.Add("password", login.Password);

                var response = await client.PostAsync("http://10.0.2.2:55215/token", new FormUrlEncodedContent(dict));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<User>(content);
                    Preferences.Set("username", Items.userName);
                    Preferences.Set("token_type", Items.token_type);
                    Preferences.Set("access_token", Items.access_token);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return false;
        }

        public async Task<bool> AddOrder(Order Item)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Item);
                StringContent sContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://10.0.2.2:50435/api/Orders", sContent);
                Console.WriteLine("Result" + response);
                Console.WriteLine("Content" + sContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ObservableCollection<Order>> GetUserOrders()
        {
            ObservableCollection<Order> Items = null;
            var username = Preferences.Get("username","");
            try
            {
                var respones = await client.GetAsync("http://10.0.2.2:50435/api/Orders?username=" + username);
                if (respones.IsSuccessStatusCode)
                {
                    var content = await respones.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<ObservableCollection<Order>>(content);
                    return Items;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return null;
        }

        public async Task<bool> Logout()
        {
            try
            {
                var access_token = Preferences.Get("access_token", "");
                var token_type = Preferences.Get("token_type","");
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token_type + " " + access_token);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

                var response = await client.PostAsync("http://10.0.2.2:55215/api/Account/Logout", null);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
