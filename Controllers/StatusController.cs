using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private static readonly DateTime StartTime = DateTime.UtcNow;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                service = "Store Backend API",
                status = "Running",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                startedAt = StartTime,
                uptime = (DateTime.UtcNow - StartTime).ToString(@"hh\:mm\:ss"),
                ip = GetLocalIp()
            });
        }

        private string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList
                .FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                .ToString() ?? "Unknown";
        }
    }
}
