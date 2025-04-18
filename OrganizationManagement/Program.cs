
// using Microsoft.EntityFrameworkCore;

// using OrganizationManagement.DBContext;

// var builder = WebApplication.CreateBuilder(args);


// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"))
// );


// builder.Services.AddControllersWithViews();


// builder.Services.AddAuthentication("CustomCookieAuth")
//     .AddCookie("CustomCookieAuth", options =>
//     {
//         options.Cookie.Name = "OrganizationManagement";
//         options.Cookie.HttpOnly = true;
//         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//         options.Cookie.SameSite = SameSiteMode.Strict;
//         options.ExpireTimeSpan = TimeSpan.FromHours(24);
//         options.LoginPath = "/Home/Login";
//         options.LogoutPath = "/Home/Logout";
//         options.AccessDeniedPath = "/Home/AccessDenied";
//         options.SlidingExpiration = true;
//     });



// var app = builder.Build();


// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts(); 
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();  

// app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();
// app.UseDeveloperExceptionPage();

// // Define the default route
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"))
);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("CustomCookieAuth")
    .AddCookie("CustomCookieAuth", options =>
    {
        options.Cookie.Name = "OrganizationManagement";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/Home/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();  

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

// Define the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
