using Etrade.DAL.Abstract;
using Etrade.DAL.Concrete;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EtradeDbContext>();
builder.Services.AddScoped<ICategoryDAL,CategoryDAL>();
builder.Services.AddScoped<IProductDAL,ProductDAL>();
builder.Services.AddScoped<IOrderDAL,OrderDAL>();
builder.Services.AddScoped<IOrderlineDAL,OrderlineDAL>();

//AddIdentity
builder.Services.AddIdentity<User, Role>(options =>
{
    //lockout kilitlenme ayarlarý
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

    //password
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<EtradeDbContext>().AddDefaultTokenProviders();

//Cookie
builder.Services.ConfigureApplicationCookie(options=>
{
    options.LoginPath = "/Account/SignIn";
    options.AccessDeniedPath = "/";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan=TimeSpan.FromMinutes(10);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = false,
        SameSite=SameSiteMode.Lax,
        SecurePolicy=CookieSecurePolicy.Always,
    };
});
//add session
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//use session
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Identity iþlemi
app.UseAuthentication(); //giriþ kontrolü
app.UseAuthorization(); //yetkilendirme kontrolü



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
