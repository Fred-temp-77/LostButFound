using LostButFound.Models;
using LostButFound.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostButFound.Controllers
{
    public class LostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public LostsController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Losts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Losts.ToListAsync());
        }

        // GET: Losts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lost = await _context.Losts
                .FirstOrDefaultAsync(m => m.LostId == id);
            if (lost == null)
            {
                return NotFound();
            }

            return View(lost);
        }

        // GET: Losts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Losts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lost lost)
        {
            if (ModelState.IsValid)
            {
                if (lost.ImageFile != null)
                {
                    string imageUrl = await _cloudinaryService.UploadImageAsync(lost.ImageFile);
                    lost.ImgByte = imageUrl;
                }
                lost.LostId = Guid.NewGuid();
                _context.Add(lost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lost);
        }

        // GET: Losts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lost = await _context.Losts.FindAsync(id);
            if (lost == null)
            {
                return NotFound();
            }
            return View(lost);
        }

        // POST: Losts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LostId,Name,Item,Location,Date,ItemDesc,ImgByte")] Lost lost)
        {
            if (id != lost.LostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostExists(lost.LostId))
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
            return View(lost);
        }

        // GET: Losts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lost = await _context.Losts
                .FirstOrDefaultAsync(m => m.LostId == id);
            if (lost == null)
            {
                return NotFound();
            }

            return View(lost);
        }

        // POST: Losts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lost = await _context.Losts.FindAsync(id);
            if (lost != null)
            {
                _context.Losts.Remove(lost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LostExists(Guid id)
        {
            return _context.Losts.Any(e => e.LostId == id);
        }
    }
}
