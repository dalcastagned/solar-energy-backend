using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Data.Context;
using SolarEnergyApi.Domain.Interfaces;
using SolarEnergyApi.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SolarEnergyApi.Domain.Entities;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SolarPlantsCs");

builder.Services.AddDbContext<SolarPlantContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
    }
);

builder.Services.AddIdentityCore<User>(
    options =>
    {
        // configure identity options
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    }
);

builder.Services
    .AddIdentity<User, Role>()
    .AddEntityFrameworkStores<SolarPlantContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    )
    .AddJwtBearer(
        x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                        builder.Configuration.GetSection("AppSettings:Token").Value
                    )
                ),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    );

builder.Services.AddMvc(
    options =>
    {
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    }
);

builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IGenerationRepository, GenerationRepository>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IGenerationService, GenerationService>();

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            "AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
        );
    }
);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Solar Energy API",
            Version = "v1",
            Description = "Api to manage solar energy data",
            Contact = new OpenApiContact
            {
                Name = "Daniel Dalcastagne",
                Url = new Uri("https://www.linkedin.com/in/daniel-dalcastagne-4baa00179/")
            }
        });
        c.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below. \r\n\r\nExample: \"Bearer 12345abcdef\"",
            }
        );
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Solar Energy API");
    });
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.UseAuthentication();

app.Run();
