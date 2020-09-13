using System;

using Xamarin.Forms;

namespace Laundryku.ViewModel
{
    public class PesananViewModel : ContentPage
    {
        public PesananViewModel()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

