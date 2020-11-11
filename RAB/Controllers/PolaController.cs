using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAB.Data;
using RAB.Models.Utama;

namespace RAB.Controllers
{
    public class PolaController : Controller
    {
        private readonly RabContext _context;

        public PolaController(RabContext context)
        {
            _context = context;
        }

        // GET: Pola
        public async Task<IActionResult> Index()
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                return PartialView("RAB/_TblPola", await _context.TblPola.ToListAsync());
            }

            return View(await _context.TblPola.ToListAsync());
        }

        // GET: Pola/Create
        public IActionResult AddOrEdit(int id=0, bool add = true)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (!isAjax)
            {
                return RedirectToAction(nameof(Index));
            }

            if (add)
                return View(new Pola());
            else
                return View(_context.TblPola.Find(id));

        }

        // POST: Pola/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("PolaId,Nama,Status,SatuanPenyusun,Posisi3D")] Pola pola)
        {
            
            if (ModelState.IsValid)
            {
                if (pola.PolaId == 0)
                {
                    try
                    {
                        _context.Add(pola);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                    }
                }
                else if(id== pola.PolaId)
                {
                    try
                    {
                        _context.Update(pola);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PolaExists(pola.PolaId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "RAB/_TblPola",_context.TblPola.ToList() ) });
            }

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", pola) });
        }

        // Get: Pola/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOk(int? id)
        {
            try
            {
                // karena ada permasalahan di relational
                // maka tbl koordiant harus dihapus lebih dahulu
                var koords = _context.TblKoordinat.Where(k => k.TitikX.PolaId == id);
                _context.TblKoordinat.RemoveRange(koords);
                // maka tbl KomponenPola harus dihapus lebih dahulu
                var komps = _context.TblKomponenPola.Where(k => k.PolaId == id);
                _context.TblKomponenPola.RemoveRange(komps);

                var pola = await _context.TblPola.FindAsync(id);
                _context.TblPola.Remove(pola);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                ModelState.AddModelError("CostumError", ex.Message);
            }
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "RAB/_TblPola", _context.TblPola.ToList()) });
        }

        private bool PolaExists(int id)
        {
            return _context.TblPola.Any(e => e.PolaId == id);
        }
    }
}
