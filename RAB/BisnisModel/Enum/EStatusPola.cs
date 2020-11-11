using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Asset.Enum
{
    public enum EStatusPola
    {
        Utama,
        Komponen,
        [Display (Name ="Sub Komponen")]
        SubKomponen
    }
}
