using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webphone._Models;

namespace Webphone.Controllers
{
    //[Authorize]
    public class ProductsController : Controller
    {
        private readonly C4AsmContext _context;

        public ProductsController(C4AsmContext context)
        {
            _context = context;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
        // GET: Products
        
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Mnuser") == null)
            {
                TempData["Denied access"] = "You are restricted from logging in to this site.";
                return RedirectToAction("Login", "Home");
            }
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Mnuser") == null)
            {
                TempData["Denied access"] = "You are restricted from logging in to this site.";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Mnuser") == null)
            {
                TempData["Denied access"] = "You are restricted from logging in to this site.";
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProId,ProName,Quantity,ProBrand,ProType,Ram,Cpu,Hdh,Camera,Color,ProPrice")] Product product, IFormFile ProImg)
        {
            if (ModelState.IsValid)
            {
                if(ProImg != null && ProImg.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ProImg.CopyToAsync(memoryStream);
                        product.ProImg = memoryStream.ToArray();
                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Trả về view nếu dữ liệu không hợp lệ
            return View(product);
        }



        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Mnuser") == null)
            {
                TempData["Denied access"] = "You are restricted from logging in to this site.";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProId,ProName,Quantity,ProBrand,ProType,Ram,Cpu,Hdh,Camera,Color,ProPrice")] Product product, IFormFile ProImg)
        {
            if (id != product.ProId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ProImg != null && ProImg.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ProImg.CopyToAsync(memoryStream);
                            product.ProImg = memoryStream.ToArray();
                        }
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Mnuser") == null)
            {
                TempData["Denied access"] = "You are restricted from logging in to this site.";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProId == id);
        }
    }
}
