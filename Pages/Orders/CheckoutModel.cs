using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformaKursy.Data;
using PlatformaKursy.Models;
using PlatformaKursy.Utilities;

namespace PlatformaKursy.Pages.Orders
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public CheckoutModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Address { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cart = HttpContext.Session.GetObject<Dictionary<int, int>>("Cart") ?? new();
            if (cart.Count == 0) return RedirectToPage("/Cart/Index");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // powinien rzadziej wyst¹piæ dziêki [Authorize], ale na wszelki wypadek
                return Challenge();
            }

            var courses = _db.Courses.Where(c => cart.Keys.Contains(c.Id)).ToList();

            var order = new Order
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                Total = 0m
            };

            foreach (var kv in cart)
            {
                var course = courses.First(c => c.Id == kv.Key);
                var qty = kv.Value;

                var item = new OrderItem
                {
                    CourseId = course.Id,
                    Quantity = qty,
                    UnitPrice = course.Price
                };
                order.Items.Add(item);
                order.Total += course.Price * qty;
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            HttpContext.Session.Remove("Cart"); // wyczyœæ koszyk po zamówieniu

            return RedirectToPage("/Orders/Success", new { id = order.Id });
        }
    }
}