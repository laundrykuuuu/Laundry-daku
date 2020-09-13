using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Newtonsoft.Json;
using static Laundryku.Model.GlobalModel;
using Laundryku.Model;
using Laundryku.ResApi;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Laundryku.View.Pelanggan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class _4Kiloan : ContentPage
    {
        string Layanan = "";
        public _4Kiloan(string tipeLayanan)
        {
            InitializeComponent();
            MyListView.ItemsSource = ListDurationKiloan(tipeLayanan);
        }

        #region 
        public static ObservableCollection<GlobalList> ListDurationKiloan(string Layanan)
        {
            ObservableCollection<GlobalList> DataList = new ObservableCollection<GlobalList>();
            try
            {
                List<GlobalList> Items = new List<GlobalList>();
                RootProduct modelProduk = ApiProduk.getProducts();
                var query = from data in modelProduk.productDetails.Where(w => w.productName == Layanan).ToList()
                            select new GlobalList() { durasiProdukKiloan = data.serviceType +"  "+data.duration +" "+ data.durationType, hargaProdukKiloan = data.serviceAmount+"/kg" };

                Items.AddRange(query);
                DataList = new ObservableCollection<GlobalList>(Items as List<GlobalList>);
            }
            catch (Exception ex)
            {
               
            }

            return (DataList);
        }
        #endregion

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            try
            {
                GlobalPesanan Model = new GlobalPesanan();
                Model.serviceKiloan = new SerivceKiloan();
                GlobalList Data = (GlobalList)e.Item;
                Model.serviceKiloan.id = Guid.NewGuid();
                Model.serviceKiloan.durasiLayanan = Data.durasiProdukKiloan;
                string harrgaperkg = Regex.Match(Data.hargaProdukKiloan, @"\d+").Value;
                Model.serviceKiloan.hargaPerKilo = double.Parse(harrgaperkg);
                var cacheKiloanSet = JsonConvert.SerializeObject(Model);
                Application.Current.Properties[ConstantVariable.CACHEPESANANKILOAN] = cacheKiloanSet;

                await Navigation.PopModalAsync();
            }
            catch (Exception)
            {

            }

        }
    }
}
