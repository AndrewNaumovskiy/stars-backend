using Stars.API.Models.DbModels;

namespace Stars.API.Models;

public class GroupByDayModel
{
    public string Day { get; set; }
    public List<GroupInDayModel> Groups { get; set; }

    public GroupByDayModel(string day, List<GroupInDayModel> groups)
    {
        Day = day;
        Groups = groups;
    }

    Dictionary<int, (TimeOnly, TimeOnly)> lessonDate = new()
    {
        { 1, (new TimeOnly(9, 0, 0), new TimeOnly(10, 30, 0)) },
        { 2, (new TimeOnly(10, 40, 0), new TimeOnly(12, 10, 0)) },
        { 3, (new TimeOnly(12, 20, 0), new TimeOnly(13, 50, 0)) },
        { 4, (new TimeOnly(14, 0, 0), new TimeOnly(15, 30, 0)) },
    };

    public void CalculateClassStatus(DateTime now, int dayNumber)
    {
        var timeOnly = TimeOnly.FromDateTime(now);

        foreach (var item in Groups)
        {
            var (startDate, endDate) = lessonDate[item.LessonNumber];

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

    public GroupByDayModel() { }
}



public class GroupInDayModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LessonNumber { get; set; }

    public ClassStatus Status { get; set; } = ClassStatus.NotStarted;

    public GroupInDayModel(ClassDbModel dbModel, DateTime now)
    {
        Id = dbModel.Group.Id;
        Name = dbModel.Group.Name;
        LessonNumber = dbModel.LessonNumber;
    }

    public GroupInDayModel() { }
}

public enum ClassStatus
{
    NotStarted = 0,
    InProgress = 1,
    Finished = 2
}