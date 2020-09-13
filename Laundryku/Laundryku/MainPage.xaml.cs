using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundryku.View;
using Laundryku.View.Login;
using Xamarin.Forms;

namespace Laundryku
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Navigation.PushModalAsync(new _1Login());
            //Navigation.PushModalAsync(new TestView());
        }
    }
}
