using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using ProyectoQ3Backend.Services;

var builder = WebApplication.CreateBuilder(args);

/**
 * Una sola instancia para toda la vida de ejecucion de la app
 */
builder.Services.AddSingleton<FirebaseService>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<NoteServices>();

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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        };
    });
    
    builder.Services.AddAuthorization();

    builder.Services.AddCors(options => 
    {
      options.AddPolicy("AllowAll", policy =>
        {
          policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
        });
     });  
   
    var app = builder.Build();