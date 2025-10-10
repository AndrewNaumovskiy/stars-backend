using Stars.API.Models;
using Stars.API.Helpers;
using Stars.API.Models.DbModels;
using Stars.API.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace Stars.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;
        private readonly IDbContextFactory<StarsDbContext> _dbContext;

        public GroupsController(IDbContextFactory<StarsDbContext> dbContext, ScheduleService scheduleService)
        {
            _dbContext = dbContext;
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [EnableCors]
        [Route("")]
        public async Task<ActionResult<ResponseModel<GetGroupsData, IError>>> GetGroups()
        {
            List<GroupByDayModel> result = new();

            var now = DateTime.UtcNow;
            int dayNow = (int)now.DayOfWeek; // Wednesday = 3, Thursday = 4

            DateTime dateOfWednesday;

            if (dayNow > 3 && dayNow < 7)
            {
                var daysOfPreviousWednesday = dayNow - 3;
                dateOfWednesday = now.AddDays(-daysOfPreviousWednesday);
            }
            // next date
            else if (dayNow < 3)
            {
                var daysOfNextWednesday = 3 - dayNow;
                dateOfWednesday = now.AddDays(daysOfNextWednesday);
            }
            else
            {
                dateOfWednesday = now;
            }

            List<IGrouping<int, ClassDbModel>> classes = null;

            using (var db = await _dbContext.CreateDbContextAsync())
            {
                classes = await db.Classes.AsNoTracking()
                                          .Include(x => x.Group)
                                          .GroupBy(x => x.DayNumber)
                                          .ToListAsync();
            }

            GroupByDayModel dayAtTop = null;

            foreach (var group in classes)
            {
                GroupByDayModel addGroup = null;

                string dayName = ((DayOfWeek)group.Key).ToString();

                List<GroupInDayModel> groups = new();

                foreach(var item in group.OrderBy(x => x.LessonNumber))
                {
                    groups.Add(new(item, now, _scheduleService));
                }

                if (dayNow == group.Key)
                {
                    addGroup = new(dayName, true, groups);
                    dayAtTop = addGroup;
                }
                else
                {
                    addGroup = new(dayName, false, groups);
                }

                addGroup.AddDate(group.Key, dateOfWednesday);

                result.Add(addGroup);
            }

            if (dayAtTop is not null)
            {
                result.Remove(dayAtTop);
                result.Insert(0, dayAtTop);
            }

            return Ok(new ResponseModel<GetGroupsData, IError>
            {
                Data = new GetGroupsData(result)
            });
        }

        [HttpGet]
        [EnableCors]
        [Route("schedule")]
        public async Task<ActionResult<ResponseModel<GetScheduleData, IError>>> Schedule()
        {
            var schedule = _scheduleService.GetSchedule();

            return Ok(new ResponseModel<GetScheduleData, IError>
            {
                Data = new GetScheduleData(schedule)
            });
        }

        [HttpPost]
        [EnableCors]
        [Route("updateSchedule")]
        public async Task<ActionResult<ResponseModel<StatusData, IError>>> SetSchedule([FromBody] ScheduleRequestModel body)
        {
            _scheduleService.SetSchedule(new ScheduleModel(body));

            return Ok(new ResponseModel<StatusData, IError>()
            {
                Data = new StatusData("Ok")
            });
        }
    }
}
