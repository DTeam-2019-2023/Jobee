using Jobee_API;
using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Jobee_API.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Project_JobeeContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DBJobee")));

var mailsetting = builder.Configuration.GetSection(nameof(MailSettings));
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddTransient<IEmailService, SendMailService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        RequireExpirationTime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true,
                        ValidateActor = true,
                        ValidateLifetime = true,
                    };
                    options.ForwardSignOut = "/";
                });

    builder.Services.AddDistributedMemoryCache();
var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    JwtTokenManager.SecretKey = builder.Configuration["Jwt:Key"];
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleWare>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();

app.Run();
