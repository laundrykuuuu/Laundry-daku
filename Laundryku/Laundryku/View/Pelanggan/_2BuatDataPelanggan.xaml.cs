using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using static Laundryku.Model.GlobalModel;

namespace Laundryku.View.Pelanggan
{
    public partial class _2BuatDataPelanggan : ContentPage
    {
        public _2BuatDataPelanggan()
        {
            InitializeComponent();
            
        }

        bool flagsimpanddata = false;
        async void FlagSimpanData(object sender, EventArgs e)
        {
            if (!flagsimpanddata)
                flagsimpanddata = true;
            else
                flagsimpanddata = false;
        }
        async void SimpanDataPelanggan(object sender, EventArgs e)
        {
            GlobalList Customer = new GlobalList();
            Customer.Nama = ENama.Text;
            Customer.NoTlp = ENomorHp.Text;
            Customer.Alamat = EAlamat.Text;

            if (flagsimpanddata)
                if (Application.Current.Properties.ContainsKey("LISTCACHEPELANGGAN"))
                {
                    var ListCachePelanggan = Convert.ToString(Application.Current.Properties["LISTCACHEPELANGGAN"]);
                    var ListModel = JsonConvert.DeserializeObject<List<GlobalList>>(ListCachePelanggan);
                    ListModel.Add(Customer);

                    var strCachePelanggan = JsonConvert.SerializeObject(ListModel);
                    Application.Current.Properties["LISTCACHEPELANGGAN"] = strCachePelanggan;
                }

            await Navigation.PushModalAsync(new _3Pesanan(Customer));
        }
    }
}
