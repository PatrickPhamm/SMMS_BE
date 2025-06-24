using Microsoft.EntityFrameworkCore;
using Smmsbe.Repositories;
using Smmsbe.Repositories.Entities;
using Smmsbe.Repositories.Interfaces;
using Smmsbe.Services;
using Smmsbe.Services.Common;
using Smmsbe.Services.Interfaces;
using Smmsbe.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Start DI
builder.Services.AddSingleton<IHashHelper, HashHelper>();

// AppSettings
var appSettings = new AppSettings
{
    ApplicationUrl = builder.Configuration["AppSettings:ApplicationUrl"],
    LandingPageUrl = builder.Configuration["AppSettings:LandingPageUrl"],
    EmailSettings = new EmailSettings
    {
        SmtpServer = builder.Configuration["AppSettings:EmailSettings:SmtpServer"],
        SmtpPort = int.Parse(builder.Configuration["AppSettings:EmailSettings:SmtpPort"]),
        SenderEmail = builder.Configuration["AppSettings:EmailSettings:SenderEmail"],
        Password = builder.Configuration["AppSettings:EmailSettings:Password"],
        SenderName = builder.Configuration["AppSettings:EmailSettings:SenderName"],
        Username = builder.Configuration["AppSettings:EmailSettings:Username"]
    }
};


builder.Services.AddSingleton(appSettings);
builder.Services.AddSingleton<IEmailHelper>(new EmailHelper(appSettings));

var cnnString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SMMSContext>((optionBuilder) =>
{
    optionBuilder.UseSqlServer(cnnString);
});

builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<INurseRepository, NurseRepository>();
builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<IParentRepository, ParentRepository>();
builder.Services.AddScoped<IParentService, ParentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IMedicalInventoryRepository, MedicalInventoryRepository>();
builder.Services.AddScoped<IMedicalInventoryService, MedicalInventoryService>();
builder.Services.AddScoped<IVaccinationScheduleRepository, VaccinationScheduleRepository>();
builder.Services.AddScoped<IVaccinationScheduleService, VaccinationScheduleService>();
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IConsentFormRepository, ConsentFormRepository>();
builder.Services.AddScoped<IConsentFormService, ConsentFormService>();
builder.Services.AddScoped<IConsultationFormRepository, ConsultationFormRepository>();
builder.Services.AddScoped<IConsultationFormService, ConsultationFormService>();
builder.Services.AddScoped<IConsultationScheduleRepository, ConsultationScheduleRepository>();
builder.Services.AddScoped<IConsultationScheduleService, ConsultationScheduleService>();
builder.Services.AddScoped<IParentPrescriptionRepository, ParentPrescriptionRepository>();
builder.Services.AddScoped<IParentPrescriptionService, ParentPrescriptionService>();
builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<IMedicalEventRepository, MedicalEventRepository>();
builder.Services.AddScoped<IMedicalEventService, MedicalEventService>();
builder.Services.AddScoped<IVaccinationResultRepository, VaccinationResultRepository>();
builder.Services.AddScoped<IVaccinationResultService, VaccinationResultService>();
builder.Services.AddScoped<IHealthCheckResultRepository, HealthCheckResultRepository>();
builder.Services.AddScoped<IHealthCheckResultService, HealthCheckResultService>();
builder.Services.AddScoped<IHealthCheckupScheduleRepository, HealthCheckupScheduleRepository>();
builder.Services.AddScoped<IHealthCheckScheduleService, HealthCheckScheduleService>();
builder.Services.AddScoped<IHealthProfileRepository, HealthProfileRepository>();
builder.Services.AddScoped<IHealthProfileService, HealthProfileService>();

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

// Use CORS
app.UseCors("AllowFrontend");

app.MapControllers();

//app.Run();
await app.RunAsync();

