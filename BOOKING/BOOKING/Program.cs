using BOOKING;
using BOOKING.Models;
using BOOKING.Services;
using BOOKING.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookingService, BookingService>();
//mssql
builder.Services.AddDbContext<DbBooking>(builder =>
{
    builder.UseSqlServer(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DbBooking;Integrated Security=True");
});
//logowanie identity framework
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2; // do zmiany
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false; // mozna bedzie zmienic
    options.Password.RequireLowercase = false; // to tez

}).AddEntityFrameworkStores<DbBooking>();

var app = builder.Build();

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
