using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.Devices
{
    public interface IScan
    {
        bool GetBarcodeL(out string scanData);
        bool GetBarcodeR(out string scanData);
    }
}
