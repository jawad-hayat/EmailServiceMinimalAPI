using EmailServiceAPI.Models;
using EmailServiceAPI.Models.DTOs;
using EmailServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var emailConfig = new EmailConfiguration
{
    SmtpServer = builder.Configuration["Smtp:Server"],
    Port = builder.Configuration["Smtp:Port"],
    Username = builder.Configuration["Smtp:Username"],
    Password = builder.Configuration["Smtp:Password"]
};
builder.Services.AddSingleton(emailConfig);
builder.Services.AddTransient<IEmailService, EmailService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/sendemail", async (IEmailService emailService, [FromBody] EmailRequest request) =>
{
    var result = await emailService.SendEmailAsync(request.To, request.Subject, request.Body);
    var res = new Response();
    if (result)
    {
        res = new Response
        {
            Success = true,
            Result = "Email sent successfully!"
        };
    }
    else
    {
        res = new Response
        {
            Success = false,
            Result = "Failed to sent email!"
        };
    }
    return res;
})
.Produces(200)
.WithName("SendEmail");

app.Run();

