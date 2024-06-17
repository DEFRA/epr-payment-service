using Asp.Versioning;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Extension;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.ResponseWriter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string _environmentName = builder.Configuration.GetValue<string>("EnvironmentName") ?? "LOCAL"; ;

bool IsEnvironmentLocalOrDev =
    _environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
    || _environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase);

// Add services to the container.

builder.Services.AddControllers();
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

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<PaymentDataContext>()
    //.AddCheck<AccreditationFeesHealthCheck>(AccreditationFeesHealthCheck.HealthCheckResultDescription,
    //        failureStatus: HealthStatus.Unhealthy,
    //        tags: new[] { "ready" }); 
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

var featureManager = app.Services.GetRequiredService<IFeatureManager>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

bool enablePaymentsFeature = await featureManager.IsEnabledAsync("EnablePaymentsFeature");
bool enablePaymentStatusInsert = await featureManager.IsEnabledAsync("EnablePaymentStatusInsert");

logger.LogInformation($"EnablePaymentsFeature: {enablePaymentsFeature}");
logger.LogInformation($"EnablePaymentStatusInsert: {enablePaymentStatusInsert}");

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