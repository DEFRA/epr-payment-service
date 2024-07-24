using Asp.Versioning;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Extension;
using EPR.Payment.Service.HealthCheck;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.ResponseWriter;
using EPR.Payment.Service.Validations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
    fv.RegisterValidatorsFromAssemblyContaining<PaymentStatusInsertRequestDtoValidator>();
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
builder.Services.AddDataContext(builder.Configuration.GetConnectionString("PaymentConnnectionString")!);
builder.Services.AddApplicationInsightsTelemetry();
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddCheck<PaymentStatusHealthCheck>(PaymentStatusHealthCheck.HealthCheckResultDescription,
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "ready" }); 
;

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
builder.Services.AddLogging();

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

bool enablePaymentsFeature = await featureManager.IsEnabledAsync("EnablePaymentsFeature"); 
bool enablePaymentStatusInsert = await featureManager.IsEnabledAsync("EnablePaymentStatusInsert");
bool enablePaymentStatusUpdate = await featureManager.IsEnabledAsync("EnablePaymentStatusUpdate");

logger.LogInformation($"EnablePaymentsFeature: {enablePaymentsFeature}");
logger.LogInformation($"EnablePaymentStatusInsert: {enablePaymentStatusInsert}");
logger.LogInformation($"EnablePaymentStatusUpdate: {enablePaymentStatusUpdate}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentServiceApi v1");
    c.RoutePrefix = "swagger";
});
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
    });

if (!IsEnvironmentLocalOrDev)
    app.UseHealthChecks("/ping",
        new HealthCheckOptions
        {
            Predicate = _ => false,
            ResponseWriter = (context, report) =>
            {
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("");
            }
        });

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<ConditionalEndpointMiddleware>();

app.MapControllers();
//app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();