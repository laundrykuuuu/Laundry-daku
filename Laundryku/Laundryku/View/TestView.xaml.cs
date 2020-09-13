using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Runtime.CompilerServices;


namespace Laundryku.View
{
    public partial class TestView : ContentPage
    {

        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(int),
              declaringType: typeof(TestView),
              defaultValue: 1,
              defaultBindingMode: BindingMode.TwoWay);

        public int? Max { get; set; }

        public int Increment { get; set; } = 1;

        public int Text
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color ButtonColor { get; set; } = Color.Black;

        public Color LabelColor { get; set; } = Color.Black;


        public TestView()
        {
            InitializeComponent();
            lblValue.SetBinding(Label.TextProperty, new Binding(nameof(Text), BindingMode.TwoWay, source: this));
            //lblValue.SetBinding(Label.TextProperty, new Binding(nameof(Text), BindingMode.TwoWay, source: this));
            //qty.Maximum = 20;
            //qty.Increment = 1;
        }

        double steperposition = 0;
        //async void stepper_ValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var step = (Stepper)sender;
        //        steperposition = step.StepperPosition != steperposition ? step.StepperPosition : steperposition;
        //        quantity.Text = "           " + steperposition.ToString();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        void btnPlus_Clicked(object sender, System.EventArgs e)
        {
            var id = (Button)sender;
            if (Max == null)
            {
                Text += Increment;
            }
            else if (Text < Max)
            {
                Text += Increment;
            }

        }

        void btnMinus_Clicked(object sender, System.EventArgs e)
        {
            var id = (Button)sender;
            if ((Text - Increment) > 1)
                Text -= Increment;
            else
                Text = 0;
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == TextProperty.PropertyName)
            {
                lblValue.Text = Text.ToString();
            }
        }
    }
}
