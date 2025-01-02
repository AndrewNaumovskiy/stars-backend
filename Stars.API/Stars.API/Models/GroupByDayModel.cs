using Stars.API.Helpers;
using Stars.API.Models.DbModels;

namespace Stars.API.Models;

public class GroupByDayModel
{
    public string DayOfWeek { get; set; }
    public string Date { get; set; }
    public string DateReschedule { get; set; }
    public bool IsToday { get; set; }

    public List<GroupInDayModel> Groups { get; set; }

    public GroupByDayModel(string day, bool isToday, List<GroupInDayModel> groups)
    {
        DayOfWeek = day;
        IsToday = isToday;
        Groups = groups;
    }

    public void CalculateClassStatus(DateTime now, int dayNumber, ScheduleService scheduleService)
    {
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");

        now = TimeZoneInfo.ConvertTime(now, timeZone);

        var timeOnly = TimeOnly.FromDateTime(now);

        var schedule = scheduleService.Schedule();

        foreach (var item in Groups)
        {
            var (startDate, endDate) = schedule[item.LessonNumber];

            if (endDate < timeOnly)
            {
                item.Status = ClassStatus.Finished;
            }
            else if (startDate < timeOnly && timeOnly < endDate)
            {
                item.Status = ClassStatus.InProgress;
            }
        }
    }

    public void AddDate(int dateOfWeek, DateTime dateOfWednesday)
    {
        if (dateOfWeek == 3)
        {
            Date = dateOfWednesday.ToString("dd.MM.yyyy");
            DateReschedule = dateOfWednesday.AddDays(-14).ToString("dd.MM.yyyy");
        }
        else if (dateOfWeek == 4)
        {
            Date = dateOfWednesday.AddDays(1).ToString("dd.MM.yyyy");
            DateReschedule = dateOfWednesday.AddDays(-13).ToString("dd.MM.yyyy");
        }
    }

    public GroupByDayModel() { }
}



public class GroupInDayModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LessonNumber { get; set; }

    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public int Cabinet { get; set; }
    public string Classes { get; set; }

    public ClassStatus Status { get; set; } = ClassStatus.NotStarted;

    public GroupInDayModel(ClassDbModel dbModel, DateTime now, ScheduleService scheduleService)
    {
        Id = dbModel.Group.Id;
        Name = dbModel.Group.Name;
        LessonNumber = dbModel.LessonNumber;

        StartTime = scheduleService.Schedule()[dbModel.LessonNumber].Item1.ToString();
        EndTime = scheduleService.Schedule()[dbModel.LessonNumber].Item2.ToString();

        Classes = dbModel.Classes;
        Cabinet = dbModel.Cabinet;
    }

    public GroupInDayModel() { }
}

public enum ClassStatus
{
    NotStarted = 0,
    InProgress = 1,
    Finished = 2
}