using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using RouteSheet.Data;
using RouteSheet.Data.Repositories;

using RouteSheet.Shared.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>(o => o.Password.RequireNonAlphanumeric = false)
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddTransient<SeedData>();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    AddDataToDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}



app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

void AddDataToDatabase(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedData>();
        service.AddUsers().Wait();
    }
}