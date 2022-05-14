using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Models.ApiModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocaleController : ControllerBase
    {
        private readonly ContextService _contextService;
        public LocaleController(ContextService contextService)
        {
            this._contextService = contextService;
        }

        [HttpGet]
        public IActionResult GetNowLocale()
        {
            return Ok(CultureModels.locale);
        }

        [HttpPost("Set/Response/Headers")]
        public IActionResult SetResponseHeaders([FromBody] List<SetResponseHeadersModel> request)
        {
            var test = new HeaderDictionary();
            var headers = request.ToDictionary(p => p.key, p => p.value);
            _contextService.SetResponseHeader(headers);

            return NoContent();
        }

        [HttpGet("Get/Request/Header/Locale")]
        public IActionResult GetRequestHeaderLocaleValue()
        {
            var locale = _contextService.GetRquestHeaderLocale();
            return Ok(locale);
        }
    }
}
