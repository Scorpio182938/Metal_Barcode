using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadModel
{
    public class Product
    {
        //字段
        private string name;

        //属性
        //自动实现属性{ get; set; }
        public int ProductID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        //实现自定义get,若需要set,则同时实现set;不可以自动实现set
        public string Name { get { return ProductID + name; } set { name = value; } }

    }
}
