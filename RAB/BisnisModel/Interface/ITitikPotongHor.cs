using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Interface
{
    interface ITitikPotongHor:IKoordinatIni
    {
        public int TtkPotId { get; set; }
        public bool TidakKeX { get; set; }
        public bool TidakKeY { get; set; }
        public bool MiringAtas { get; set; }
        public bool MiringBawah { get; set; }
        public bool KiriBawahSemu { get; set; }
    }
}
