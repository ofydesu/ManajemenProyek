﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAB.Asset.Enum;
using RAB.Asset.Obyek;
using RAB.Asset.OlahanModel;
using RAB.BisnisModel.Sesi;
using RAB.Data;
using RAB.Models.Utama;
using RAB.Models.View;

namespace RAB.Controllers
{
    public class GambarController : Controller
    {
        private readonly RabContext _context;

        public GambarController(RabContext context)
        {
            _context = context;
        }

        public IActionResult Tabel(int? id)
        {
            var qGaris = new List<OGaris>().AsQueryable();

            if (id != null)
            {
                try
                {
                    QCekBidangDatar qDatar = new QCekBidangDatar(_context, (int)id);
                    qGaris = qDatar.QryGarisPartial.OrderBy(g => g.Arah);   //.ToList();
                }
                catch
                {
                    // returan false
                }
            }

            ViewBag.ListPola = new SelectList(_context.TblPola, "PolaId", "Nama", id);
            ViewBag.PolaId = id;
            // iQueryable tdk bisa menggunkan toListAsync()
            // jadi untuk sementara pakai toList() aja
            return View(qGaris.ToList());
        }

        private class OSkala
        {
            public int Nilai { get; set; }
            public string Nama { get; set; }
        }

        private List<OSkala> LstSkala
        {
            get
            {
                List<OSkala> lstSkala = new List<OSkala>();
                lstSkala.Add(new OSkala() { Nilai = 400, Nama = "400 %" });
                lstSkala.Add(new OSkala() { Nilai = 300, Nama = "300 %" });
                lstSkala.Add(new OSkala() { Nilai = 200, Nama = "200 %" });
                lstSkala.Add(new OSkala() { Nilai = 150, Nama = "150 %" });
                lstSkala.Add(new OSkala() { Nilai = 100, Nama = "100 %" });
                lstSkala.Add(new OSkala() { Nilai = 75, Nama = "75 %" });
                lstSkala.Add(new OSkala() { Nilai = 50, Nama = "50 %" });
                lstSkala.Add(new OSkala() { Nilai = 25, Nama = "25 %" });
                lstSkala.Add(new OSkala() { Nilai = 10, Nama = "10 %" });
                return lstSkala;
            }
        }
        public IActionResult Index(int? id, int skl = 100)
        {
            var sesPola = SPola.GetSesi(this);
            if (id != null)
                sesPola.SetPolaId(id);
            else
                id = sesPola.PolaId;

            //if (!isAjax)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            decimal skala = (decimal)skl;
            if (skala < 10) { skala = 100; }
            var MVGambarGaris = new MVGambarGaris()
            {
                Skala = skala / 100,
                PolaId = id
            };

            if (id != null & id != 0)
            {
                //perbaharui tabel koordinat
                try
                {
                    QCekKoordinat cekKoord = new QCekKoordinat(_context, (int)id);
                    //perbaharui tabel titik potong
                    cekKoord.UpdateDariTblTitik();
                }
                catch { }

                var qGaris = new List<OGaris>().AsQueryable();
                var qGrsKoor = new List<OKetGarisKoordinat>().AsQueryable();
                var qTtkKoor = new List<Koordinat>().AsQueryable();
                try
                {
                    QCekBidangDatar qDatar = new QCekBidangDatar(_context, (int)id);
                    qGaris = qDatar.QryGarisPartial.OrderBy(g => g.Arah);

                    QCekKoordinat qKoor = new QCekKoordinat(_context, (int)id);
                    qGrsKoor = qKoor.GarisKoordinat;
                    qTtkKoor = qKoor.QryTblIniPosUpToDate;

                    MVGambarGaris.TinggiMax = qGrsKoor.Where(g => g.Garis.Arah == ESumbu.Y)
                                .Select(s => s.Garis.Akhir.TitikY.PosAbs).Max() + 300;
                    MVGambarGaris.LebarMax = qGrsKoor.Where(g => g.Garis.Arah == ESumbu.X)
                                .Select(s => s.Garis.Akhir.TitikX.PosAbs).Max() + 300;
                    //buat garis koordinat

                }
                catch (Exception) { }

                MVGambarGaris.LstGarisReal = qGaris.ToList();
                MVGambarGaris.LstGrsKoord = qGrsKoor.ToList();
                MVGambarGaris.LstKoord = qTtkKoor.ToList();
            }

            ViewBag.Skala = skala / 100;
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("RAB/_ViewGambarGaris", MVGambarGaris);
            }


            ViewBag.ListSkala = new SelectList(LstSkala, "Nilai", "Nama", skala);
            ViewBag.ListPola = new SelectList(_context.TblPola, "PolaId", "Nama", id);
            ViewBag.PolaId = id;
            // iQueryable tdk bisa menggunkan toListAsync()
            // jadi untuk sementara pakai toList() aja
            return View(MVGambarGaris);
        }

