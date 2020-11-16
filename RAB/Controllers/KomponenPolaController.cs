using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAB.Asset.Enum;
using RAB.Data;
using RAB.Models.Utama;

namespace RAB.Controllers
{
    public class KomponenPolaController : Controller
    {
        private readonly RabContext _context;

        public KomponenPolaController(RabContext context)
        {
            _context = context;
        }

        // GET: KomponenPola
        public async Task<IActionResult> Index(int id)
        {
            var rabContext = _context.TblKomponenPola.Include(k => k.Komponen).Include(k => k.Pola).Where(k=>k.PolaId==id);

            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            ViewBag.PolaId = id;
            if (isAjax){
                return PartialView("RAB/_TblKomponenPola", await rabContext.Where(k=>k.PolaId==id).ToListAsync());
            }

            ViewData["ListPola"] = new SelectList(_context.TblPola.Where(p => p.Status == EStatusPola.Utama), "PolaId", "Nama", id);

            return View(await rabContext.ToListAsync());
        }

        // GET: KomponenPola/Edit/5
        public async Task<IActionResult> AddOrEdit(int? id, bool add = true)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komponenPola = await _context.TblKomponenPola.FindAsync(id);
            int polaId;
            if (add)
            {
                polaId = (int)id;
                komponenPola = new KomponenPola();
            }
            else
            {
                polaId = (int)komponenPola.PolaId;
            }

            if (komponenPola == null)
            {
                return NotFound();
            }

            ViewBag.PolaNama = _context.TblPola.Find(polaId).Nama;
            ViewBag.PolaId = polaId;
            ViewData["KompId"] = new SelectList(_context.TblPola.Where(p=>p.Status==EStatusPola.Komponen), "PolaId", "Nama", komponenPola.KompId);

            return View(komponenPola);
        }

        // POST: KomponenPola/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("KomPolaId,PolaId,KompId")] KomponenPola komponenPola)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    try
                    {
                        _context.Add(komponenPola);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CustomError", ex.Message);
                    }
                }
                else if (id == komponenPola.KomPolaId)
                { 

                    try
                    {
                        _context.Update(komponenPola);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KomponenPolaExists(komponenPola.KomPolaId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var modalAjax = await _context.TblKomponenPola.Include(k => k.Komponen).Include(k => k.Pola)
                                        .Where(k => k.PolaId == komponenPola.PolaId).ToListAsync();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "RAB/_TblKomponenPola", modalAjax) });
            }
            ViewData["KompId"] = new SelectList(_context.TblPola, "PolaId", "Nama", komponenPola.KompId);
            //ViewData["PolaId"] = new SelectList(_context.TblPola, "PolaId", "Nama", komponenPola.PolaId);
            //return View(komponenPola);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", komponenPola) });
        }

        // POST: KomponenPola/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komponenPola = await _context.TblKomponenPola.FindAsync(id);
            _context.TblKomponenPola.Remove(komponenPola);
            await _context.SaveChangesAsync();
            var modalAjax = await _context.TblKomponenPola.Include(k => k.Komponen).Include(k => k.Pola)
                                    .Where(k => k.PolaId == komponenPola.PolaId).ToListAsync();
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "RAB/_TblKomponenPola", modalAjax) });
        }

        private bool KomponenPolaExists(int id)
        {
            return _context.TblKomponenPola.Any(e => e.KomPolaId == id);
        }
    }
}
