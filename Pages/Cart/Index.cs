using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlatformaKursy.Data;
using PlatformaKursy.Models;
using PlatformaKursy.Utilities;

namespace PlatformaKursy.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public List<(Course Course, int Quantity)> Items { get; set; } = new();

        public async Task OnGetAsync()
        {
            var cart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") ?? new();
            if (cart.Count == 0) return;

            var ids = cart.Keys.ToList();
            var courses = await _db.Courses.Where(c => ids.Contains(c.Id)).ToListAsync();
            Items = courses.Select(c => (c, cart[c.Id])).ToList();
        }
    }
}