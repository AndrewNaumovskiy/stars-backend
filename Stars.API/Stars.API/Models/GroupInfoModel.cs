using Stars.API.Models.DbModels;

namespace Stars.API.Models;

public class GroupInfoModel
{
    public string GroupName { get; set; }
    public StudentModel Head { get; set; }
    public string TelegramLink { get; set; }

    public GroupInfoModel(GroupDbModel dbModel)
    {
        GroupName = $"{dbModel.Name} група";
        Head = new (dbModel.StudentHead);
        TelegramLink = dbModel.TelegramLink;
    }
}
