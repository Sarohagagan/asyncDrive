using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using asyncDrive.DataAccess;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utility;
using asyncDrive.Web.Middlewares;
using asyncDrive.Web.Service.IService;
using asyncDrive.Web.Service;
using System.Configuration;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("asyncDriveAuthConnectionString") ?? throw new InvalidOperationException("Connection string 'asyncDriveAuthConnectionString' not found.");

builder.Services.AddDbContext<asyncDriveAuthDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<asyncDriveAuthDbContext>();
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<asyncDriveAuthDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<asyncDriveAuthDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>//always add after AddIdentity
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();
// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Add HttpContextAccessor to access session in services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IApiService, asyncDrive.Web.Service.ApiService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor(); // Register HttpContextAccessor if needed
builder.Services.AddHttpClient(); // Register HttpClientFactory if needed
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession(); // Add this line to enable session handling
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthTokenHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
