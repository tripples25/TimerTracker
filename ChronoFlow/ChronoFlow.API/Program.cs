using ChronoFlow.API.DAL;
using ChronoFlow.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var module = new ApplicationModule(builder.Configuration);
module.RegisterModule(builder.Services);

//смотреть Module

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