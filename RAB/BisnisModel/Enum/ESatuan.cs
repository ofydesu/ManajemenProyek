using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RAB.Asset.Enum
{
    public enum ESat1D
    {
        [Display(Name ="Meter")]
       M,
        [Display(Name = "Centimeter")]
        cm,
        [Display(Name = "Milimeter")]
        mm
    }
}
