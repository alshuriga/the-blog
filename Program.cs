using MiniBlog.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMiniBlogRepo, EFMiniBlogRepo>();
builder.Services.AddDbContext<MiniBlogDbContext>(opts =>

{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MiniBlogDbContext"));
});

Console.WriteLine(builder.Configuration.GetConnectionString("MiniBlogDbContext"));
var app = builder.Build();
app.EnsureSeed();
app.MapDefaultControllerRoute();
app.Run();


