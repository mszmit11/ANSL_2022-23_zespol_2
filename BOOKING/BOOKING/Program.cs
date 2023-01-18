using BOOKING.CloudStorage;
using BOOKING.Data;
using BOOKING.Models;
using BOOKING.Services;
using BOOKING.Services.Interface;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookingService, BookingService>();
//mssql
builder.Services.AddDbContext<DbBooking>(builder =>
{
    builder.UseSqlServer(@"Data Source=mssql6.webio.pl,2401;Database=ms777_bookingApp;Uid=ms777_userData;Password=QWErtyUIO123!;TrustServerCertificate=True");
});
//@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DbBooking;Integrated Security=True"
//@"Data Source=mssql5.webio.pl,2401;Database=testbooking_BOOKING;Uid=testbooking_adminDB;Password=rwjNkZiSgXzL5nU@;TrustServerCertificate=True"

//logowanie identity framework
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2; // do zmiany
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false; // mozna bedzie zmienic
    options.Password.RequireLowercase = false; // to tez

}).AddEntityFrameworkStores<DbBooking>();


builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetService<UserManager<UserModel>>();
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

    await ContextSeed.SeedRolesAsync(userManager, roleManager);
    await ContextSeed.SeedAdminAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
