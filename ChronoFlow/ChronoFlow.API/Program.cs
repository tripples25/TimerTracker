using ChronoFlow.API.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var module = new Module(builder.Configuration);
module.RegisterModules(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();