using Blog.Application;
using Blog.Infrastructure;
using Blog.Infrastructure.DataSeed;
using Blog.MVC.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplication();
builder.Services.ConfigureInfrastructure(builder.Configuration);
if(!builder.Environment.IsDevelopment())
{
    builder.Services.AddMvc(opts => opts.Filters.Add<CustomExceptionFilter>());
}

var app = builder.Build();

SeedData.EnsureSeedContent(app.Services);
await SeedData.EnsureSeedIdentity(app.Services);

if (!app.Environment.IsDevelopment())
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

app.Run();

//for tests
public partial class Program { }
