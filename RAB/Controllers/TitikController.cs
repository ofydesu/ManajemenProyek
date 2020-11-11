using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RAB.Asset.Enum;
using RAB.Asset.Interface;
using RAB.BisnisModel.Sesi;
using RAB.Data;
using RAB.Models.Utama;
using static RAB.Helper;

namespace RAB.Controllers
{
    public class TitikController : Controller
    {
        private readonly RabContext _context;

        public TitikController(RabContext context)
        {
            _context = context;
        }

        // GET: Grid

        public IActionResult Index(int? id)
        {
            //buat daftar pola yang tersedia untuk pilihan Pola
            var sesPola = SPola.GetSesi(this);
            if (id != null)
                sesPola.SetPolaId(id);
            else
                id = sesPola.PolaId;

            var rabConAll = _context.TblTitik.Include(g => g.Pola)
                                .Where(pol => pol.PolaId == id)
                                .ToList().AsQueryable();

            var rabConNonZ = rabConAll.Where(k => (int)k.Sumbu != (int)ESumbu.Z)
                            .OrderBy(p => p.Sumbu).ThenBy(p2 => p2.SumbuId);
            var rabConZ = rabConAll.Where(k => (int)k.Sumbu == (int)ESumbu.Z)
                            .OrderByDescending(t=>t.Jarak);

            var rabContext = rabConNonZ.Union(rabConZ);

            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                return PartialView("RAB/_TblTitik", rabContext.ToList());
            }

            ViewBag.PolaId = id;
            ViewBag.ListPola = new SelectList(_context.TblPola, "PolaId", "Nama",id);

            return View(rabContext.ToList());
        }

        public IActionResult MaxSumbu (int polaId  , int eSumId)
        {
            //ESumbu sumb = (ESumbu)eSumId;
            int max = 0;
            try
            {
                max = _context.TblTitik.Where(g => g.PolaId == polaId && (int) g.Sumbu == eSumId).Max(m => m.SumbuId);
            }
            catch { }
            return  Ok( max + 1);
        }

        // GET: Grid/Create
        public IActionResult AddOrEdit(int id, bool add = true)
        {

            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (!isAjax)
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == 0)
            {
                return NotFound();  //false; //RedirectToAction(nameof(Index));
            }

            int sumbId = 0;
            int polaId = 0;

            var modelView = _context.TblTitik.Find(id);
            if(add)
            {
                //buat baru
                modelView = new Titik();
                var tGrid = _context.TblTitik.Where(g => g.PolaId == id).ToList();
                if (tGrid.Count > 0)
                    sumbId = tGrid.Where(g => g.Sumbu == ESumbu.X).Max(m => m.SumbuId) +1;
                polaId = id;
            }
            else
            {
                // edit yang sudah ada
                polaId = (int)modelView.PolaId;
                sumbId = modelView.SumbuId;
            }

            var pola = _context.TblPola.Find(polaId);
            ViewBag.PolaId = polaId;
            ViewBag.PolaNama = pola.Nama;
            ViewBag.SumbuId = sumbId == 0? 1 : sumbId;
            ViewBag.Satuan = pola.SatuanPenyusun;
            return View(modelView);
        }

        // POST: Grid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("PolaId,TtkId,Sumbu,SumbuId,Nama,Jarak,PosAbs,PosReal")] Titik grid)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _context.Add(grid);
                    await _context.SaveChangesAsync();
                }
                else if (id == grid.TtkId)
                {
                    try
                    {
                        // datang dari Edit
                        _context.Update(grid);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TitikExists(grid.TtkId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }

                    }
                }
                // mendapatkan string view tanpa perlu ke methode yang bersangkutan
                var modalAjax = _context.TblTitik.Include(g => g.Pola)
                                    .Where(g => g.PolaId == grid.PolaId)
                                    .OrderBy(o => o.Sumbu).ThenBy(o => o.SumbuId)
                                    .ToList();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this,"RAB/_TblTitik",modalAjax) });
            }
            // mendapatkan string view tanpa perlu ke methode yang bersangkutan
            //jika  gagal
            return Json(new {isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", grid) });
        }

        // Post: Grid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var grid = await _context.TblTitik.FindAsync(id);
            _context.TblTitik.Remove(grid);
            await _context.SaveChangesAsync();
            return Json(new {html = Helper.RenderRazorViewToString(this, "RAB/_TblGrid", _context.TblTitik.Include(g=>g.Pola).Where(g=>g.PolaId == grid.PolaId).ToList()) });
        }

        private bool TitikExists(int id)
        {
            return _context.TblTitik.Any(e => e.TtkId == id);
        }
    }
}