        public IActionResult KompGaris(int? polaId, int? garisId, int? skl, bool inzet = false)
        {
            var sesPola = SPola.GetSesi(this);
            int _polaId = 0;
            if (polaId == null || polaId == 0)
            {
                _polaId = sesPola.PolaId;
            }
            else
            {
                _polaId = (int)polaId;
            }

            if(garisId != null)
            {
                // simpan ke sessi
                sesPola.SetGarisId(garisId);
            }
            else { 
                garisId = sesPola.GarisId; 
            }
            if (skl != null)
            {
                // simpan ke sessi
                sesPola.SetSkala(skl);
            }
            else
            {
                skl = sesPola.Skala;
            }
            //tidak ada pilihan pola

            decimal skala = (decimal)skl;
            var polaNama = "";

            //int polId = ;

            var mvKompGaris = new MVGambarKompGaris();
            var mvGambarGarisParsial = new MVGambarGarisParsial()
            {
                Skala = skala / 100,
                PolaId = _polaId
            };
            var mvGambarGarisValid = new MVGambarGarisValid()
            {
                Skala = skala / 100,
                PolaId = _polaId
            };

            if (_polaId != 0)
            {

                #region garis valid
                polaNama = _context.TblPola.Find(_polaId).Nama;

                var qGaris = new List<Garis>().AsQueryable();
                var qGrsKoor = new List<OKetGarisKoordinat>().AsQueryable();
                var qTtkKoor = new List<Koordinat>().AsQueryable();
                try
                {
                    QCekGaris qcGaris = new QCekGaris(_context, _polaId);
                    qGaris = qcGaris.QryTblIniPosUpToDate.OrderBy(g => g.Arah);

                    // memperbaharui tabel garis di pola terpilih
                    QCekGaris qGrs = new QCekGaris(_context, _polaId);
                    qGrs.UpdateDariGambar();

                    QCekKoordinat qKoor = new QCekKoordinat(_context, _polaId);
                    qGrsKoor = qKoor.GarisKoordinat;
                    qTtkKoor = qKoor.QryTblIniPosUpToDate;

                    mvGambarGarisValid.TinggiMax = qGrsKoor.Where(g => g.Garis.Arah == ESumbu.Y)
                                .Select(s => s.Garis.Akhir.TitikY.PosAbs).Max() + 300;
                    mvGambarGarisValid.LebarMax = qGrsKoor.Where(g => g.Garis.Arah == ESumbu.X)
                                .Select(s => s.Garis.Akhir.TitikX.PosAbs).Max() + 300;
                    //buat garis koordinat
                }
                catch (Exception) { }
                mvGambarGarisValid.LstGarisReal = qGaris.ToList();
                mvGambarGarisValid.LstGrsKoord = qGrsKoor.ToList();
                mvGambarGarisValid.LstKoord = qTtkKoor.ToList();
                #endregion
                #region garis parsial
                var garis = new Garis();
                try
                {
                    // for test
                    //garisId = 1;
                    QCekGaris qcGaris = new QCekGaris(_context, _polaId);
                    garis = qcGaris.QryTblIniPosUpToDate.Single(g => g.GarisId == (int)garisId);  //OrderBy(g => g.Arah);

                    QCekKoordinat qKoor = new QCekKoordinat(_context, _polaId);
                    qGrsKoor = qKoor.GarisKoordinat4Z;
                    qTtkKoor = qKoor.QryTblIni4ZPosUpToDate;

                    mvGambarGarisParsial.TinggiMax = qGrsKoor.Where(g => g.Garis.Arah == ESumbu.Y)
                                .Select(s => s.Garis.Akhir.TitikY.PosRel).Max() + 300;

                    //mvGambarGarisParsial.LebarMax = qGrsKoor.Where(g => g.Garis.Arah == ESumbu.X)
                    //            .Select(s => s.Garis.Akhir.TitikY.PosAbs).Max() + 500;
                    //buat garis koordinat

                }
                catch (Exception) { }
                if (garisId != 0)
                {
                    mvGambarGarisParsial.GarisReal = garis;
                }
                mvGambarGarisParsial.LstGrsKoord = qGrsKoor.ToList();
                mvGambarGarisParsial.LstKoord = qTtkKoor.ToList();

                #endregion
                mvKompGaris.GarisValid = mvGambarGarisValid;
                mvKompGaris.GarisParsial = mvGambarGarisParsial;
            }

            ViewBag.Skala = skala / 100;
            ViewBag.GarisTerpilih = garisId;
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                if (inzet)
                {
                    return PartialView("RAB/_ViewGambarKompGaris", mvGambarGarisValid);
                }
                else
                {
                    return PartialView("RAB/_ViewGambarGarisParsial", mvGambarGarisParsial);
                }
            }

            //ViewBag.PolaNama = polaNama;
            ViewBag.ListSkala = new SelectList(LstSkala, "Nilai", "Nama", skala);

            return View(mvKompGaris);
        }

    }
}
