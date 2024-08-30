using Stars.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Stars.API.Controllers;

[Route("api/students")]
[ApiController]
public class StudentController : ControllerBase
{
    [HttpGet]
    [EnableCors]
    [Route("{id}")]
    public async Task<ActionResult<ResponseModel<GetStudentByIdData, IError>>> GetById(int id)
    {

    }
}
