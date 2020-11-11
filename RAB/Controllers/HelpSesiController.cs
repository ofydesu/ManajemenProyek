using Microsoft.AspNetCore.Mvc;
using RAB.BisnisModel.Sesi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAB.Controllers
{
    public class HelpSesiController : Controller
    {
        public void SetSesGaris(int? grsId)
        {
            if (grsId == null) grsId = -1;
            var sesPola = SPola.GetSesi(this);
            sesPola.SetGarisId(grsId);
        }

        public void SetSesPolaId(int? polaId)
        {
            if (polaId == null) polaId = -1;
            var sesPola = SPola.GetSesi(this);
            sesPola.SetPolaId(polaId);
        }

    }
}
