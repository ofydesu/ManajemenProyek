using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAB.Asset.OlahanModel;
using RAB.Data;
using RAB.Models.Utama;
using RAB.Asset.Obyek;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using RAB.BisnisModel.Sesi;

namespace RAB.Controllers
{
    public class KoordinatController : Controller
    {
        private readonly RabContext _context;

        public KoordinatController(RabContext context)
        {
            _context = context;
        }

        // GET: TitikPotongHor
        public async Task<IActionResult> Index(int? id)
        {
            var sesPola = SPola.GetSesi(this);
            if (id != null)
                sesPola.SetPolaId(id);
            else
                id = sesPola.PolaId;

            if (id != 0)
            {
                try{
                    QCekKoordinat cekKoord = new QCekKoordinat(_context, (int)id);
                    //perbaharui tabel titik potong
                    cekKoord.UpdateDariTblTitik();
                }
                catch { }
            }
            var rabContext = await _context.TblKoordinat.Where(t => t.TitikX.PolaId == id).OrderBy(t => t.Yid).ThenBy(y => y.Xid).ToListAsync();


            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                return PartialView("RAB/_TblKoordinat", rabContext);
            }


            ViewBag.ListPola = new SelectList(_context.TblPola, "PolaId", "Nama", id);
            ViewBag.PolaId = id;

            return View(rabContext);
        }


        // GET: TitikPotongHor/Edit/5
        public async Task<IActionResult> Edit(int? id, int polaId, bool dariGambar, decimal skala=100)
        {
            if (id == null)
            {
                return NotFound();
            }

            var koordinat = await _context.TblKoordinat.FindAsync(id);
            if (koordinat == null)
            {
                return NotFound();
            }
            var koordTambahan = new OKoordTambahan()
            {
                IsiPolaId = polaId,
                KoordId = koordinat.KoordId,
                Xid = koordinat.Xid,
                Yid = koordinat.Yid,
                PubNama = _context.TblTitik.Find(koordinat.Xid).Nama + " " + _context.TblTitik.Find(koordinat.Yid).Nama,
                TidakKeX = koordinat.TidakKeX,
                TidakKeY = koordinat.TidakKeY,

                SbgAwalX = koordinat.SbgAwalX,
                SbgAwalY = koordinat.SbgAwalY,

                SbgAkhirX = koordinat.SbgAkhirX,
                SbgAkhirY = koordinat.SbgAkhirY,

                MiringAtas = koordinat.MiringAtas,
                MiringBawah = koordinat.MiringBawah,
                LengkungAtas = koordinat.LengkungAtas,
                LengkungBawah = koordinat.LengkungBawah,

                KiriBawahSemu = koordinat.KiriBawahSemu,

                PermintaanDariGambar = dariGambar,
                Skala = skala
            };

            //ViewBag.PolaId = titikPotongHor.PolaId;
            return View(koordTambahan);
        }

        // POST: TitikPotongHor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
                [Bind("KoordId,Xid,Yid," +
                        "TidakKeX,TidakKeY," +
                        "SbgAwalX,SbgAwalY," +
                        "SbgAkhirX,SbgAkhirY," +
                        "MiringAtas,MiringBawah," +
                        "LengkungAtas,LengkungBawah," +
                        "KiriBawahSemu," +
                        "PermintaanDariGambar,Skala, IsiPolaId"
                        )] OKoordTambahan koordTambah)
        {
            if (id != koordTambah.KoordId)
            {
                return NotFound();
            }

            var koord = new Koordinat()
            {
                KoordId = koordTambah.KoordId,
                //PolaId = koordTambah.PolaId,
                Xid = koordTambah.Xid,
                Yid = koordTambah.Yid,
                TidakKeX = koordTambah.TidakKeX,
                TidakKeY = koordTambah.TidakKeY,

                SbgAwalX = koordTambah.SbgAwalX,
                SbgAwalY = koordTambah.SbgAwalY,

                SbgAkhirX = koordTambah.SbgAkhirX,
                SbgAkhirY = koordTambah.SbgAkhirY,
                
                MiringAtas = koordTambah.MiringAtas,
                MiringBawah = koordTambah.MiringBawah,
                LengkungAtas = koordTambah.LengkungAtas,
                LengkungBawah = koordTambah.LengkungBawah,
                KiriBawahSemu = koordTambah.KiriBawahSemu
            };
            
            if (ModelState.IsValid)
            {
                if (id == koordTambah.KoordId)
                {
                    try
                    {
                        _context.Update(koord);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KoordinatExists(koord.KoordId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                if (koordTambah.PermintaanDariGambar)
                {
                    // kembali ke index gambar
                    return RedirectToAction(nameof(Index), "Gambar", new { id = koordTambah.IsiPolaId, skl = koordTambah.Skala });
                }
                else
                {
                    return RedirectToAction(nameof(Index), new { id = koordTambah.IsiPolaId });
                }
            }
           
            if (koordTambah.PermintaanDariGambar)
            {
                //kembali ke action gambar garis di gambar
                return RedirectToAction(nameof(Index),"Gambar", new { id = koordTambah.IsiPolaId, skl= koordTambah.Skala });
            }
            return View(koord);
        }

        private bool KoordinatExists(int id)
        {
            return _context.TblKoordinat.Any(e => e.KoordId == id);
        }
    }
}
