using Blog.Application;
using Blog.Infrastructure;
using Blog.Infrastructure.DataSeed;
using Blog.MVC.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplication();
builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddMvc(opts => opts.Filters.Add<CustomExceptionFilter>());
if(!builder.Environment.IsDevelopment())
{
    builder.Services.AddMvc(opts => opts.Filters.Add<CustomExceptionFilter>());
}
builder.Services.AddRazorPages();

var app = builder.Build();

SeedData.EnsureSeedContent(app.Services);
await SeedData.EnsureSeedIdentity(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=posts}/{action=list}");
app.MapRazorPages();

//for tests
public partial class Program { }
