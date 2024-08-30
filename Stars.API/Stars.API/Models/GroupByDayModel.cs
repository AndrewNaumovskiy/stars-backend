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

    // TODO: implement
    public void CalculateClassStatus(DateTime now)
    {
        foreach(var item in Groups)
        {

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

    public GroupInDayModel(){}
}

public enum ClassStatus
{
    NotStarted = 0,
    InProgress = 1,
    Finished = 2
}