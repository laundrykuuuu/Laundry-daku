using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Laundryku.Model;
using Newtonsoft.Json;
using Xamarin.Forms;
using static Laundryku.Model.GlobalModel;
using Laundryku.ResApi;

namespace Laundryku.View.Pelanggan
{
    public partial class _3Pesanan : ContentPage
    {
        bool framekiloan = true;
        bool framesatuan = false;

        GlobalPesanan Pesanan = new GlobalPesanan();
        RootProduct modelProduk = new RootProduct();

        public _3Pesanan(GlobalList Customer)
        {
            InitializeComponent();
            modelProduk = ApiProduk.getProducts();

            Nama.Text = Customer.Nama;
            NoHp.Text = Customer.NoTlp;
            Alamat.Text = Customer.Alamat;
            Pesanan = new GlobalPesanan() {Nama= Customer.Nama, NoTlp = Customer.NoTlp, Alamat = Customer.Alamat};

            //default selected Layanan
            typeLayanan1.BackgroundColor = Color.FromHex("#FF8D14");
            typeLayanan1.TextColor = Color.White;
            Pesanan.tipeLayanan = typeLayanan1.CommandParameter.ToString();
            typeLayanan2.BackgroundColor = Color.White;
            typeLayanan3.BackgroundColor = Color.White;

            //default selected Kiloan
            service1.BackgroundColor = Color.FromHex("#FF8D14");
            service1.TextColor = Color.White;
            service2.BackgroundColor = Color.White;

            //button Lanjutkan
            btnLanjutkan.IsEnabled = false;
            btnLanjutkan.BackgroundColor = Color.Silver;
            btnLanjutkan.TextColor = Color.Black;

            //default
            #region kiloan
            //var kg = ApiProduk.ConstantVariableProduk.PRODUCTTYYPEKILOAN;
            var kiloanproductName= modelProduk.products.Where(w => w.productType == "Kiloan").Select(s=>s.productName).ToList();
            typeLayanan1.Text = kiloanproductName[0]; //cuci kering + seterika
            typeLayanan2.Text = kiloanproductName[1]; //cuci kering 
            typeLayanan3.Text = kiloanproductName[2]; //seterika
            #endregion

            #region satuan
            framepesanansatuan.IsVisible = false;
            durasisatuanexpressselected.Text = modelProduk.serviceTypes[0].description; //express
            durasisatuanregularselected.Text = modelProduk.serviceTypes[1].description; //regular
            #endregion

        }

        protected override void OnAppearing()
        {
            if (Application.Current.Properties.ContainsKey(ConstantVariable.CACHEPESANANKILOAN))
            {
                string strKiloan = Convert.ToString(Application.Current.Properties[ConstantVariable.CACHEPESANANKILOAN]);
                var ModelKiloan = JsonConvert.DeserializeObject<GlobalPesanan>(strKiloan);
                if (Pesanan.serviceKiloan == null)
                    Pesanan.serviceKiloan = new SerivceKiloan();

                if(!string.IsNullOrEmpty(ModelKiloan.serviceKiloan.durasiLayanan))
                    durasikiloanselected.Text = ModelKiloan.serviceKiloan.durasiLayanan;

                Pesanan.serviceKiloan.durasiLayanan = ModelKiloan.serviceKiloan.durasiLayanan;
                Pesanan.serviceKiloan.hargaPerKilo = ModelKiloan.serviceKiloan.hargaPerKilo;
                HitungTotalHarga();
            }
        }


