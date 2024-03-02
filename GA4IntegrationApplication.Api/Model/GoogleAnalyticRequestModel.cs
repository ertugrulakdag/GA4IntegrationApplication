using System.ComponentModel;

namespace GA4IntegrationApplication.Api.Model
{
    public class GoogleAnalyticRequestModel
    {
        [DefaultValue("2024-02-26")]
        public DateTime StartDate { get; set; }
        [DefaultValue("2024-03-01")]
        public DateTime EndDate { get; set; }
    }
}
