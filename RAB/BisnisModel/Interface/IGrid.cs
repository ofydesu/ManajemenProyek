using RAB.Asset.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Interface
{
    interface IGrid
    {
        public int GridId { get; set; }
        public int PolaId { get; set; }
        public int SumbuId { get; set; }
        public ESumbu Sumbu { get; set; }
        public string Nama { get; set; }
        public int Jarak { get; set; }
        //public string StringId { get; }
    }
}
