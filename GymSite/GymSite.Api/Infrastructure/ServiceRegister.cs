﻿using GymSite.Application.Auth;
using GymSite.Database;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GymSite.Api.Infrastructure
{
    public static class ServiceRegister
    {
        public static void RegisterServices(this IServiceCollection @this)
        {
            typeof(ApplicationDbContext).Assembly.RegisterImplementations(@this);
            typeof(AuthService).Assembly.RegisterImplementations(@this);
        }

        private static void RegisterImplementations(this Assembly @this, IServiceCollection services)
        {
            var implementations = @this.DefinedTypes.
                Where(type => type.GetTypeInfo().GetCustomAttribute<Implementation>() is not null);

            foreach(var implementation in implementations)
            {
                var attribute = implementation.GetCustomAttribute<Implementation>();

                services.AddScoped(attribute.Type, implementation);
            }
        }

        public static void ConfigureSwagger(this IServiceCollection @this) 
        {
            @this.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Vikop API",
                    Description = "API trying to mirror wykop.pl"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static void ConfigureIdentity(this IServiceCollection @this)
        {
            @this.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
