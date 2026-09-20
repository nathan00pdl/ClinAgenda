using ClinAgenda.Application.UseCases;
using ClinAgenda.Core.Interfaces;
using ClinAgenda.Infrastructure.Repositories;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? throw new InvalidOperationException("AllowedOrigins is not configured.");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)  // the frontend's dev server, from appsettings.json
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
if (connectionString != null)
{
    connectionString = connectionString
        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));
}
else
{
    throw new InvalidOperationException("SQL connection string is not configured.");
}

builder.Services.AddSingleton(connectionString);

builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<StatusUseCase>();

builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<SpecialtyUseCase>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<PatientUseCase>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<DoctorUseCase>();

builder.Services.AddScoped<IDoctorSpecialtyRepository, DoctorSpecialtyRepository>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<AppointmentUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();