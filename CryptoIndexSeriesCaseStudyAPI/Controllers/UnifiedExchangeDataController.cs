using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoIndexSeriesCaseStudyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnifiedExchangeDataController : ControllerBase
    {

        private readonly ILogger<UnifiedExchangeDataController> _logger;

        public UnifiedExchangeDataController(ILogger<UnifiedExchangeDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {

            return Ok();
        }
    }
}
