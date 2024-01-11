using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Data;
using ALLinONE.Shared; // AddNorthwindContext extension method

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddNorthwindContext();

builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews().AddViewLocalization();

var app = builder.Build();

string[] cultures = new[] { "en-US", "en-GB", "fr", "fr-FR", "pl-PL" };

RequestLocalizationOptions localizationOptions = new();

// cultures[0] will be en-US
localizationOptions.SetDefaultCulture(cultures[0])
    .AddSupportedCultures(cultures) // localization of data formats
    .AddSupportedUICultures(cultures); // localization of UI
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
