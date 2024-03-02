using GA4IntegrationApplication.Api.Interfaces;
using GA4IntegrationApplication.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace GA4IntegrationApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAnalyticController : ControllerBase
    {
        private readonly IGoogleAnalytic _googleAnalytic;
        private readonly ILogger<GoogleAnalyticController> _logger;
        public GoogleAnalyticController(IGoogleAnalytic googleAnalytic, ILogger<GoogleAnalyticController> logger)
        {
            _googleAnalytic = googleAnalytic;
            _logger = logger;
        }

        [HttpGet]
        [Route("ScreenPageViews")]
        public ActionResult ScreenPageView([FromQuery] GoogleAnalyticRequestModel request)
        {
            var response = _googleAnalytic.ScreenPageViews(request.StartDate, request.EndDate);
            return Ok(response);
        }
        [HttpGet]
        [Route("TotalUsers")]
        public ActionResult TotalUsers([FromQuery] GoogleAnalyticRequestModel request)
        {
            var response = _googleAnalytic.TotalUsers(request.StartDate, request.EndDate);
            return Ok(response);
        }

        [HttpGet]
        [Route("ActiveUsers")]
        public ActionResult ActiveUsers([FromQuery] GoogleAnalyticRequestModel request)
        {
            var response = _googleAnalytic.ActiveUsers(request.StartDate, request.EndDate);
            return Ok(response);
        }

        [HttpGet]
        [Route("Realtime")]
        public ActionResult Realtime()
        {
            var response = _googleAnalytic.Realtime( );
            return Ok(response);
        }
    }
}
