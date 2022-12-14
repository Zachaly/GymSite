using GymSite.Api.Infrastructure;
using GymSite.Application.Commands;
using GymSite.Database;
using GymSite.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration["DefaultConnection"]));

builder.Services.ConfigureIdentity();

builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]);
    var key = new SymmetricSecurityKey(bytes);

    config.SaveToken = true;
    config.RequireHttpsMetadata = false;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = key,
        ValidIssuer = builder.Configuration["Auth:Issuer"],
        ValidAudience = builder.Configuration["Auth:Audience"],
    };
});

builder.Services.RegisterServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services.AddMediatR(typeof(LoginCommand));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
});

var app = builder.Build();

try
{
    // Creating admin user if one does not exist
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        context.Database.EnsureCreated();

        var createAdmin = !context.UserClaims.Any(x => x.ClaimValue == "Admin");

        if (createAdmin)
        {
            Console.WriteLine("Admin user does not exist!");
            Console.WriteLine("Do you want to create it? y/n");
            var response = Console.ReadLine().ToLower();
            createAdmin = response == "y" || response == "yes";
        }

        if (createAdmin)
        {
            Console.WriteLine();
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Nickname: ");
            var nick = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            var admin = new ApplicationUser
            {
                UserName = username,
                NickName = nick
            };

            userManager.CreateAsync(admin, password).GetAwaiter().GetResult();

            var adminClaim = new Claim("Role", "Admin");

            userManager.AddClaimAsync(admin, adminClaim).GetAwaiter().GetResult();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:5002");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
