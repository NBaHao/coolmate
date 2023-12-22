using CloudinaryDotNet;
using CoolMate.Helpers;
using CoolMate.Interfaces;
using CoolMate.Models;
using CoolMate.Repositories;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using CoolMate.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.EntityFrameworkCore.Extensions;
using System.Text;
using System.Text.Json.Serialization;
using CoolMate.Services;
using StackExchange.Redis;
using CoolMate.Middleware;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkMySQL().AddDbContext<DBContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<SiteUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;

}).AddEntityFrameworkStores<DBContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = GoogleDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
}).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientId");
    googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientSecret");
}); 

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
        o.TokenLifespan = TimeSpan.FromMinutes(10));

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect($"{builder.Configuration["Redis:Host"]}, password = {builder.Configuration["Redis:Password"]}"));
builder.Services.AddScoped<IDatabase>(provider => provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddScoped<UriService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IProductItemRepository, ProductItemRepository>();
builder.Services.AddScoped<IShopOrderRepository, ShopOrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
    {
        options
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    }
    );
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
