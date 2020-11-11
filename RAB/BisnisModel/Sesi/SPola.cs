using Newtonsoft.Json;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RAB.BisnisModel.Sesi
{
    public class SPola
    {
        private Pola _objPola { get; set; }
        private int _polaId { get; set; }
        private int _garisId { get; set; }
        private int _koorId { get; set; }
        private int _skala { get; set; }
        private bool _dariGambar { get; set; }

        HttpContext _contexIni;
        public const string SesiObjPola = "SesiObjPola";
        public const string SesiPolaId = "SesiPolaId";
        public const string SesiGarisId = "SesiGarisId";
        public const string SesiKoorId = "SesiKoorId";
        public const string SesiSkala = "SesiSkala";
        public const string SesiDariGambar = "SesiDariGambar";

        public static SPola GetSesi(HttpContext contex)
        {
            var objSPola = new SPola();
            objSPola._contexIni = contex;
            objSPola._objPola = objSPola.GetObjPola(contex);
            objSPola._polaId = objSPola.GetPolaId(contex);
            objSPola._koorId = objSPola.GetKoorId(contex);
            objSPola._garisId = objSPola.GetGarisId(contex);
            objSPola._skala = objSPola.GetSkala(contex);
            objSPola._dariGambar = objSPola.GetDariGambar(contex);

            return objSPola;
        }
        public static SPola GetSesi(Controller controller)
        {
            return GetSesi(controller.HttpContext);
        }

        public Pola Pola
        {
            get
            {
                return _objPola;
            }
        }
        public int PolaId
        {
            get
            {
                return _polaId;
            }
        }
        public int GarisId
        {
            get
            {
                return _garisId;
            }
        }
        public int KoorId
        {
            get
            {
                return _koorId;
            }
        }
        public int Skala
        {
            get
            {
                return _skala;
            }
        }
        public bool DariGambar
        {
            get
            {
                return _dariGambar;
            }
        }

        private Pola GetObjPola(HttpContext context)
        {
            if (context.Session.GetString(SesiObjPola) == null)
            {
                return new Pola();
            }
            return JsonConvert.DeserializeObject<Pola>(context.Session.GetString(SesiObjPola));
        }
        private int GetPolaId(HttpContext context)
        {
            if (context.Session.GetString(SesiPolaId) == null)
            {
                return 0;
            }
            return (int) context.Session.GetInt32(SesiPolaId);
        }
        private int GetKoorId(HttpContext context)
        {
            if (context.Session.GetString(SesiKoorId) == null)
            {
                return 0;
            }
            return (int)context.Session.GetInt32(SesiKoorId);
        }
        private int GetGarisId(HttpContext context)
        {
            if (context.Session.GetString(SesiGarisId) == null)
            {
                return 0;
            }
            return (int)context.Session.GetInt32(SesiGarisId);
        }
        private int GetSkala(HttpContext context)
        {
            if (context.Session.GetString(SesiSkala) == null)
            {
                return 100;
            }
            return (int)context.Session.GetInt32(SesiSkala);
        }
        private bool GetDariGambar(HttpContext context)
        {
            if (context.Session.GetString(SesiDariGambar) == null)
            {
                return false;
            }
            return context.Session.GetInt32(SesiDariGambar)>0;
        }

        public void SetObjPola(Pola pola)
        {
            _contexIni.Session.SetString(SesiObjPola, JsonConvert.SerializeObject(pola));
        }
        public void SetPolaId(int? polaId)
        {
            if (polaId == null) polaId = 0;
            _contexIni.Session.SetInt32(SesiPolaId,  (int) polaId);
        }
        public void SetKoorId(int? koorId)
        {
            if (koorId == null) koorId = 0;
            _contexIni.Session.SetInt32(SesiKoorId, (int)koorId);
        }
        public void SetGarisId(int? garisId)
        {
            if (garisId == null) garisId = 0;
            _contexIni.Session.SetInt32(SesiGarisId, (int)garisId);
        }
        public void SetSkala(int? skala)
        {
            if (skala == null) skala = 100;
            _contexIni.Session.SetInt32(SesiSkala, (int)skala);
        }
        public void SetDariGambar(bool dariGambar)
        {
            _contexIni.Session.SetInt32(SesiDariGambar, dariGambar? 1:0);
        }
    }
}
