using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RAB.Asset.Enum;
using RAB.BisnisModel.Sesi;
using RAB.Data;
using RAB.Models.Utama;

namespace RAB.Controllers
{
    public class KomponenGarisController : Controller
    {
        private readonly RabContext _context;

        public KomponenGarisController(RabContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLstGaris(int polaId)
        {
            //var sesPola = SPola.GetSesi(this);
            //sesPola.SetPolaId(polaId);
            SetSesPolaId(polaId);

            var lstGrs = _context.TblGaris
                            .Include(g => g.KoordAwal)
                            .Include(g => g.KoordAkhir)
                            .Include(g => g.KoordAwal.TitikX)
                            .Include(g => g.KoordAwal.TitikY)

                            .Where(p => p.KoordAwal.TitikX.PolaId == polaId).ToList();
            var lstJson = lstGrs.Select(g => new { g.GarisId, g.Nama }).ToList();
            var select = new SelectList(lstJson,"GarisId","Nama",0);
            return Ok(select);
        }

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

        // GET: KomponenGaris
        public async Task<IActionResult> Index(int? polaId, int? grsId, bool dariForm, bool dariAjax)
        {
            var sesPola = SPola.GetSesi(this);
            if (dariForm)
            {
                sesPola.SetPolaId(polaId);
                sesPola.SetGarisId(grsId);
            }
            else if(dariAjax)
            {
                polaId = sesPola.PolaId;
                grsId = sesPola.GarisId;
            }
            #region menambang model tblKomponenGaris
            var rabContext = _context.TblKomponenGaris
                                    .Include(k => k.Garis)
                                        .Include(g => g.Garis.KoordAwal)
                                        .Include(g => g.Garis.KoordAkhir)
                                        .Include(g => g.Garis.KoordAwal.TitikX)
                                        .Include(g => g.Garis.KoordAwal.TitikY)
                                        .Include(g => g.Garis.KoordAkhir.TitikX)
                                        .Include(g => g.Garis.KoordAkhir.TitikY)
                                    .Include(k => k.PolaKomponen).ThenInclude(p => p.Komponen)
                                    .Include(k => k.TitikZ)
                                    .Where(k => k.Garis.KoordAwal.TitikX.PolaId == polaId);
            #endregion
            if (grsId !=null && grsId != 0)
            {
                rabContext = rabContext.Where(k => k.Garis.GarisId == grsId);
            }

            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("RAB/_TblKomponenGaris", await rabContext.ToListAsync());
            }

            ViewData["PolaId"] = new SelectList(_context.TblPola.Where(p=>p.Status==EStatusPola.Utama), "PolaId", "Nama", polaId);

            return View(await rabContext.ToListAsync());
        }

        // GET: KomponenGaris/Edit/5
        public async Task<IActionResult> AddOrEdit(int? grsId, bool add=true)
        {
            var sesPola = SPola.GetSesi(this);
            var polaId = sesPola.PolaId;
            if (grsId == null)
            {
                grsId = sesPola.GarisId;
            }

            List<KomponenPola> lstKomp = new List<KomponenPola>();
            var komponenGaris = await _context.TblKomponenGaris.FindAsync(grsId);

            if (add)
            {
                komponenGaris = new KomponenGaris() { 
                    GarisId = (int) grsId,
                };
            }
            else
            {
                if (komponenGaris == null)
                {
                    return NotFound();
                }

                grsId = komponenGaris.GarisId;
                //polaId = komponenGaris.Garis.KoordAwal.PolaId;
            }


            var grs = _context.TblGaris
                        .Include(g => g.KoordAwal)
                        .Include(g => g.KoordAkhir)
                        .Include(g => g.KoordAwal.TitikX)
                        .Include(g => g.KoordAwal.TitikY)
                        .Include(g => g.KoordAkhir.TitikX)
                        .Include(g => g.KoordAkhir.TitikY)
                        .Where(g => g.GarisId == grsId) //.ToList();
                        .Single();

            ViewBag.GarisNama = grs.Nama + " (" + grs.Arah +")";
            var lstKomp4Select = _context.TblKomponenPola.Where(k => k.PolaId == polaId)
                                .Select(k => new { k.KomPolId, k.Komponen.Nama }).ToList();
            var lstPosZ = _context.TblTitik.Where(t => t.PolaId == polaId && t.Sumbu == ESumbu.Z)
                            .Select(t => new { t.TtkId, t.Nama }).ToList();

            ViewData["KompId"] = new SelectList(lstKomp4Select, "KomPolId", "Nama", komponenGaris.KompId);
            ViewData["PosZId"] = new SelectList(lstPosZ,"TtkId","Nama",komponenGaris.PosZId);

            return View(komponenGaris);
        }

        // POST: KomponenGaris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("KompGrsId,GarisId,KompId,PosRelatifDiZ,PosZId,PosRelatif,PosRelX,PosRelY,OffsetKeKanan")] KomponenGaris komponenGaris)
        {
            var sesPola = SPola.GetSesi(this);
            var permintaanDariGambar = sesPola.DariGambar;

            if (ModelState.IsValid)
            {
                if (komponenGaris.KompGrsId == 0)
                {
                    try
                    {
                        _context.Add(komponenGaris);
                        await _context.SaveChangesAsync();
                    }
                    catch { }

                }
                else if(id == komponenGaris.KompGrsId)
                {
                    try
                    {
                        _context.Update(komponenGaris);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KomponenGarisExists(komponenGaris.KompGrsId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                if (permintaanDariGambar){
                    // kembali ke index gambar
                    return RedirectToAction("KompGaris", "Gambar");
                }
                else {
                    return RedirectToAction(nameof(Index));
                }
            }


            var grs = _context.TblGaris
                        .Include(g => g.KoordAwal)
                        .Include(g => g.KoordAkhir)
                        .Include(g => g.KoordAwal.TitikX)
                        .Include(g => g.KoordAwal.TitikY)
                        .Include(g => g.KoordAkhir.TitikX)
                        .Include(g => g.KoordAkhir.TitikY)
                        .Where(g => g.GarisId == id) //.ToList();
                        .Single();
            int polaId = grs.KoordAwal.TitikX.Pola.PolaId;

            ViewBag.GarisNama = grs.Nama + " (" + grs.Arah + ")";
            var lstKomp4Select = _context.TblKomponenPola.Where(k => k.PolaId == polaId)
                                .Select(k => new { k.KomPolId, k.Komponen.Nama }).ToList();
            var lstPosZ = _context.TblTitik.Where(t => t.PolaId == polaId && t.Sumbu == ESumbu.Z)
                            .Select(t => new { t.TtkId, t.Nama }).ToList();

            ViewData["KompId"] = new SelectList(lstKomp4Select, "KomPolId", "Nama", komponenGaris.KompId);
            ViewData["PosZId"] = new SelectList(lstPosZ, "TtkId", "Nama", komponenGaris.PosZId);

            if (permintaanDariGambar){
                //kembali ke action gambar garis di gambar
                return RedirectToAction("KompGaris", "Gambar");
            }

            return View(komponenGaris);
        }

        // POST: KomponenGaris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komponenGaris = await _context.TblKomponenGaris.FindAsync(id);
            _context.TblKomponenGaris.Remove(komponenGaris);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomponenGarisExists(int id)
        {
            return _context.TblKomponenGaris.Any(e => e.KompGrsId == id);
        }
    }
}
