using Microsoft.AspNetCore.Authentication.Cookies;
using ChronoFlow.API.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var module = new ApplicationModule(builder.Configuration);
module.RegisterModules(builder.Services);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = (context) =>
        {
            context.Response.StatusCode = 401; // обработать ещё 403 ошибку
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();