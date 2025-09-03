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
                                       .Include(x => x.Marks)
                                       .FirstOrDefaultAsync(x => x.Id == id);
        }

        if (student is null)
            return Ok(new ResponseModel<IData, Error>()
            {
                Error = new Error("Student not found")
            });

        foreach(var item in student.Marks)
        {
            item.Student = null;
        }

        return Ok(new ResponseModel<GetStudentByIdData, IError>()
        {
            Data = new GetStudentByIdData(student)
        });
    }

    [HttpGet]
    [EnableCors]
    [Route("group/{id}")]
    public async Task<ActionResult<ResponseModel<GetStudentsByGroupIdData, IError>>> GetStudentsByGroupId(int id)
    {
        List<StudentDbModel> students = null;

        GroupDbModel? groupDbInfo = null;

        using (var db = await _dbContext.CreateDbContextAsync())
        {
            var query = db.Students.AsNoTracking()
                                   .Where(x => x.GroupFk == id);

            var now = DateTime.UtcNow;
            var startDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var endDay = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            students = await query.OrderByDescending(x => x.StudentType)
                                  .ThenBy(x => x.LastName)
                                  .ThenBy(x => x.FirstName)
                                  .ThenBy(x => x.MiddleName)
                                  .Include(x => x.Marks.Where(m => m.DateSet >= startDay && m.DateSet <= endDay))
                                  .ToListAsync();

            groupDbInfo = await db.Groups.AsNoTracking()
                                         .Include(x => x.StudentHead)
                                         .FirstOrDefaultAsync(x => x.Id == id);
        }

        List<StudentModel> result = new(students.Count);

        GroupInfoModel groupInfo = new(groupDbInfo);

        foreach (var item in students)
        {
            foreach(var mark in item.Marks)
            {
                mark.Student = null;
            }

            result.Add(new(item));
        }

        return Ok(new ResponseModel<GetStudentsByGroupIdData, IError>()
        {
            Data = new GetStudentsByGroupIdData(result, groupInfo)
        });
    }

    [HttpPost]
    [EnableCors]
    [Route("{id}/setMark/{markType}")]
    public async Task<ActionResult<ResponseModel<SetMarkData, IError>>> SetStudentsMark(int id, int markType)
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

        return Ok(new ResponseModel<SetMarkData, IError>()
        {
            Data = new SetMarkData(mark)
        });
    }

    [HttpPost]
    [EnableCors]
    [Route("{id}/updateMark/{markId}/{markType}")]
    public async Task<ActionResult<ResponseModel<SetMarkData, IError>>> UpdateStudentsMark(int id, int markId, int markType)
    {
        MarkDbModel? mark = null;

        using (var db = await _dbContext.CreateDbContextAsync())
        {
            mark = await db.Marks.FirstOrDefaultAsync(x => x.Id == markId);
            mark.MarkType = markType;
            await db.SaveChangesAsync();
        }

        return Ok(new ResponseModel<SetMarkData, IError>()
        {
            Data = new SetMarkData(mark)
        });
    }

    [HttpPost]
    [EnableCors]
    [Route("{id}/deleteMark/{markId}")]
    public async Task<ActionResult<ResponseModel<StatusData, IError>>> DeleteStudentsMark(int id, int markId)
    {
        using (var db = await _dbContext.CreateDbContextAsync())
        {
            var mark = await db.Marks.FirstOrDefaultAsync(x => x.Id == markId);
            db.Remove(mark);
            await db.SaveChangesAsync();
        }

        return Ok(new ResponseModel<StatusData, IError>()
        {
            Data = new StatusData("Ok")
        });
    }

    [HttpPut]
    [EnableCors]
    [Route("favourite/{id}")]
    public async Task<ActionResult<ResponseModel<StatusData, IError>>> UpdateFavouriteStudent(int id)
    {
        using (var db = await _dbContext.CreateDbContextAsync())
        {
            var student = await db.Students.FirstOrDefaultAsync(x => x.Id == id);
            student.StudentType = student.StudentType == 1 ? 0 : 1;
            await db.SaveChangesAsync();
        }

        return Ok(new ResponseModel<StatusData, IError>()
        {
            Data = new StatusData("Ok")
        });
    }

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
