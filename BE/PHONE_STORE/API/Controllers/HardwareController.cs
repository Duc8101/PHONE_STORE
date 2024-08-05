using API.Services.Hardware;
using Common.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HardwareController : BaseAPIController
    {
        private readonly IHardwareService _service;
        public HardwareController(IHardwareService service) 
        {
            _service = service;     
        }

        [HttpGet]
        public ResponseBase Info()
        {
            return _service.Info();
        }
    }
}
