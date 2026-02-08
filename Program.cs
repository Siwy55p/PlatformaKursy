using Microsoft.EntityFrameworkCore;
using PlatformaKursy.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Brak konfiguracji ³añcucha po³¹czenia 'DefaultConnection'. SprawdŸ appsettings.json lub secrets.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity (jeœli u¿ywasz ASP.NET Core Identity)
// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// sesja
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(2);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// jeœli korzystasz z Identity:
// app.UseAuthentication();
// app.UseAuthorization();

app.UseSession(); // <- wa¿ne

app.MapRazorPages();
app.Run();
