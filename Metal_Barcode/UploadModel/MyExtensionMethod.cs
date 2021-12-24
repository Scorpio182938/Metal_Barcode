using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadModel
{
    public static class MyExtensionMethod
    {
        public static decimal TotalPrice(this ShoppingCart products)
        {
            decimal total = 0;
            foreach (Product pro in products.Products)
            {
                total += pro.Price;
            }
            return total;
        }

        public static decimal ITotalPrices(this IEnumerable<Product> ProductEnum)
        {
            decimal total = 0;
            foreach (Product pro in ProductEnum)
            {
                total += pro.Price;
            }
            return total;
        }
    }
}
