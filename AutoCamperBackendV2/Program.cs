global using AutoCamperBackendV2.Models;
using AutoCamperBackendV2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

public class program
{
    public static void Main(string[] args)
    {
        // Build the connection string
        //string currentDirectory = Environment.CurrentDirectory;
        //string connectionString = File.ReadAllText(Path.Combine(currentDirectory, "../DataBaseCredentials.txt"));
        //string[] result = connectionString.Split(';');
        //string ServerAddress = result[0];
        //string DatabaseName = result[1];
        //string UserID = result[2];
        //string Password = result[3];

        string ServerAddress = "REDACTED";
        string DatabaseName = "REDACTED";
        string UserID = "REDACTED";
        string Password = "REDACTED";

        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;



        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(policyBuilder =>
          policyBuilder.AddDefaultPolicy(policy =>
          {
              policy.AllowAnyOrigin();
              policy.AllowAnyHeader();
              policy.AllowAnyMethod();
          })
       );

        builder.Services.AddDbContext<ParkInPeaceProjectContext>(options =>
        {
            //options.UseSqlServer($"Server={ServerAddress};database={DatabaseName};user id={UserID};password={Password};trusted_connection=true;TrustServerCertificate=True;integrated security=false;");
            options.UseSqlServer($"Server={ServerAddress};database={DatabaseName};user id={UserID};password={Password};trusted_connection=true;TrustServerCertificate=True;integrated security=false;");
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}




