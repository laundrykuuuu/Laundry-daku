using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using static Laundryku.Model.GlobalModel;

namespace Laundryku.View.Pelanggan
{
    public partial class _1PesananBaru : ContentPage
    {
        
        public _1PesananBaru()
        {
            InitializeComponent();
            ListPelanggan.ItemsSource = GetListPelanggan();
            ListPelanggan.RefreshCommand = new Command(() => {
                //Do your stuff.
                RefreshList();
                ListPelanggan.IsRefreshing = false;
            });
        }

        void RefreshList()
        {
            ListPelanggan.ItemsSource = GetListPelanggan();
        }

        public static ObservableCollection<GlobalList> GetListPelanggan()
        {
            ObservableCollection<GlobalList> List = new ObservableCollection<GlobalList>();
            List<GlobalList> ListPelangganLaundry = new List<GlobalList>();
            if (Application.Current.Properties.ContainsKey("LISTCACHEPELANGGAN"))
            {
                var ListCachePelanggan = Convert.ToString(Application.Current.Properties["LISTCACHEPELANGGAN"]);
                ListPelangganLaundry = JsonConvert.DeserializeObject<List<GlobalList>>(ListCachePelanggan);
                
            }
            else
            {
                ListPelangganLaundry = new List<GlobalList> {
                        new GlobalList { Nama = "Bambang Mulyadi", NoTlp="222", Alamat = "Depok"},
                        new GlobalList { Nama = "Dimas Kursiana", NoTlp = "223", Alamat = "Bekasi"},
                        new GlobalList { Nama = "Ali Mubarokah", NoTlp = "224", Alamat = "Depok"}};

                var strCachePelanggan = JsonConvert.SerializeObject(ListPelangganLaundry);
                Application.Current.Properties["LISTCACHEPELANGGAN"] = strCachePelanggan;
            }

            List = new ObservableCollection<GlobalList>(ListPelangganLaundry as List<GlobalList>);

            return List;
        }

        async void OnClickCreate(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new _2BuatDataPelanggan());
        }

        async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            GlobalList Customer = (GlobalList)e.Item;
            await Navigation.PushModalAsync(new _3Pesanan(Customer));
        }

    }
}
