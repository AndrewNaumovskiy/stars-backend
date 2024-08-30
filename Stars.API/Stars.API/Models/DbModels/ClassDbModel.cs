using System.ComponentModel.DataAnnotations.Schema;

namespace Stars.API.Models.DbModels;

[Table("classes")]
public class ClassDbModel
{
    public int Id { get; set; }
    public int GroupFk { get; set; }
    public int DayNumber { get; set; }
    public int LessonNumber { get; set; }

    public GroupDbModel Group { get; set; }
}
