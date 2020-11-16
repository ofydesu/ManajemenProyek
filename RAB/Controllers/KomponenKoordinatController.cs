using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RAB.Asset.Enum;
using RAB.BisnisModel.Sesi;
using RAB.Data;
using RAB.Models.Utama;

namespace RAB.Controllers
{
    public class KomponenKoordinatController : Controller
    {
        private readonly RabContext _context;

        public KomponenKoordinatController(RabContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLstKoord(int polaId)
        {
            var sesPola = SPola.GetSesi(this);
            sesPola.SetPolaId(polaId);

            var lstGrs = _context.TblKoordinat
                            .Include(k=>k.TitikX)
                            .Include(k=>k.TitikY)
                            .Where(p => p.TitikX.PolaId == polaId).ToList();
            var lstJson = lstGrs.Select(g => new { g.KoordId, g.Nama }).ToList();
            var select = new SelectList(lstJson, "KoordId", "Nama", 0);
            return Ok(select);
        }

        // GET: KomponenKoordinat
        public IActionResult Index(int? polaId, int? koorId, bool dariForm, bool dariAjax)
        {
            #region simpan/ambil kode ke/dari sesi
            var sesPola = SPola.GetSesi(this);
            if(dariAjax)
            {
                polaId = sesPola.PolaId;
                koorId = sesPola.KoorId;
            }
            #endregion
            #region tblKomponenKoordinat
            var rabContext = _context.TblKomponenKoordinat.AsQueryable();

            if (polaId != null) { 
                rabContext = rabContext.Where(k => k.PolaKomponen.PolaId == polaId);
                sesPola.SetPolaId(polaId);
            }
            #endregion

            if (koorId != null && koorId != 0)
            {
                rabContext = rabContext.Where(k=>k.KoorId == koorId);
                sesPola.SetKoorId(koorId);
            }

            //rabContext = rabContext.Include(k => k.Koordinat);


            if (polaId == null && koorId == null) {
                rabContext = rabContext.Where(k => k.KoorId == 0);
            }
            else
            {
                rabContext = rabContext
                                    .Include(k => k.Koordinat)
                                        .Include(k => k.Koordinat.TitikX)
                                        .Include(k => k.Koordinat.TitikY)
                                    .Include(k => k.PolaKomponen)
                                        .Include(k => k.PolaKomponen.Komponen)
                                    .Include(k => k.TitikZatas)
                                    .Include(k => k.TitikZbawah)
                                    ;
                //simpan polaId dan koorId di sessi
            }

            ViewBag.KoorId = koorId;
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("RAB/_TblKomponenKoordinat", rabContext.ToList());
            }

            ViewData["PolaId"] = new SelectList(_context.TblPola.Where(p => p.Status == EStatusPola.Utama), "PolaId", "Nama", polaId);
            return View( rabContext.ToList());
        }

        // GET: KomponenKoordinat/Edit/5
        public async Task<IActionResult> AddOrEdit(int? komKoorId,int? koorId, bool add = true)
        {
            var sesPola = SPola.GetSesi(this);
            var polaId = sesPola.PolaId;
            if (koorId == null)
            {
                // untuk add
                koorId = sesPola.KoorId;
            }

            List<KomponenPola> lstKomp = new List<KomponenPola>();
            var komponenKoor = await _context.TblKomponenKoordinat.FindAsync(komKoorId);

            if (add)
            {
                komponenKoor = new KomponenKoordinat()
                {
                    KoorId = (int)koorId,
                };
            }
            else
            {
                if (komponenKoor == null)
                {
                    return NotFound();
                }

                koorId = komponenKoor.KoorId;
                //polaId = komponenGaris.Garis.KoordAwal.PolaId;
            }

            var koor = _context.TblKoordinat.Include(k => k.TitikX).Include(k => k.TitikY)
                            .Where(k=>k.KoordId == koorId).Single();

            ViewBag.KoorNama = koor.Nama;
            var lstKomp4Select = _context.TblKomponenPola.Where(k => k.PolaId == polaId && (int) k.Komponen.Posisi3D == (int) EPosisi3D.Kolom)
                                .Select(k => new { k.KomPolaId, k.Komponen.Nama }).ToList();
            var lstPosZ = _context.TblTitik.Where(t => t.PolaId == polaId && t.Sumbu == ESumbu.Z)
                            .OrderByDescending(t=>t.Jarak)
                            .Select(t => new { t.TtkId, t.Nama }).ToList();

            ViewData["KompId"] = new SelectList(lstKomp4Select, "KomPolId", "Nama", komponenKoor.KompId);
            ViewData["PosAtasId"] = new SelectList(lstPosZ, "TtkId", "Nama", komponenKoor.PosAtasId);
            ViewData["PosBawahId"] = new SelectList(lstPosZ, "TtkId", "Nama", komponenKoor.PosBawahId);
            return View(komponenKoor);
        }

        // POST: KomponenKoordinat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("KompKoorId,KoorId,KompId,PosAtasId,PosBawahId")] KomponenKoordinat komponenKoor)
        {
            var sesPola = SPola.GetSesi(this);
            var permintaanDariGambar = sesPola.DariGambar;
            var koor = _context.TblKoordinat.Include(k => k.TitikX).Include(k => k.TitikY) //.ThenInclude(k => k.Pola)
                            .Where(k => k.KoordId == komponenKoor.KoorId).Single();
            int polaId = koor.TitikX.PolaId;

            if (ModelState.IsValid)
            {
                if (komponenKoor.KompKoorId == 0)
                {
                    try
                    {
                        _context.Add(komponenKoor);
                        await _context.SaveChangesAsync();
                    }
                    catch { }

                }
                else if (id == komponenKoor.KompKoorId)
                {
                    try
                    {
                        _context.Update(komponenKoor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KomponenKoordinatExists(komponenKoor.KompKoorId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                if (permintaanDariGambar)
                {
                    // kembali ke index gambar
                    return RedirectToAction("KompGaris", "Gambar");
                }
                else
                {
                    return RedirectToAction(nameof(Index),new {polaId= polaId, koorId = komponenKoor.KoorId });
                }
            }


            ViewBag.KoorNama = koor.Nama;
            var lstKomp4Select = _context.TblKomponenPola.Where(k => k.PolaId == polaId && (int)k.Komponen.Posisi3D == (int)EPosisi3D.Kolom)
                                .Select(k => new { k.KomPolaId, k.Komponen.Nama }).ToList();
            var lstPosZ = _context.TblTitik.Where(t => t.PolaId == polaId && t.Sumbu == ESumbu.Z)
                            .OrderByDescending(t => t.Jarak)
                            .Select(t => new { t.TtkId, t.Nama }).ToList();

            ViewData["KompId"] = new SelectList(lstKomp4Select, "KomPolId", "Nama", komponenKoor.KompId);
            ViewData["PosAtasId"] = new SelectList(lstPosZ, "TtkId", "Nama", komponenKoor.PosAtasId);
            ViewData["PosBawahId"] = new SelectList(lstPosZ, "TtkId", "Nama", komponenKoor.PosBawahId);
            return View(komponenKoor);

        }

        // POST: KomponenKoordinat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komponenKoordinat = await _context.TblKomponenKoordinat.FindAsync(id);
            _context.TblKomponenKoordinat.Remove(komponenKoordinat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomponenKoordinatExists(int id)
        {
            return _context.TblKomponenKoordinat.Any(e => e.KompKoorId == id);
        }
    }
}
