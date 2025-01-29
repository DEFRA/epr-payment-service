using Asp.Versioning;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Extension;
using EPR.Payment.Service.HealthCheck;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Validations.Payments;
using EPR.Payment.Service.Validations.RegistrationFees.Producer;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string _environmentName = builder.Configuration.GetValue<string>("EnvironmentName") ?? "LOCAL";

bool IsEnvironmentLocalOrDev =
    _environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
    || _environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<OnlinePaymentInsertRequestDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<ProducerRegistrationFeesRequestDtoValidator>();
    fv.AutomaticValidationEnabled = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.EnableAnnotations();
    setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentServiceApi", Version = "v1" });
    setupAction.DocumentFilter<FeatureEnabledDocumentFilter>();
    setupAction.OperationFilter<FeatureGateOperationFilter>();
});
builder.Services.AddDependencies();
builder.Services.AddDataContext(builder.Configuration["ConnectionStrings:PaymentConnectionString"]!);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHealthChecks();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddFeatureManagement();

var app = builder.Build();

// Apply pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations:");
            foreach (var migration in pendingMigrations)
            {
                Console.WriteLine($"- {migration}");
            }
            dbContext.Database.Migrate();
        }
        else
        {
            Console.WriteLine("No pending migrations.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

var featureManager = app.Services.GetRequiredService<IFeatureManager>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

bool enableOnlinePaymentsFeature = await featureManager.IsEnabledAsync("EnableOnlinePaymentsFeature");
bool enableOnlinePaymentInsert = await featureManager.IsEnabledAsync("EnableOnlinePaymentInsert");
bool enableOnlinePaymentUpdate = await featureManager.IsEnabledAsync("EnableOnlinePaymentUpdate");
bool enableGetOnlinePaymentByExternalPaymentId = await featureManager.IsEnabledAsync("EnableGetOnlinePaymentByExternalPaymentId");

bool enableRegistrationFeesFeature = await featureManager.IsEnabledAsync("EnableRegistrationFeesFeature");
bool enableProducerResubmissionAmount = await featureManager.IsEnabledAsync("EnableProducerResubmissionAmount");
bool enableRegistrationFeesCalculation = await featureManager.IsEnabledAsync("EnableRegistrationFeesCalculation");

logger.LogInformation("EnableOnlinePaymentsFeature: {EnableOnlinePaymentsFeature}", enableOnlinePaymentsFeature);
logger.LogInformation("EnableOnlinePaymentInsert: {EnableOnlinePaymentInsert}", enableOnlinePaymentInsert);
logger.LogInformation("EnableOnlinePaymentUpdate: {EnableOnlinePaymentUpdate}", enableOnlinePaymentUpdate);
logger.LogInformation("EnableGetOnlinePaymentByExternalPaymentId: {EnableGetOnlinePaymentByExternalPaymentId}", enableGetOnlinePaymentByExternalPaymentId);


logger.LogInformation("EnableRegistrationFeesFeature: {EnableRegistrationFeesFeature}", enableRegistrationFeesFeature);
logger.LogInformation("EnableProducerResubmissionAmount: {EnableProducerResubmissionAmount}", enableProducerResubmissionAmount);
logger.LogInformation("EnableRegistrationFeesCalculation: {EnableRegistrationFeesCalculation}", enableRegistrationFeesCalculation);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentServiceApi v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<ConditionalEndpointMiddleware>();

app.MapControllers();

app.MapHealthChecks("/admin/health", HealthCheckOptionsBuilder.Build()).AllowAnonymous();
app.Run();