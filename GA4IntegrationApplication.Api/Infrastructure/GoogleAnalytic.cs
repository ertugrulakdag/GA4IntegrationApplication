using GA4IntegrationApplication.Api.Helpers;
using GA4IntegrationApplication.Api.Interfaces;
using GA4IntegrationApplication.Api.Model;
using Google.Analytics.Data.V1Beta;
using Microsoft.Extensions.Options;
using Metric = Google.Analytics.Data.V1Beta.Metric;

namespace GA4IntegrationApplication.Api.Infrastructure
{
    public class GoogleAnalytic : IGoogleAnalytic
    {
        private readonly BetaAnalyticsDataClient _client;
        private readonly GoogleAnalyticConfigModel _config;

        public GoogleAnalytic(IOptions<GoogleAnalyticConfigModel> options)
        {
            _config = options.Value;
            _client = new BetaAnalyticsDataClientBuilder
            {
                CredentialsPath = _config.CredentialsPath
            }.Build();
        }

        public object ScreenPageViews(DateTime StartDate, DateTime EndDate)
        {

            RunReportRequest request = new RunReportRequest
            {
                Property = _config.PropertyIds,
                Metrics = { new Metric { Name = "screenPageViews" } },
                DateRanges =
            {
                new DateRange
                {
                    StartDate = StartDate.ExtToStartOfDay().ToString("yyyy-MM-dd"),
                    EndDate = EndDate.ExtToEndOfDay().ToString("yyyy-MM-dd")
                },
            },
            };
            RunReportResponse response = _client.RunReport(request);
            return response;
        }

        public object TotalUsers(DateTime StartDate, DateTime EndDate)
        {
            RunReportRequest request = new RunReportRequest
            {
                Property = _config.PropertyIds,
                Metrics = { new Metric { Name = "totalUsers" } },
                DateRanges =
            {
                new DateRange
                {
                    StartDate = StartDate.ToString("yyyy-MM-dd"),
                    EndDate = EndDate.ToString("yyyy-MM-dd")
                },
            },
            };
            RunReportResponse response = _client.RunReport(request);
            return response;
        }

        public object ActiveUsers(DateTime StartDate, DateTime EndDate)
        {
            RunReportRequest request = new RunReportRequest
            {
                Property = _config.PropertyIds,
                Metrics = { new Metric { Name = "activeUsers" } },
                DateRanges =
            {
                new DateRange
                {
                    StartDate = StartDate.ToString("yyyy-MM-dd"),
                    EndDate = EndDate.ToString("yyyy-MM-dd")
                },
            },
            };
            RunReportResponse response = _client.RunReport(request);
            return response;
        }

        public object Realtime()
        {
            RunRealtimeReportRequest request = new RunRealtimeReportRequest
            {
                Property = _config.PropertyIds,
                Metrics = { new Metric { Name = "activeUsers" } },
                Dimensions = { new Dimension { Name = "country" }, new Dimension { Name = "city" } }

            };
            RunRealtimeReportResponse response = _client.RunRealtimeReport(request);
            return response;
        }
    }
}
