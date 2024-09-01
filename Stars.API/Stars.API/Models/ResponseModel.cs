using Stars.API.Models.DbModels;

namespace Stars.API.Models;

public class ResponseModel<D, E>
        where D : IData
        where E : IError
{
    public D Data { get; set; }
    public E Error { get; set; }
}

public interface IError { }

public class Error : IError
{
    public string Description { get; set; }
    public Error(string desc)
    {
        Description = desc;
    }
}

public interface IData { }

public class GetStudentByIdData : IData
{
    public StudentDbModel Student { get; set; }
    public GetStudentByIdData(StudentDbModel student)
    {
        Student = student;
    }
}
public class GetStudentsByGroupIdData : IData
{
    public List<StudentModel> Students { get; set; }
    public GroupInfoModel GroupInfo { get; set; }
    public GetStudentsByGroupIdData(List<StudentModel> students, GroupInfoModel groupInfo)
    {
        Students = students;
        GroupInfo = groupInfo;
    }
}





public class GetGroupsData : IData
{
    public List<GroupByDayModel> Groups { get; set; }
    public GetGroupsData(List<GroupByDayModel> groups)
    {
        Groups = groups;
    }
}

public class LoginData : IData
{
    public string Licence { get; set; }
    public LoginData(string licence)
    {
        Licence = licence;
    }
}

public class StatusData : IData
{
    public string Status { get; set; }
    public StatusData(string status)
    {
        Status = status;
    }
}

