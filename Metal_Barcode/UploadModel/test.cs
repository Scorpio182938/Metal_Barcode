using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadModel
{
    public class test
    {
        public void GetSS()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<UploadModel.Product>
                {
                    new Product {Name="Kaka",Price=275M },
                    new Product {Name="wangKa",Price=48.95M },
                    new Product {Name="wangKaka",Price=19.50M },
                    new Product {Name="KawangKa",Price=39.25M }
                }
            };
            //创建并填充ShoppingCart
            UploadModel.Product[] productArrary =
            {
              new UploadModel.Product {Name="Kaka",Price=375M },
              new UploadModel.Product {Name="wangKa",Price=48.95M },
              new UploadModel.Product {Name="wangKaka",Price=19.50M },
              new UploadModel.Product {Name="KawangKa",Price=34.95M }
            };
            

        }
        

        
    }
}
