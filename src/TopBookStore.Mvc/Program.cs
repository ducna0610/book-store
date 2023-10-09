using Microsoft.EntityFrameworkCore;
using TopBookStore.Domain.Interfaces;
using TopBookStore.Infrastructure.Persistence;
using TopBookStore.Mvc.Middleware;
using TopBookStore.Infrastructure.Identity;
using TopBookStore.Application.Interfaces;
using TopBookStore.Application.Services;
using TopBookStore.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var TopBookStoreCS = builder.Configuration.GetConnectionString("TopBookStoreCS");
builder.Services.AddDbContext<TopBookStoreContext>(options =>
    options.UseSqlServer(TopBookStoreCS));

var ApplicationDbContextCS = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(ApplicationDbContextCS));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ITopBookStoreUnitOfWork, TopBookStoreUnitOfWork>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.UseMiddleware<CategoriesMiddlewar>();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "paging_books",
    pattern: "{controller}/{action}/page-{pagenumber}/size-{pagesize}/filter-{categoryid}-{price}-{numberofpages}-{authorid}");
app.MapControllerRoute(
    name: "paging",
    pattern: "{controller}/{action}/page-{pagenumber}/size-{pagesize}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();