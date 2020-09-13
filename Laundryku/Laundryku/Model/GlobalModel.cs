using System;
using System.Collections.Generic;
using System.Text;

namespace Laundryku.Model
{
    public class GlobalModel
    {
        public class GlobalList
        {
            //pelanggan
            public string Nama { get; set; }
            public string NoTlp { get; set; }
            public string Alamat { get; set; }

            //ProductSatuan
            public string durasiProdukKiloan { get; set; }
            public string hargaProdukKiloan { get; set; }

            //ProductSatuan
            public Guid IdProductSatuan { get; set; }
            public string ImageProductSatuan { get; set; }
            public string NamaProdukSatuan { get; set; }
            public string HargaProdukSatuan { get; set; }
            public string DurasiProdukSatuan { get; set; }
            public string EstimateDateProdukSatuan { get; set; }
            public int BanyaknyaProdukSatuan { get; set; }
            public bool isDisabledButtonMinus { get; set; }
            
        }


        public class GlobalPesanan
        {
            public Guid id { get; set; }
            public string Nama { get; set; }
            public string NoTlp { get; set; }
            public string Alamat { get; set; }
            public string tipeLayanan { get; set; }
            public SerivceKiloan serviceKiloan { get; set;}
            public SerivceSatuan serviceSatuan { get; set; }
            public double TotalHarga { get; set; }
        }
        //kiloan
        public class SerivceKiloan
        {
            public Guid id { get; set; }
            public string tipeLayanan { get; set; } //cuci&strika, cuci kerring, strika
            public string durasiLayanan { get; set; } //express, reguler
            public string Catatan { get; set; }
            public double Berat { get; set; }//total berat kiloan
            public double hargaPerKilo { get; set; }
            public double totalHargaKiloan { get; set; }//total harga kiloan
        }
        //satuan
        public class SerivceSatuan
        {
            public Guid id { get; set; }
            public string tipeLayanan { get; set; } //cuci&strika, cuci kerring, strika
            public List<ListPesananSatuan> ListPesananSatuan { get; set; }
            public double totalHargaSatuan { get; set; }//total harga kiloan
        }
        public class ListPesananSatuan
        {
            public Guid id { get; set; }
            public Guid idProduct { get; set; }
            public string hargaPerSatuan { get; set; }
            public double Quantity { get; set; }
            public string Catatan { get; set; }
        }
        public class SerivceSatuanRegular
        {
            public Guid id { get; set; }
            public Guid idProduct { get; set; }
            public string hargaPerSatuan { get; set; }
            public double Quantity { get; set; }
            public string Catatan { get; set; }
        }


        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class ProductType
        {
            public int id { get; set; }
            public string description { get; set; }
            public string type { get; set; }
        }

        public class ServiceType
        {
            public int id { get; set; }
            public string description { get; set; }
            public string type { get; set; }
        }

        public class Product
        {
            public string id { get; set; }
            public string productName { get; set; }
            public object productImage { get; set; }
            public string productType { get; set; }
            public string productUnit { get; set; }
        }

        public class ProductDetail
        {
            public string id { get; set; }
            public string productName { get; set; }
            public object productImage { get; set; }
            public string productType { get; set; }
            public string productUnit { get; set; }
            public string serviceType { get; set; }
            public int duration { get; set; }
            public string durationType { get; set; }
            public double serviceAmount { get; set; }
        }

        public class RootProduct
        {
            public List<ProductType> productTypes { get; set; }
            public List<ServiceType> serviceTypes { get; set; }
            public List<Product> products { get; set; }
            public List<ProductDetail> productDetails { get; set; }
        }

    }
}
