using Stars.API.Models;
using Stars.API.Helpers;
using Stars.API.Models.DbModels;
using Stars.API.Models.RequestModels;
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

        if (student is null)
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
    public async Task<ActionResult<ResponseModel<GetStudentsByGroupIdData, IError>>> GetStudentsByGroupId(int id, [FromQuery] string? search)
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

            var now = DateTime.UtcNow;

            students = await query.OrderBy(x => x.LastName)
                                  .ThenBy(x => x.FirstName)
                                  .ThenBy(x => x.MiddleName)
                                  .Include(x => x.Marks.Where(m => m.DateSet.Date == now.Date))
                                  .ToListAsync();
        }

        foreach (var item in students)
        {
            foreach (var mark in item.Marks)
            {
                mark.Student = null;
            }
        }

        return Ok(new ResponseModel<GetStudentsByGroupIdData, IError>()
        {
            Data = new GetStudentsByGroupIdData(students)
        });
    }

    [HttpPost]
    [EnableCors]
    [Route("{id}/setMark/{markType}")]
    public async Task<ActionResult<ResponseModel<StatusData, IError>>> SetStudentsMark(int id, int markType)
    {
        var mark = new MarkDbModel()
        {
            StudentFk = id,
            MarkType = markType,
            DateSet = DateTime.UtcNow
        };

        using (var db = await _dbContext.CreateDbContextAsync())
        {
            await db.Marks.AddAsync(mark);
            await db.SaveChangesAsync();
        }

        return Ok(new ResponseModel<StatusData, IError>()
        {
            Data = new StatusData("Ok")
        });
    }

    // TODO: updateMark

    [HttpPost]
    [EnableCors]
    [Route("update/{id}")]
    public async Task<ActionResult<ResponseModel<StatusData, IError>>> UpdateStudentInfo(int id, [FromBody] UpdateStudentInfoModel info)
    {
        StudentDbModel? student = null;

        using (var db = await _dbContext.CreateDbContextAsync())
        {
            student = await db.Students.FirstOrDefaultAsync(x => x.Id == id);

            student.FirstName = info.FirstName;
            student.LastName = info.LastName;
            student.MiddleName = info.MiddleName;

            await db.SaveChangesAsync();
        }

        return Ok(new ResponseModel<StatusData, IError>()
        {
            Data = new StatusData("Ok")
        });
    }
}
