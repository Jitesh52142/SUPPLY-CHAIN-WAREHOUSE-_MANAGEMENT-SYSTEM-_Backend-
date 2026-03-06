using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Config;
using warehouse_management_system.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using warehouse_management_system.Modules.ROL.Interfaces;
using warehouse_management_system.Infrastructure.BackgroundJobs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ============================
// DATABASE
// ============================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================
// PROJECT SERVICES
// ============================
builder.Services.AddProjectServices();

// ============================
// CORS (For Angular + Swagger)
// ============================
builder.Services.AddCors(options =>
{
    // Existing policy
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

    // Angular policy
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// ============================
// AUTHENTICATION (JWT + ROLE SUPPORT)
// ============================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!)),

            RoleClaimType = ClaimTypes.Role
        };
    });

// ============================
// CONTROLLERS + SWAGGER (JWT Support)
// ============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token here"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ============================
// HANGFIRE SETUP
// ============================
builder.Services.AddHangfireServices(builder.Configuration);
builder.Services.AddHangfireServer();

// Register Escalation Job
builder.Services.AddScoped<SLAEscalationJob>();

// ============================
// BUILD
// ============================
var app = builder.Build();

// ============================
// PIPELINE
// ============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply Angular CORS
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireServices();

app.MapControllers();

// ============================
// 🔥 RECURRING BACKGROUND JOBS
// ============================

// ROL Check (Every 15 Minutes)
RecurringJob.AddOrUpdate<IRolService>(
    "rol-check-job",
    x => x.CheckStockAsync(),
    "*/15 * * * *");

// SLA Escalation Check (Every 15 Minutes)
RecurringJob.AddOrUpdate<SLAEscalationJob>(
    "approval-escalation-job",
    x => x.CheckAndEscalateAsync(),
    "*/15 * * * *");

app.Run();