using Stars.API.Models;
using Stars.API.Helpers;
using Stars.API.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace Stars.API.Controllers;

[Route("api/students")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IDbContextFactory<StarsDbContext> _dbContext;

    public StudentsController(IDbContextFactory<StarsDbContext> dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [EnableCors]
    [Route("{id}")]
    public async Task<ActionResult<ResponseModel<GetStudentByIdData, IError>>> GetById(int id)
    {
        StudentDbModel? student = null;

        using (var db = await _dbContext.CreateDbContextAsync())
        {
            student = await db.Students.AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == id);
        }

        if(student is null)
            return Ok(new ResponseModel<IData, Error>()
            {
                Error = new Error("Student not found")
            });

        return Ok(new ResponseModel<GetStudentByIdData, IError>()
        {
            Data = new GetStudentByIdData(student)
        });
    }

    [HttpGet]
    [EnableCors]
    [Route("group/{id}")]
    public async Task<ActionResult<ResponseModel<GetStudentsByGroupIdData, IError>>> GetStudentsByGroupId(int id, [FromQuery]string? search)
    {
        List<StudentDbModel> students = null;

        using (var db = await _dbContext.CreateDbContextAsync())
        {
            var query = db.Students.AsNoTracking()
                                   .Where(x => x.GroupFk == id);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.LastName.Contains(search) ||
                                         x.FirstName.Contains(search) ||
                                         x.MiddleName.Contains(search));
            }

            students = await query.OrderBy(x => x.LastName)
                                  .ThenBy(x => x.FirstName)
                                  .ThenBy(x => x.MiddleName)
                                  .ToListAsync();
        }

        return Ok(new ResponseModel<GetStudentsByGroupIdData, IError>()
        {
            Data = new GetStudentsByGroupIdData(students)
        });
    }
}
