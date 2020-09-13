using System;
using System.Collections.Generic;
using Laundryku.View.Pelanggan;
using Xamarin.Forms;

namespace Laundryku.View.Login
{
    public partial class _1Login : ContentPage
    {
        public _1Login()
        {
            InitializeComponent();
        }

        //button Login
        void LoginOnClicked(object sender, EventArgs args)
        {
            //cek no tlp web service true => go to _3Otp false => popup error mssg
            Navigation.PushModalAsync(new _1PesananBaru());
        }

        //button Daftar
        void DaftarOnClicked(object sender, EventArgs args)
        {
            Navigation.PushModalAsync(new _2Daftar());
        }
    }
    
}
