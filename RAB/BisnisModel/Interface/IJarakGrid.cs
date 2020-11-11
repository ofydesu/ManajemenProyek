using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Interface
{
    interface IJarakGrid
    {
        public int PolaId { get; set; }
        public int GridId { get; set; }
        public int JarakKum { get; set; }
    }
}
