using GA4IntegrationApplication.Api.Helpers;
using GA4IntegrationApplication.Api.Infrastructure;
using GA4IntegrationApplication.Api.Interfaces;
using GA4IntegrationApplication.Api.Model;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var env = builder.Environment;
builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

builder.Services.Configure<GoogleAnalyticConfigModel>(builder.Configuration.GetSection("GoogleAnalytic"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGoogleAnalytic, GoogleAnalytic>();
//AddTransient:Uygulama içerisinde bağımlılık olarak oluşturduğumuz ve kullandığımız nesnenin her kullanım ve çağrıda tekrardan oluşturulmasını sağlar.
//AddSingleton:Uygulama içerisinde bağımlılık oluşturduğumuz ve kullandığımız nesnenin tek bir sefer oluşturulmasını ve aynı nesnenin uygulama içinde kullanılmasını sağlar.
//AddScoped:Uygulama içerisindeki bağımlılık oluşturduğumu nesnenin request sonlanana kadar aynı nesneyi kullanmasını farklı bir çağrı için gelindiğinde yeni bir nesne yaratılmasını sağlar.
builder.Services.AddHangfire(h =>
                   h.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseDashboardMetric(DashboardMetrics.ServerCount)
                    .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                    .UseDashboardMetric(DashboardMetrics.RetriesCount)
                    .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                    .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                    .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                    .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                    .UseDashboardMetric(DashboardMetrics.SucceededCount)
                    .UseDashboardMetric(DashboardMetrics.FailedCount)
                    .UseDashboardMetric(DashboardMetrics.DeletedCount)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    .UseMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/job", new DashboardOptions
{
    Authorization = new[]
{
    new HangfireCustomBasicAuthenticationFilter
    {
         User = builder.Configuration.GetSection("HangfireSettings:UserName").Value,
         Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
    }
    }
});
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });// ilgili job başarılı şekilde çalıştırılamaz ise, hata alınılmayıncaya kadar 3 defa tekrar edilmektedir.
BackgroundTasks.GetJobs();


app.MapControllers();

app.Run();
