
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Interfaces;
using Stock.Repositories;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUnit, UnitRepository>();

builder.Services.AddDbContext<InventoryContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//18 36
//https://www.youtube.com/watch?v=aWpwWEzBN5I&list=PLKveM2BE9JqHa38TYJg_Sej61_u_uaOLF&index=4