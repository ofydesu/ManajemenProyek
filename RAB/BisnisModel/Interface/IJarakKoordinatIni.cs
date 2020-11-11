using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Interface
{
    interface IJarakKoordinatIni : IKoordinatIni
    {
        public int JarakX { get; set; }
        public int JarakY { get; set; }
        public int JarakKumX { get; set; }
        public int JarakKumY { get; set; }
    }
}
