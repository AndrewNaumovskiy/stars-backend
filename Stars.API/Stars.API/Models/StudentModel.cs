using Stars.API.Models.DbModels;
using System.Linq.Expressions;

namespace Stars.API.Models;

public class StudentModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int GroupFk { get; set; }

    public string Impression { get; set; }
    public bool IsFavorite { get; set; }

    public MarkDbModel Mark { get; set; }

    public StudentModel(StudentDbModel dbItem)
    {
        Id = dbItem.Id;
        FirstName = dbItem.FirstName;
        LastName = dbItem.LastName;
        MiddleName = dbItem.MiddleName;
        GroupFk = dbItem.GroupFk;

        Impression = dbItem.Impression;
        IsFavorite = dbItem.IsFavorite == 1;

        Mark = dbItem.Marks.FirstOrDefault();
    }
}
