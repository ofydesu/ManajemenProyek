using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RAB.Asset.Enum
{
    public enum ESumbu
    {
       X,
       Y,
       Z
    }

    public enum EPosisi3D
    {
        Denah,
        Balok,  //X2 sbg Y3, Y2 sbg Z3, dan X3 adalah offset inputan
        Kolom,   //X2 sbg X3, Y2 sbg Y3, dan Z3 adalah offset inputan
        Kusen,
    }
}
