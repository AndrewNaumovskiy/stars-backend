using Stars.API.Models;
using Stars.API.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Stars.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [EnableCors]
        [Route("login")]
        public async Task<ActionResult<ResponseModel<LoginData, IError>>> Login([FromBody] LoginRequestModel body)
        {
            if(body.Login == "cum" && body.Password == "cum")
            {
                return Ok(new ResponseModel<LoginData, IError>()
                {
                    Data = new LoginData("kekekekekek")
                });
            }

            return Ok(new ResponseModel<IData, Error>()
            {
                Error = new Error("Wrong password")
            });
        }
    }
}
