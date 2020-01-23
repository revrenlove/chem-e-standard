using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC.AgGateway.Integration.Models
{
    public class PALRequestLineItem
    {
        public uint LineNumber { get; set; }
        public string GTIN { get; set; }
        public string CropCode { get; set; }
        public int QTY { get; set; }
        public string PUOM { get; set; }
    }
}
