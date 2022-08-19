using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using MiniBlog.Infrastructure.Data;
using MiniBlog.Infrastructure.DataSeed;
using MiniBlog.Core.Constants;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Web.Filters;
using MiniBlog.Web.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(opts => { opts.Filters.Add<ApplicationExceptionFilter>(); });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMiniBlogRepo, MiniBlogEfRepo>();
builder.Services.AddScoped<IRepository<Post>, EfPostsRepo>();
builder.Services.AddScoped<IReadRepository<Post>, EfPostsReadRepo>();
builder.Services.AddScoped<IRepository<Commentary>, EfCommentariesRepo>();
builder.Services.AddScoped<IReadRepository<Commentary>, EfCommentariesReadRepo>();
builder.Services.AddScoped<IRepository<Tag>, EfTagsRepo>();
builder.Services.AddScoped<IReadRepository<Tag>, EfTagsReadRepo>();
builder.Services.AddDbContext<MiniBlogEfContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MiniBlogDbContext"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    if (builder.Environment.IsDevelopment()) opts.EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<IdentityEfContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("IdentityEfContext"));
    if (builder.Environment.IsDevelopment()) opts.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opts => {
    opts.Cookie.Name = "TestCookieName";
});

builder.Services.AddScoped<ApplicationExceptionFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    SeedData.EnsureSeedContent(app.Services);
    await SeedData.EnsureSeedIdentity(app.Services);
}

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsDevelopment() || !app.Environment.IsTesting())
{
    app.UseHttpsRedirection();
}
app.UseStatusCodePages("text/html", ErrorTemplates.StatusCodePageTemplate);
app.UseStaticFiles();

if(app.Environment.IsTesting())
{
    app.UseMiddleware<TestsFakeAuthMiddleWare>();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();

public partial class Program { }