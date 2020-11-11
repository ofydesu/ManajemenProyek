using RAB.Asset.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Interface
{
    interface IPola
    {
        public int PolaId { get; set; }
        public string Nama { get; set; }
        public  EStatusPola Status{ get; set; }
    }
}
