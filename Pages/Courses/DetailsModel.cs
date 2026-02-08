
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformaKursy.Data;
using PlatformaKursy.Models;
using PlatformaKursy.Utilities;

namespace PlatformaKursy.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DetailsModel(ApplicationDbContext db) => _db = db;

        public Course? Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _db.Courses.FindAsync(id);
            if (Course == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var cart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") ?? new();
            if (cart.ContainsKey(id)) cart[id]++;
            else cart[id] = 1;
            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToPage("/Cart/Index");
        }
    }
}