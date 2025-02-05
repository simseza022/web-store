using Microsoft.AspNetCore.Mvc;

namespace ShopCartApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DummyController
    {

        [HttpGet("dummy")]
        public int DummyEndpoint(){
            return 1;
        }
    }
}