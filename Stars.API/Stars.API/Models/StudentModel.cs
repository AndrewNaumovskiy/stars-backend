using Stars.API.Models.DbModels;

namespace Stars.API.Models;

public class StudentModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int GroupFk { get; set; }

    public string Impression { get; set; }
    public int StudentType { get; set; } // 1 - star, 0 - usual, -1 - remote, -2 - expelled

    public MarkDbModel Mark { get; set; }

    public StudentModel(StudentDbModel dbItem)
    {
        Id = dbItem.Id;
        FirstName = dbItem.FirstName;
        LastName = dbItem.LastName;
        MiddleName = dbItem.MiddleName;
        GroupFk = dbItem.GroupFk;

        Impression = dbItem.Impression;
        StudentType = dbItem.StudentType;

        Mark = dbItem.Marks.FirstOrDefault();
    }
}