        #region change kiloan
        async void onchengeService(object sender, EventArgs e)
        {
            try
            {
                service1.BackgroundColor = Color.White;
                service2.BackgroundColor = Color.White;
                service1.TextColor = Color.Black;
                service2.TextColor = Color.Black;

                var ServiceChange = (Button)sender;
                ServiceChange.BackgroundColor = Color.FromHex("#FF8D14");
                ServiceChange.TextColor = Color.White;

                if (ServiceChange.CommandParameter.ToString() == "Kiloan")
                {
                    //kiloan
                    framepesanankiloan1.IsVisible = true;
                    framepesanankiloan2.IsVisible = true;
                    framelabeltipelayanan.IsVisible = true;
                    framemenutipelayanan.IsVisible = true;
                    //satuan
                    framepesanansatuan.IsVisible = false;

                }
                else
                {
                    //kiloan
                    framepesanankiloan1.IsVisible = false;
                    framepesanankiloan2.IsVisible = false;
                    framelabeltipelayanan.IsVisible = false;
                    framemenutipelayanan.IsVisible = false;
                    //satuan
                    framepesanansatuan.IsVisible = true;
                }
                
            }
            catch (Exception)
            {
                
            }
        }
        #endregion

        #region durasi kiloan
        async void durasikiloan(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new _4Kiloan(Pesanan.tipeLayanan));
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region durasi satuan
        async void durasisatuan(object sender, EventArgs e)
        {
            try
            {
                var durationsatuanselected = (Button)sender;
                if (!string.IsNullOrEmpty(Pesanan.tipeLayanan))
                await Navigation.PushModalAsync(new _4Satuan(durationsatuanselected.Text));
            }
            catch (Exception)
            {

            }
        }
        #endregion

        async void ontextchangetotalharrgakiloan(object sender, EventArgs e)
        {
            try
            {
                double outcheck = 0;
                var beratKiloan = (Entry)sender;
                double.TryParse(beratKiloan.Text.ToString(),out outcheck);
                Pesanan.serviceKiloan.Berat = outcheck;
                HitungTotalHarga();
            }
            catch (Exception)
            {

            }
        }

        async void onClickTypeLayanan(object sender, EventArgs e)
        {
            try
            {
                typeLayanan1.BackgroundColor = Color.White;
                typeLayanan2.BackgroundColor = Color.White;
                typeLayanan3.BackgroundColor = Color.White;
                typeLayanan1.TextColor = Color.Black;
                typeLayanan2.TextColor = Color.Black;
                typeLayanan3.TextColor = Color.Black;

                var layananChange = (Button)sender;
                Pesanan.tipeLayanan = layananChange.Text;
                layananChange.BackgroundColor = Color.FromHex("#FF8D14");
                layananChange.TextColor = Color.White;

                //set harga kiloan
                if (framekiloan)
                {
                    var existDuration = Pesanan.serviceKiloan.durasiLayanan.Split(' ').ToList();
                    existDuration = existDuration.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    var productName = layananChange.Text;
                    var serviceType = existDuration[0];
                    var duration = int.Parse(existDuration[1]);
                    var durationType = existDuration[2];

                    modelProduk = ApiProduk.getProducts();
                    var serviceAmountSelected = modelProduk.productDetails.Where(w => w.productName == productName && w.serviceType == serviceType && w.duration == duration && w.durationType == durationType).FirstOrDefault();
                    Pesanan.serviceKiloan.hargaPerKilo = serviceAmountSelected.serviceAmount;
                }

                HitungTotalHarga();
            }
            catch (Exception)
            {

            }
        }

        void HitungTotalHarga()
        {
            btnLanjutkan.IsEnabled = false;
            btnLanjutkan.BackgroundColor = Color.Silver;
            btnLanjutkan.TextColor = Color.Black;

            totalHarga.Text = "";
            if(Pesanan.serviceKiloan.Berat != 0 && Pesanan.serviceKiloan.hargaPerKilo != null)
            {
                totalHarga.Text = "Rp "+(Pesanan.serviceKiloan.Berat * Pesanan.serviceKiloan.hargaPerKilo).ToString();
                btnLanjutkan.BackgroundColor = Color.FromHex("#FF8D14");
                btnLanjutkan.TextColor = Color.White;
                btnLanjutkan.IsEnabled = true;
            }

        }

    }
}
