using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAB.Asset.OlahanModel;
using RAB.Data;
using RAB.Models.Utama;

namespace RAB.Controllers
{
    public class GarisController : Controller
    {
        private readonly RabContext _context;

        public GarisController(RabContext context)
        {
            _context = context;
        }

        // GET: Garis
        public async Task<IActionResult> Index()
        {
            QCekGaris qGrs = new QCekGaris(_context, 3);
            qGrs.UpdateDariGambar();



            var rabContext = _context.TblGaris.Include(g => g.KoordAkhir).Include(g => g.KoordAwal);
            return View(await rabContext.ToListAsync());
        }

        // GET: Garis/Edit/5
        public async Task<IActionResult> AddOrEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garis = await _context.TblGaris.FindAsync(id);
            if (garis == null)
            {
                return NotFound();
            }
            ViewData["AkhirId"] = new SelectList(_context.TblKoordinat, "KoordId", "KoordId", garis.AkhirId);
            ViewData["AwalId"] = new SelectList(_context.TblKoordinat, "KoordId", "KoordId", garis.AwalId);
            return View(garis);
        }

        // POST: Garis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("GarisId,AwalId,AkhirId")] Garis garis)
        {
            if (id != garis.GarisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(garis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarisExists(garis.GarisId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkhirId"] = new SelectList(_context.TblKoordinat, "KoordId", "KoordId", garis.AkhirId);
            ViewData["AwalId"] = new SelectList(_context.TblKoordinat, "KoordId", "KoordId", garis.AwalId);
            return View(garis);
        }

        // POST: Garis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garis = await _context.TblGaris.FindAsync(id);
            _context.TblGaris.Remove(garis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GarisExists(int id)
        {
            return _context.TblGaris.Any(e => e.GarisId == id);
        }
    }
}
