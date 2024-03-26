using Microsoft.AspNetCore.Mvc;

namespace Liyanjie.AspNetCore.Extensions.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpPost]
        public object Get(
            [FromQuery, DelimitedArray] Guid[] abc,
            [FromForm] Model model)
        {
            return new
            {
                abc,
                model,
            };
        }
    }

    public class Model
    {
        [ModelBinder<DelimitedArrayModelBinder>]
        public required Guid[] Ids { get; set; }
    }
}
