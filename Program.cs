using ClinAgenda.Application.UseCases;
using ClinAgenda.Core.Interfaces;
using ClinAgenda.Infrastructure.Repositories;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<MySqlConnection>(_ => new MySqlConnection(connectionString));

builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<StatusUseCase>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<SpecialtyUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();