using System;
using System.Net.Http;
using Newtonsoft.Json;
using static Laundryku.Model.GlobalModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Collections.ObjectModel;
using System.Globalization;
using Laundryku.Model;

namespace Laundryku.ResApi
{
    public class ApiProduk
    {
        public class ConstantVariableProduk
        {
            public const string PRODUCTTYYPEKILOAN = "Kiloan";
            public const string PRODUCTTYYPESATUAN = "Satuan";
        }

        #region Data Product
        public static RootProduct getProducts()
        {
            RootProduct Model = new RootProduct();
            using (var client = new HttpClient())
            {
                try
                {
                    if (Application.Current.Properties.ContainsKey(ConstantVariable.CACHEALLPRODUCT))
                    {
                        var DataCache = Convert.ToString(Application.Current.Properties[ConstantVariable.CACHEALLPRODUCT]);
                        Model = JsonConvert.DeserializeObject<RootProduct>(DataCache);
                    }
                    else
                    {
                        var url = ConstantVariable.URLALLPRODUCT;
                        var result = client.GetAsync(url).Result;
                        var resultString = result.Content.ReadAsStringAsync().Result;
                        Application.Current.Properties[ConstantVariable.CACHEALLPRODUCT] = resultString;
                        Model = JsonConvert.DeserializeObject<RootProduct>(resultString);
                    }
                }
                catch (Exception ex)
                {
                    //cache exception
                }
            }

            return (Model);
        }
        #endregion
    }
}
