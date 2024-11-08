using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Repositories;
using MimicAPI.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MimicContext>(options =>
    options.UseSqlite("Data Source=Database\\mimic.db"));

builder.Services.AddScoped<IPalavraRepository, PalavraRepository>();

#region AutoMapper - Configuração
var config = new MapperConfiguration(cfg => {
    cfg.AddProfile(new DTOMapperProfile());
});
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion


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

app.Run();
