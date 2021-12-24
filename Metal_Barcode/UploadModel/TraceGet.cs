using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadModel
{
    public class TraceGet
    {
        public bool pass { get; set; }
        public string[] choice_ids { get; set; }

        public TraceGetDetail[] processes { get; set; }
    }
}
