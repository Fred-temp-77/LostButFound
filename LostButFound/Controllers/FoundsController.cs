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
    public class FoundsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public FoundsController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Founds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Founds.ToListAsync());
        }

        // GET: Founds/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var found = await _context.Founds
                .FirstOrDefaultAsync(m => m.FoundId == id);
            if (found == null)
            {
                return NotFound();
            }

            return View(found);
        }

        // GET: Founds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Founds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Found found)
        {
            if (ModelState.IsValid)
            {
                if (found.ImageFile != null)
                {
                    string imageUrl = await _cloudinaryService.UploadImageAsync(found.ImageFile);
                    found.ImgByte = imageUrl;
                }
                found.FoundId = Guid.NewGuid();
                _context.Add(found);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(found);
        }

        // GET: Founds/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var found = await _context.Founds.FindAsync(id);
            if (found == null)
            {
                return NotFound();
            }
            return View(found);
        }

        // POST: Founds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FoundId,Name,Item,Location,Date,ItemDesc,ImgByte")] Found found)
        {
            if (id != found.FoundId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(found);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoundExists(found.FoundId))
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
            return View(found);
        }

        // GET: Founds/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var found = await _context.Founds
                .FirstOrDefaultAsync(m => m.FoundId == id);
            if (found == null)
            {
                return NotFound();
            }

            return View(found);
        }

        // POST: Founds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var found = await _context.Founds.FindAsync(id);
            if (found != null)
            {
                _context.Founds.Remove(found);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoundExists(Guid id)
        {
            return _context.Founds.Any(e => e.FoundId == id);
        }
    }
}
