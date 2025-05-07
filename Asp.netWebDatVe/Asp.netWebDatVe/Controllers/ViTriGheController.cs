using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    public class ViTriGheController : Controller
    {
        private readonly QLDatVeContext _context;

        public ViTriGheController(QLDatVeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Vitrighes.Include(v => v.BiensoNavigation).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.XeList = _context.Xes.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Vitrighe vitrige)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vitrige);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.XeList = _context.Xes.ToList();
            return View(vitrige);
        }

        // GET: Vitrighes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vitrighes == null)
            {
                return NotFound();
            }

            var vitrighe = await _context.Vitrighes.FindAsync(id);
            if (vitrighe == null)
            {
                return NotFound();
            }

            // Đổ dữ liệu cho dropdown biển số xe
            ViewData["Bienso"] = new SelectList(_context.Xes, "Bienso", "Bienso", vitrighe.Bienso);
            return View(vitrighe);
        }

        // POST: Vitrighes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVitri,Bienso,Tenvitri,Trangthai")] Vitrighe vitrighe)
        {
            if (id != vitrighe.IdVitri)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vitrighe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VitrigheExists(vitrighe.IdVitri))
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

            ViewData["Bienso"] = new SelectList(_context.Xes, "Bienso", "Bienso", vitrighe.Bienso);
            return View(vitrighe);
        }

        private bool VitrigheExists(int id)
        {
            return (_context.Vitrighes?.Any(e => e.IdVitri == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vitri = await _context.Vitrighes.FindAsync(id);
            if (vitri == null) return NotFound();

            _context.Vitrighes.Remove(vitri);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var vitri = await _context.Vitrighes.Include(v => v.BiensoNavigation)
                                                .FirstOrDefaultAsync(v => v.IdVitri == id);
            if (vitri == null) return NotFound();
            return View(vitri);
        }
    }
    }
