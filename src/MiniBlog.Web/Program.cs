using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using MiniBlog.Application.Interfaces;
using MiniBlog.Application.Mapping;
using MiniBlog.Application.Services;
using MiniBlog.Application.Validators;
using MiniBlog.Domain.Entities;
using MiniBlog.Domain.Interfaces;
using MiniBlog.Infrastructure.Auth;
using MiniBlog.Infrastructure.BackgroundJobs;
using MiniBlog.Infrastructure.Caching;
using MiniBlog.Infrastructure.Persistence;
using MiniBlog.Infrastructure.Repositories;
using MiniBlog.Web.Filters;
using MiniBlog.Web.Middleware;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog ----------
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();

// ---------- DbContext ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------- Identity ----------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ---------- JWT Auth ----------
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        // برای اینکه Razor Pages هم بتونه از کوکی استفاده کنه، توکن رو تو یه کوکی می‌ذاریم و اینجا می‌خونیم
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("access_token", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PostOwner", policy =>
        policy.RequireAssertion(context => true)); // منطق واقعی تو Resource-based Authorization در Controller چک می‌شه
});

// ---------- DI: Repositories ----------
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

// ---------- DI: Services + Decorator (Caching) ----------
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<IPostService>(sp =>
    new CachedPostService(sp.GetRequiredService<PostService>(), sp.GetRequiredService<IMemoryCache>()));
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ---------- Caching ----------
builder.Services.AddMemoryCache();

// ---------- Background Job ----------
builder.Services.AddHostedService<StatsLoggerService>();

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, typeof(MappingProfile).Assembly);

// ---------- FluentValidation ----------
builder.Services.AddValidatorsFromAssemblyContaining<CreatePostValidator>();
builder.Services.AddFluentValidationAutoValidation();

// ---------- MVC / Razor Pages + Filters ----------
builder.Services.AddScoped<LoggingActionFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<LoggingActionFilter>();
});
builder.Services.AddRazorPages();

builder.Services.Configure<Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions>(
    Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
    options => options.MapInboundClaims = false);

var app = builder.Build();

// ---------- Middleware Pipeline ----------
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();

// For Integration Test.
public partial class Program { }