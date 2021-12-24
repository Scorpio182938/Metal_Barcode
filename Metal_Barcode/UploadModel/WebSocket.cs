using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadModel
{
    public class WebSocket
    {

        public int NodeNo { get; set; }
        public string Function { get; set; }
        
        public Content[] Content { get; set; }
    }
}
