using MiniBlog.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPostsRepo, EFPostsRepo>();
builder.Services.AddDbContext<MiniBlogDbContext>(opts =>

{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MiniBlogDbContext"));
});

var app = builder.Build();

app.UseStaticFiles();
app.EnsureSeed();
app.MapDefaultControllerRoute();

app.Run();


