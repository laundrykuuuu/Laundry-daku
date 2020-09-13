using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using Newtonsoft.Json;
using static Laundryku.Model.GlobalModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Laundryku.ResApi;
using Laundryku.Model;

namespace Laundryku.View.Pelanggan
{
    public partial class _4Satuan : ContentPage
    {
        #region steper
        ObservableCollection<GlobalList> DefaultListProductSatuan = new ObservableCollection<GlobalList>();
        GlobalPesanan Pesanan = new GlobalPesanan();
        public int? Max { get; set; }
        public int Increment { get; set; } = 1;
        public double TotalBayar = 0;
        #endregion

        public _4Satuan(string Layanan)
        {
            InitializeComponent();
            DefaultListProductSatuan = GetListProductSatuan(Layanan);
            ListPelanggan.ItemsSource = DefaultListProductSatuan;


        }

        void btnPlus_Clicked(object sender, System.EventArgs e)
        {
            var ButtonPlus = (Button)sender;
            Guid idLastSelected = Guid.Parse(ButtonPlus.CommandParameter.ToString());
            List<GlobalList> myList = new List<GlobalList>(DefaultListProductSatuan);
            var selectedProduct = myList.Where(w => w.IdProductSatuan == idLastSelected).FirstOrDefault();

            selectedProduct.isDisabledButtonMinus = true;
            if (Max == null)
            {
                selectedProduct.BanyaknyaProdukSatuan += Increment;
            }
            else if (selectedProduct.BanyaknyaProdukSatuan < Max)
            {
                selectedProduct.BanyaknyaProdukSatuan += Increment;
            }

            ListPelanggan.ItemsSource = null;
            SetCache(selectedProduct);
            hitung(myList);
            ListPelanggan.ItemsSource = DefaultListProductSatuan;

        }

        void btnMinus_Clicked(object sender, System.EventArgs e)
        {

            var ButtonPlus = (Button)sender;
            Guid idLastSelected = Guid.Parse(ButtonPlus.CommandParameter.ToString());
            List<GlobalList> myList = new List<GlobalList>(DefaultListProductSatuan);
            var selectedProduct = myList.Where(w => w.IdProductSatuan == idLastSelected).FirstOrDefault();

            if ((selectedProduct.BanyaknyaProdukSatuan - Increment) >= 1)
            {
                selectedProduct.BanyaknyaProdukSatuan -= Increment;
                selectedProduct.isDisabledButtonMinus = true;
            }
            else
            {
                selectedProduct.BanyaknyaProdukSatuan = 0;
                selectedProduct.isDisabledButtonMinus = false;
            }
            ListPelanggan.ItemsSource = null;
            SetCache(selectedProduct);
            hitung(myList);
            ListPelanggan.ItemsSource = DefaultListProductSatuan;
        }

        void SetCache(GlobalList modelselectedchange)
        {

            GlobalPesanan modelSatuan = new GlobalPesanan();
            if (Application.Current.Properties.ContainsKey(ConstantVariable.CACHEPESANANSATUAN))
            {

                string strSatuan = Convert.ToString(Application.Current.Properties[ConstantVariable.CACHEPESANANSATUAN]);
                modelSatuan = JsonConvert.DeserializeObject<GlobalPesanan>(strSatuan);
                
                var chkmodel = modelSatuan.serviceSatuan.ListPesananSatuan.Where(w => w.idProduct == modelselectedchange.IdProductSatuan).FirstOrDefault();

                if(chkmodel == null)
                {
                    ListPesananSatuan model = new ListPesananSatuan();
                    model.idProduct = modelselectedchange.IdProductSatuan;
                    model.Quantity = modelselectedchange.BanyaknyaProdukSatuan;
                    model.hargaPerSatuan = Regex.Replace(modelselectedchange.HargaProdukSatuan, @"\D", "");
                    modelSatuan.serviceSatuan.ListPesananSatuan.Add(model);
                }
                else
                {
                    modelSatuan.serviceSatuan.ListPesananSatuan.Where(w => w.idProduct == modelselectedchange.IdProductSatuan).FirstOrDefault().Quantity = modelselectedchange.BanyaknyaProdukSatuan;
                }

            }
            else
            {
                modelSatuan.serviceSatuan = new SerivceSatuan();
                modelSatuan.serviceSatuan.ListPesananSatuan = new List<ListPesananSatuan>();
                ListPesananSatuan model = new ListPesananSatuan();
                model.idProduct = modelselectedchange.IdProductSatuan;
                model.Quantity = modelselectedchange.BanyaknyaProdukSatuan;
                model.hargaPerSatuan = Regex.Replace(modelselectedchange.HargaProdukSatuan, @"\D", "");
                modelSatuan.serviceSatuan.ListPesananSatuan.Add(model);
            }

            var cacheSatuanSet = JsonConvert.SerializeObject(modelSatuan);
            Application.Current.Properties[ConstantVariable.CACHEPESANANSATUAN] = cacheSatuanSet;
        }

        void hitung(List<GlobalList> listProduct)
        {
            double totalSelectedsatuan = 0;
            foreach(var item in listProduct)
            {
                var sumprroductsatuan = item.BanyaknyaProdukSatuan > 0 ? double.Parse(Regex.Replace(item.HargaProdukSatuan, @"\D", "")) * item.BanyaknyaProdukSatuan : 0;
                totalSelectedsatuan = totalSelectedsatuan + sumprroductsatuan;
            }

            total.Text = (TotalBayar + totalSelectedsatuan).ToString();
        }

        #region ListData
        public static ObservableCollection<GlobalList> GetListProductSatuan(string Layanan)
        {
            ObservableCollection<GlobalList> List = new ObservableCollection<GlobalList>();
            List<GlobalList> ListProductSatuan = new List<GlobalList>();

            RootProduct modelProduk = new RootProduct();
            modelProduk = ApiProduk.getProducts();
            var query = modelProduk.productDetails.Where(w => w.productType == "Satuan" && w.serviceType == Layanan);

            foreach(var item in query)
            {
                DateTime now = DateTime.Now;

                var EstimateDates = item.durationType == "Days" ? "Finished date : "+ now.AddDays(item.duration).ToString("dd/MM/yyy") : "Finished on : " + now.AddHours(item.duration).ToString("hh:mm");

                GlobalList data = new GlobalList()
                {
                    isDisabledButtonMinus = false,
                    BanyaknyaProdukSatuan = 0,
                    IdProductSatuan = Guid.Parse(item.id),
                    ImageProductSatuan = "icon.png",
                    NamaProdukSatuan = item.productName,
                    HargaProdukSatuan = "Rp " + item.serviceAmount + "/pcs",
                    DurasiProdukSatuan = item.duration + " " + item.durationType,
                    EstimateDateProdukSatuan = EstimateDates

                };
                ListProductSatuan.Add(data);
            }
            //var strCachePelanggan = JsonConvert.SerializeObject(ListProductSatuan);
            //Application.Current.Properties[ConstantVariable.CACHEPESANANSATUAN] = strCachePelanggan;
            List = new ObservableCollection<GlobalList>(ListProductSatuan as List<GlobalList>);
            return List;
        }
        #endregion
    }
}
