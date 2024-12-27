using DataManagerCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.OpenApi.Models;
using DataManager.Library.DataAccess;
using DataManager.Library.Internal.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// personal services
builder.Services.AddTransient<IInventoryData, InventoryData>();
builder.Services.AddTransient<IProductData, ProductData>();
builder.Services.AddTransient<ISaleData, SaleData>();
builder.Services.AddTransient<IUserData, UserData>();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
    .AddJwtBearer("JwtBearer", jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyTBDTestWord1234567890")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5),
        };
    }
);
builder.Services.AddSwaggerGen(setup =>
    {
        setup.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "Retail Manager API",
                Version = "v1",
            });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(x =>
    { x.SwaggerEndpoint("/swagger/v1/swagger.json", "Retail Data API v1"); }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();