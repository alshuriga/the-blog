using Microsoft.AspNetCore.Identity;
using MiniBlog.Infrastructure.Data;
using MiniBlog.Infrastructure.DataSeed;
using MiniBlog.Core.Constants;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPostsRepo, EFPostsRepo>();
builder.Services.AddDbContext<MiniBlogDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MiniBlogDbContext"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    if(builder.Environment.IsDevelopment()) opts.EnableSensitiveDataLogging();
});
builder.Services.AddDbContext<IdentityEfContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("IdentityEfContext"));
    if(builder.Environment.IsDevelopment()) opts.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityEfContext>();
builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequireDigit = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 5;
    opts.User.RequireUniqueEmail = true;
});

var app = builder.Build();
    
IdentitySeedData.EnsureSeed(app.Services);
app.UseStatusCodePages("text/html", ErrorTemplates.StatusCodePageTemplate);
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();


