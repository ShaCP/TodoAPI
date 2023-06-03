using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Converters;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;

namespace TodoApi.Configuration
{
    public static class ConfigureServices
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            // todo: change cors to more secure policy
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Dev", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Configure Entity Framework Core
            services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TodoConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TodoDbContext>().AddDefaultTokenProviders();
            
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            if (jwtSettings == null ||
            string.IsNullOrEmpty(jwtSettings.Key) ||
            string.IsNullOrEmpty(jwtSettings.Issuer) ||
            string.IsNullOrEmpty(jwtSettings.Audience))
            {
                throw new InvalidOperationException("JWT settings are missing or incomplete in the configuration.");
            }

            services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ITodoItemService, TodoItemService>();
            services.AddScoped<ITodoItemConverter, TodoItemConverter>();
        }

    }
}