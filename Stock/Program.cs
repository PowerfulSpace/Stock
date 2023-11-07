
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Repositories;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProduct, ProductRepository>();
builder.Services.AddScoped<IUnit, UnitRepository>();
builder.Services.AddScoped<IProductGroup, ProductGroupRepository>();
builder.Services.AddScoped<IProductProfile, ProductProfileRepository>();
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<IBrand, BrandRepository>();


builder.Services.AddDbContext<InventoryContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<InventoryContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

//11 50
//https://www.youtube.com/watch?v=aWpwWEzBN5I&list=PLKveM2BE9JqHa38TYJg_Sej61_u_uaOLF&index=4