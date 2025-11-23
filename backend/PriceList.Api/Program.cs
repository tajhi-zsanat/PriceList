using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Services;
using PriceList.Infrastructure.Auth;
using PriceList.Infrastructure.Data;
using PriceList.Infrastructure.Identity;
using PriceList.Infrastructure.Repositories.Ef;
using PriceList.Infrastructure.Services;
using PriceList.Infrastructure.Services.Storage;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ⬇️ Read origin for CORS from config (env var OK)
var webOrigin = builder.Configuration["Web:Origin"] ?? "http://localhost:5173";

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
     .EnableDetailedErrors()
     .LogTo(Console.WriteLine, LogLevel.Information)
);

// Identity
builder.Services
    .AddIdentityCore<AppUser>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequiredLength = 6;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

// JWT
var jwt = builder.Configuration.GetSection("Jwt");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });

// CORS (allow React dev server localhost:5173)
//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy(CorsPolicy, policy =>
//        policy.WithOrigins(webOrigin) // ⬅️ now configurable
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials());
//});

const string CorsPolicy = "FrontendPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p => p
        .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddControllers();

builder.Services.AddScoped<ITokenService, TokenService>();

// Needed for Swagger to find endpoints
builder.Services.AddEndpointsApiExplorer();

// Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PriceList API",
        Version = "v1",
        Description = "Backend for PriceList (React + .NET)"
    });

    // 🔐 Tell Swagger about "Bearer" scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// ✅ DI: interfaces (Core) → implementations (Infrastructure)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IFormCellRepository, FormCellRepository>();
builder.Services.AddScoped<IFormColumnDefRepository, FormColumnDefRepository>();
builder.Services.AddScoped<IFormRowRepository, FormRowRepository>();
builder.Services.AddScoped<IFormFeatureRepository, FormFeatureRepository>();

//Service
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Decide where files live (under wwwroot/uploads)
var webRoot = builder.Environment.WebRootPath
              ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
var uploadsPhysicalRoot = Path.Combine(webRoot, "uploads");
builder.Services.Configure<FileStorageOptions>(opt =>
{
    opt.PhysicalRoot = uploadsPhysicalRoot; 
    opt.RequestPath = "/uploads";         
});
builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();

var app = builder.Build();

// ✅ Auto-apply EF migrations on startup (no CLI step needed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Serve JSON and UI
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "PriceList API v1");
        o.RoutePrefix = "swagger"; 
    });
}

app.UseMiddleware<PriceList.Api.Middlewares.ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
