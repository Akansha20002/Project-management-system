using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Configure the DbContext to use PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"))
);

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Add HSTS only in non-development environments
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Enable static files serving

app.UseRouting();

app.UseAuthorization();

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
