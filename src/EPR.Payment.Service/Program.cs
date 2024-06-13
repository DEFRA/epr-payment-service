using Asp.Versioning;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Extension;
using EPR.Payment.Service.ResponseWriter;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string _environmentName = builder.Configuration.GetValue<string>("EnvironmentName") ?? "LOCAL"; ;

bool IsEnvironmentLocalOrDev =
    _environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
    || _environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies();
builder.Services.AddDataContext(builder.Configuration.GetConnectionString("PaymentConnnectionString")!);

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<DataContext>()
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();
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

app.MapControllers();
//app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();