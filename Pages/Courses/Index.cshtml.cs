using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlatformaKursy.Data;
using PlatformaKursy.Models;

namespace PlatformaKursy.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db) => _db = db;

        public IList<Course> Courses { get; set; } = new List<Course>();

        public async Task OnGetAsync()
        {
            Courses = await _db.Courses.AsNoTracking().ToListAsync();
        }
    }
}