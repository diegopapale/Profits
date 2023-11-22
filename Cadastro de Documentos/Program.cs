using Cadastro_de_Documentos.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<Context>(options =>
    options.UseNpgsql("Host=localhost;Port=5434;Pooling=true;Database=CADASTRO_DOCUMENTO;User Id=postgres;Password=Bmc0785*;"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "exportToExcel",
    pattern: "Documents/ExportToExcel",
    defaults: new { controller = "Documents", action = "ExportToExcel" });

app.Run();
