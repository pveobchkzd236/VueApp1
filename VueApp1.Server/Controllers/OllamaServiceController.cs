using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Helpers;
using static VueApp1.Server.Helpers.OllamaApiHelper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VueApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OllamaServiceController : ControllerBase
    {

        private readonly ILogger<OllamaServiceController> _logger;

        public OllamaServiceController(ILogger<OllamaServiceController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 调用ollama chat方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "chat")]
        public Task<ChatResponse> ChatAsync([FromBody] ChatRequest request)
        {
            return OllamaApiHelper.ChatAsync(request);
        }
    }
}
