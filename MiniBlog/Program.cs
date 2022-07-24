using MiniBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPostsRepo, EFPostsRepo>();
builder.Services.AddDbContext<MiniBlogDbContext>(opts =>

{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MiniBlogDbContext"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    if(builder.Environment.IsDevelopment()) opts.EnableSensitiveDataLogging();
}); 

var app = builder.Build();

app.UseStatusCodePages("text/html", ErrorTemplates.StatusCodePageTemplate);
app.UseStaticFiles();
app.EnsureSeed();
app.MapDefaultControllerRoute();

app.Run();


