using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Infrastructure.Data;
using PriceList.Infrastructure.Repositories;
using PriceList.Infrastructure.Repositories.Ef;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS (allow React dev server localhost:5173)
const string CorsPolicy = "FrontendPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddControllers();

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

    // OPTIONAL: enable annotations
    // c.EnableAnnotations();

    // OPTIONAL: XML comments (see step 3)
    // var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xml), true);

    // OPTIONAL: JWT bearer support (uncomment when you add auth)
    // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    //   Name = "Authorization",
    //   Type = SecuritySchemeType.Http,
    //   Scheme = "bearer",
    //   BearerFormat = "JWT",
    //   In = ParameterLocation.Header,
    //   Description = "Enter: Bearer {token}"
    // });
    // c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    //   { new OpenApiSecurityScheme { Reference = new OpenApiReference {
    //         Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, Array.Empty<string>() }
    // });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// ✅ DI: interfaces (Core) → implementations (Infrastructure)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Serve JSON and UI
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "PriceList API v1");
        o.RoutePrefix = "swagger"; // UI at /swagger
    });
}

app.UseMiddleware<PriceList.Api.Middlewares.ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
